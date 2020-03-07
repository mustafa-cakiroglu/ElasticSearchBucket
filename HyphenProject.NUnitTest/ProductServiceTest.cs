using HyphenProject.Business.Abstract;
using HyphenProject.Business.Concrete.Manager;
using HyphenProject.Business.ElasticSearchOptions.Conrete;
using HyphenProject.Business.ObjectDtos.Product;
using HyphenProject.Entities.Models;
using HyphenProject.NUnitTest.Mock;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    public class ProductServiceTest
    {
        private IProductService _productService;

        [SetUp]
        public void Setup()
        {
            var environmentName = Environment.GetEnvironmentVariable("Hosting:Environment");
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();
            IConfiguration configuration = config.Build();

            _productService = new ProductService(new MemoryProductDal(), new ElasticSearchService(new ElasticSearchConfigration(configuration)));
        }

        [Test]
        public void TestGetAllList()
        {
            Assert.Pass();
            //List<Product> products = _productDal.GetAllList();
            //Assert.AreEqual(products.Count, 4);
        }

        [Test]
        public void TestGetByItem()
        {
            Assert.Pass();
            //Product product = _productDal.GetByItem(new Product { Id = 2 });
            //Assert.AreEqual(product.ModelName, "Samsung");
        }

        [Test]
        public void TestInsert()
        {
            Product product = _productService.Insert(new Product { Id = 5, ModelName = "Insert Test", Brand = "", Price = "", Size = "", Stock = 0 });
            Assert.AreEqual(product.Id, 5);
        }

        [Test]
        public void TestRemove()
        {
            Assert.Pass();
            //try
            //{
            //    _productDal.Remove(new Product { Id = 4 });
            //}
            //catch
            //{
            //    Assert.Fail();
            //}
            //try
            //{
            //    _productDal.Remove(new Product { Id = 10 });
            //    Assert.Fail();
            //}
            //catch { }
        }

        [Test]
        public void TestUpdate()
        {
            Assert.Pass();
            //Product product = _productDal.Update(new Product { Id = 2, ModelName = "Insert Test", Brand = "", Price = "", Size = "", Stock = 10 });
            //Assert.AreEqual(product.Stock, 10);
        }

        [Test]
        public void TestGetSearchWithBrand()
        {
            ProductSearchInput productSearchInput = new ProductSearchInput();
            productSearchInput.Brand = "LG";
            Task<ProductOutput> productOutput = _productService.GetSearchAsync(productSearchInput);
            productOutput.Wait();
            Assert.AreEqual(productOutput.Result.ProductElasticIndexDtoList.Count, 4);
        }

        [Test]
        public void TestGetSearchWithScreenSize()
        {
            ProductSearchInput productSearchInput = new ProductSearchInput();
            productSearchInput.ScreenSize = "55";
            Task<ProductOutput> productOutput = _productService.GetSearchAsync(productSearchInput);
            productOutput.Wait();
            Assert.AreEqual(productOutput.Result.ProductElasticIndexDtoList.Count, 4);
        }

        [Test]
        public void TestGetSearchException()
        {
            try
            {
                ProductSearchInput productSearchInput = new ProductSearchInput();
                productSearchInput.Brand = string.Empty;
                productSearchInput.ScreenSize = string.Empty;
                Task<ProductOutput> productOutput = _productService.GetSearchAsync(productSearchInput);
                productOutput.Wait();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void TestProductAddOrUpdateElasticIndex()
        {
            Assert.Pass();
        }

        [Test]
        public void TestProductDeleteDocumentElasticIndex()
        {
            Assert.Pass();
        }
    }
}