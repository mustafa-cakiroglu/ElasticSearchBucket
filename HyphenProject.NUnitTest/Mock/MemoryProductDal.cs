using HyphenProject.DataAccess.Abstract;
using HyphenProject.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HyphenProject.NUnitTest.Mock
{
    internal class MemoryProductDal : IProductDal
    {
        private readonly IList<Product> products;

        public MemoryProductDal()
        {
            products = new List<Product>();
            fillMockData();
        }

        public Product Add(Product entity)
        {
            if (products.Any(x => x.Id == entity.Id))
            {
                throw new Exception("MemoryProductDal.Add");
            }
            products.Add(entity);
            return entity;
        }

        public void Delete(Product entity)
        {
            if (!products.Any(x => x.Id == entity.Id))
            {
                throw new Exception("MemoryProductDal.Delete");
            }
            products.Remove(products.First(x => x.Id == entity.Id));
        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            if (!products.AsQueryable().Any(filter))
            {
                throw new Exception("MemoryProductDal.Get");
            }
            return products.AsQueryable().First(filter);
        }

        public List<Product> GetList(Expression<Func<Product, bool>> filter = null)
        {
            if (filter != null)
            {
                return products.AsQueryable().Where(filter).ToList();
            }
            return products.ToList();
        }

        public Product Update(Product entity)
        {
            if (!products.Any(x => x.Id == entity.Id))
            {
                throw new Exception("MemoryProductDal.Update");
            }
            Product product = products.First(x => x.Id == entity.Id);
            product.Brand = entity.Brand;
            product.ModelName = entity.ModelName;
            product.Price = entity.Price;
            product.Size = entity.Size;
            product.Stock = entity.Stock;
            return product;
        }

        private void fillMockData()
        {
            products.Clear();

            products.Add(new Product
            {
                Id = 1,
                ModelName = "Vestel",
                Brand = "",
                Price = "",
                Size = "42",
                Stock = 10
            });
            products.Add(new Product
            {
                Id = 2,
                ModelName = "Samsung",
                Brand = "",
                Price = "",
                Size = "42",
                Stock = 5
            });
            products.Add(new Product
            {
                Id = 3,
                ModelName = "Sony",
                Brand = "",
                Price = "",
                Size = "42",
                Stock = 15
            });
            products.Add(new Product
            {
                Id = 4,
                ModelName = "Samsung",
                Brand = "",
                Price = "",
                Size = "55",
                Stock = 0
            });
        }
    }
}