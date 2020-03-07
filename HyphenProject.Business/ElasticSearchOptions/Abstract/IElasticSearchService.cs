using HyphenProject.Business.ElasticSearchOptions.Conrete;
using HyphenProject.Business.ObjectDtos.Post;
using HyphenProject.Entities.Models;
using Nest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HyphenProject.Business.ElasticSearchOptions.Abstract
{
    public interface IElasticSearchService
    {

        Task CreateIndexAsync<T, TKey>(string indexName) where T : ElasticEntity<TKey>;
        Task CreateProductIndexAsync<T, TKey>(string indexName) where T : class;
        Task AddOrUpdateAsync<T, TKey>(string indexName, T model) where T : ElasticEntity<TKey>;
        Task<ISearchResponse<T>> SimpleSearchAsync<T, TKey>(string indexName, SearchDescriptor<T> query) where T : ElasticEntity<TKey>;
        Task BulkAddOrUpdate<T, TKey>(string indexName, List<T> list) where T : ElasticEntity<TKey>;
        Task CrateIndexAsync(string indexName);
    }
}