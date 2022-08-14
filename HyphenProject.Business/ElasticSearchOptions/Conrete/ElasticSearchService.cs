using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HyphenProject.Business.ElasticSearchOptions.Abstract;
using HyphenProject.Business.ObjectDtos.Post;
using Nest;

namespace HyphenProject.Business.ElasticSearchOptions.Conrete
{
    public class ElasticSearchService : IElasticSearchService
    {

        public IElasticClient ElasticSearchClient { get; set; }
        private readonly IElasticSearchConfigration _elasticSearchConfigration;
        public ElasticSearchService(IElasticSearchConfigration elasticSearchConfigration)
        {
            _elasticSearchConfigration = elasticSearchConfigration;
            ElasticSearchClient = GetClient();
        }
        private ElasticClient GetClient()
        {
            var str = _elasticSearchConfigration.ConnectionString;
            var strs = str.Split('|');
            var nodes = strs.Select(s => new Uri(s)).ToList();

            var connectionString = new ConnectionSettings(new Uri(str))
                .DisablePing()
                .SniffOnStartup(false)
                .SniffOnConnectionFault(false);
            return new ElasticClient(connectionString);
        }
        public virtual async Task CreateIndexAsync<T, TKey>(string indexName) where T : ElasticEntity<TKey>
        {
            var exis = await ElasticSearchClient.IndexExistsAsync(indexName);
            if (exis.Exists)
                return;
            var newName = indexName + DateTime.Now.Ticks;
            var result = await ElasticSearchClient
                .CreateIndexAsync(newName,
                    ss =>
                        ss.Index(newName)
                            .Settings(
                                o => o.NumberOfShards(4).NumberOfReplicas(2).Setting("max_result_window", int.MaxValue)
                                         .Analysis(a => a
                                            .TokenFilters(tkf => tkf.AsciiFolding("my_ascii_folding", af => af.PreserveOriginal(true)))
                                            .Analyzers(aa => aa
                                            .Custom("turkish_analyzer", ca => ca
                                             .Filters("lowercase", "my_ascii_folding")
                                             .Tokenizer("edge_ngram")
                                             ))
                                         )
                        )
                            .Mappings(m => m.Map<T>(mm => mm.AutoMap()
                            .Properties(p => p
                 .Text(t => t.Name(n => n.SearchingArea)
                .Analyzer("turkish_analyzer")
            )))));
            if (result.Acknowledged)
            {
                await ElasticSearchClient.AliasAsync(al => al.Add(add => add.Index(newName).Alias(indexName)));
                return;
            }
            throw new ElasticSearchException($"Create Index {indexName} failed : :" + result.ServerError.Error.Reason);
        }



        public virtual async Task CreateProductIndexAsync<T, TKey>(string indexName) where T : class
        {
            var exist = await ElasticSearchClient.IndexExistsAsync(indexName);
            if (exist.Exists)
                return;
            var newName = indexName;

            var createIndexResponse = ElasticSearchClient.CreateIndex(newName, c => c

                .Mappings(map => map
                    .Map<ProductElasticIndexDto>(
                        m => m.Properties(
                            p => p
                            //.Text(t => t.Name(n => n.SearchingArea).Analyzer("ngram_analyzer")) //.Analyzer("ngram_analyzer")
                            .Text(t => t.Name(n => n.Size).Analyzer("standard_analyzer"))
                            )
                    ))
                .Settings(st => st
                    .Analysis(an => an
                        .Analyzers(anz => anz
                            .Custom("ngram_analyzer", cc => cc
                                .Tokenizer("ngram_tokenizer"))
                            .Custom("standard_analyzer", sc => sc
                                .Tokenizer("standard_tokenizer"))
                            )
                        .Tokenizers(tz => tz
                            .NGram("ngram_tokenizer", td => td
                                .MinGram(2)
                                .MaxGram(20)
                                .TokenChars(
                                    TokenChar.Letter,
                                    TokenChar.Digit,
                                    TokenChar.Punctuation,
                                    TokenChar.Symbol
                                )
                            )
                            .Standard("standard_tokenizer", sa => sa)
                        )
                    )
                )
            );
        }

        public virtual async Task AddOrUpdateAsync<T, TKey>(string indexName, T model) where T : ElasticEntity<TKey>
        {
            var exis = ElasticSearchClient.DocumentExists(DocumentPath<T>.Id(new Id(model)), dd => dd.Index(indexName));

            if (exis.Exists)
            {
                var result = await ElasticSearchClient.UpdateAsync(DocumentPath<T>.Id(new Id(model)),
                    ss => ss.Index(indexName).Doc(model).RetryOnConflict(3));

                if (result.ServerError == null) return;
                throw new ElasticSearchException($"Update Document failed at index{indexName} :" + result.ServerError.Error.Reason);
            }
            else
            {
                var result = await ElasticSearchClient.IndexAsync(model, ss => ss.Index(indexName));
                if (result.ServerError == null) return;
                throw new ElasticSearchException($"Insert Document failed at index {indexName} :" + result.ServerError.Error.Reason);
            }
        }

        public virtual async Task<ISearchResponse<T>> SimpleSearchAsync<T, TKey>(string indexName, SearchDescriptor<T> query) where T : ElasticEntity<TKey>
        {
            query.Index(indexName);
            var response = await ElasticSearchClient.SearchAsync<T>(query);
            return response;
        }
     
        public async Task BulkAddOrUpdate<T, TKey>(string indexName, List<T> list) where T : ElasticEntity<TKey>
        {
            var bulk = new BulkRequest(indexName)
            {
                Operations = new List<IBulkOperation>()
            };
            foreach (var item in list)
            {
                bulk.Operations.Add(new BulkIndexOperation<T>(item));
            }
            var response = await ElasticSearchClient.BulkAsync(bulk);
            if (response.Errors)
                throw new ElasticSearchException($"Bulk InsertOrUpdate Docuemnt failed at index {indexName} :{response.ServerError.Error.Reason}");
        }

        public virtual async Task CrateIndexAsync(string indexName)
        {
            var exis = await ElasticSearchClient.IndexExistsAsync(indexName);
            if (exis.Exists)
                return;
            var newName = indexName + DateTime.Now.Ticks;
            var result = await ElasticSearchClient
                .CreateIndexAsync(newName,
                    ss =>
                        ss.Index(newName)
                            .Settings(
                                o => o.NumberOfShards(4).NumberOfReplicas(2).Setting("max_result_window", int.MaxValue)));
            if (result.Acknowledged)
            {
                await ElasticSearchClient.AliasAsync(al => al.Add(add => add.Index(newName).Alias(indexName)));
                return;
            }
            throw new ElasticSearchException($"Create Index {indexName} failed :" + result.ServerError.Error.Reason);
        }
      
    }
}