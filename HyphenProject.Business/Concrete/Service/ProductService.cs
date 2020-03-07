using HyphenProject.Business.Abstract;
using HyphenProject.Business.ElasticSearchOptions.Abstract;
using HyphenProject.Business.ElasticSearchOptions.Conrete;
using HyphenProject.Business.Helper;
using HyphenProject.Business.ObjectDtos.AggregationModels;
using HyphenProject.Business.ObjectDtos.Post;
using HyphenProject.Business.ObjectDtos.Product;
using HyphenProject.Core.Consts;
using HyphenProject.DataAccess.Abstract;
using HyphenProject.Entities.Models;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HyphenProject.Business.Concrete.Manager
{
    public class ProductService : IProductService
    {
        public IElasticClient EsClient { get; set; }
        private readonly IProductDal _productDal;
        private readonly IElasticSearchService _elasticSearchService;
        public ProductService(IProductDal productDal, IElasticSearchService elasticSearchService)
        {
            _productDal = productDal;
            _elasticSearchService = elasticSearchService;
        }
        public Product Insert(Product product)
        {
            _productDal.Add(product);
            return product;
        }

        public async Task<bool> ProductAddOrUpdateElasticIndexAsync(ProductElasticIndexDto productElasticIndexDto)
        {
            try
            {
                await _elasticSearchService.CreateProductIndexAsync<ProductElasticIndexDto, int>(ElasticSearchItemsConst.ProductIndexName);
                await _elasticSearchService.AddOrUpdateAsync<ProductElasticIndexDto, int>(ElasticSearchItemsConst.ProductIndexName, productElasticIndexDto);
                return await Task.FromResult<bool>(true);
            }
            catch (Exception ex)
            {
                return await Task.FromException<bool>(ex);
            }
        }
        public async Task<ProductOutput> GetSearchAsync(ProductSearchInput productSearchInput)
        {
            if (string.IsNullOrEmpty(productSearchInput.Brand) && string.IsNullOrEmpty(productSearchInput.ScreenSize))
                throw new InvalidArgumentException();
            try
            {
                var indexName = ElasticSearchItemsConst.ProductIndexName;
                var searchQuery = new Nest.SearchDescriptor<ProductElasticIndexDto>();
                string term = "";
                if (productSearchInput.ScreenSize != null)
                {
                    term = "screensizes";
                    searchQuery
                    .Query(x => x
                   .Terms(c => c
                       .Verbatim()
                       .Field(p => p.ScreenSize)
                       .Terms(productSearchInput.ScreenSize.ToLower())
                   ))
                       .Aggregations(a => a
                            .Terms(term, t => t
                            .Field("screenSize.keyword")
                            .MinimumDocumentCount(1))
                    );
                }
                if (!String.IsNullOrEmpty(productSearchInput.Brand))
                {
                    term = "searchingArea";
                    searchQuery
                    .Query(x => x.Match(m =>
                            m.Field(f => f.SearchingArea)
                            .Query(productSearchInput.Brand.ToLower())
                            .Analyzer("ngram_analyzer")
                            )
                    )
                     .Aggregations(a => a
                            .Terms(term, t => t
                            .Field("screenSize.keyword")
                            .MinimumDocumentCount(1))
                    ); ;
                }

                var searchResultData = await _elasticSearchService.SimpleSearchAsync<ProductElasticIndexDto, int>(indexName, searchQuery);

                var aggregationResponse = searchResultData.Aggregations.Terms(term);


                var aggregationList = new List<Aggregation>();

                foreach (var item in aggregationResponse.Buckets)
                {
                    var aggregation = new Aggregation();
                    aggregation.Key = item.Key;
                    aggregation.DocCount = item.DocCount;
                    aggregationList.Add(aggregation);
                }
                var productElasticIndexList = from opt in searchResultData.Documents
                                              select new ProductElasticIndexDto
                                              {
                                                  SearchingArea = opt.SearchingArea,
                                                  Id = opt.Id,
                                                  Brand = opt.Brand,
                                                  ModelName = opt.ModelName,
                                                  ScreenSize = opt.ScreenSize,
                                                  Price = opt.Price,
                                                  Stock = opt.Stock,
                                              };

                var output = new ProductOutput();
                output.AggregationList = aggregationList;
                output.ProductElasticIndexDtoList = productElasticIndexList.ToList();
                return await Task.FromResult(output);
            }
            catch (Exception ex)
            {
                return await Task.FromException<ProductOutput>(ex);
            }
        }
        public List<Product> GetAllList()
        {
            return _productDal.GetList();
        }
        public Product GetByItem(object item)
        {
            throw new NotImplementedException();
        }
        public Product Update(Product entity)
        {
            throw new NotImplementedException();
        }
        public void Remove(Product entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> BulkInsertAsync(List<ProductElasticIndexDto> productList)
        {
            await _elasticSearchService.CreateProductIndexAsync<ProductElasticIndexDto, int>(ElasticSearchItemsConst.ProductIndexName);
            await _elasticSearchService.BulkAddOrUpdate<ProductElasticIndexDto, int>(ElasticSearchItemsConst.ProductIndexName, productList);
            return await Task.FromResult<bool>(true);
        }
    }
}