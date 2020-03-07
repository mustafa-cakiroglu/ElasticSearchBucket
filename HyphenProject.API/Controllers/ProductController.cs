using AutoMapper;
using HyphenProject.Business.Abstract;
using HyphenProject.Business.ObjectDtos;
using HyphenProject.Business.ObjectDtos.Post;
using HyphenProject.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using System.Linq;
using HyphenProject.Business.ObjectDtos.Product;
using HyphenProject.Business.Helper;

namespace HyphenProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IMapper mapper, IProductService productService)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpPost("newproduct")]
        public async Task<Product> AddNewProductAsync(ProductInputDto productInputDto)
        {
            try
            {
                var createdProductInfo = _productService.Insert(_mapper.Map<Product>(productInputDto));
                var indexCreateItem = GetElasticIndexItem(createdProductInfo);
                PostAddOrUpdateElasticIndexAsync(indexCreateItem);
                return await Task.FromResult(createdProductInfo);
            }
            catch (Exception ex)
            {

                return await Task.FromException<Product>(ex);
            }
        }

        [HttpPost("search")]
        public async Task<ProductOutput> GetSearchAsync(ProductSearchInput productSearchInput)
        {
            return await Task.FromResult(_productService.GetSearchAsync(productSearchInput).Result);

        }

        [HttpGet("bulkInsertAsync")]
        public async Task<bool> BulkAddOrUpdateAsync()
        {

            var productList = _productService.GetAllList();
            var productElasticIndexList = new List<ProductElasticIndexDto>();
            foreach (var product in productList)
            {
                productElasticIndexList.Add(GetElasticIndexItem(product));
            }
            await _productService.BulkInsertAsync(productElasticIndexList);
            return await Task.FromResult(true);
            throw new InvalidArgumentException();

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async void PostAddOrUpdateElasticIndexAsync(ProductElasticIndexDto productDto)
        {
            await _productService.ProductAddOrUpdateElasticIndexAsync(productDto);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private ProductElasticIndexDto GetElasticIndexItem(Product productInfo)
        {
            return new ProductElasticIndexDto
            {
                Id = productInfo.Id,
                Brand = productInfo.Brand,
                ModelName = productInfo.ModelName,
                ScreenSize = productInfo.Size,
                Price = productInfo.Price,
                Stock = productInfo.Stock,
                SearchingArea = productInfo.Brand,
            };
        }
    }
}
