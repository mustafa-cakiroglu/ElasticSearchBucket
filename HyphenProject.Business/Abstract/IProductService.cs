using HyphenProject.Business.ObjectDtos.Post;
using HyphenProject.Business.ObjectDtos.Product;
using HyphenProject.Core.Business.EntityRepository;
using HyphenProject.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HyphenProject.Business.Abstract
{
    public interface IProductService : IEntityCommonRepository<Product>
    {
        Task<ProductOutput> GetSearchAsync(ProductSearchInput productSearchInput);
        Task<bool> ProductAddOrUpdateElasticIndexAsync(ProductElasticIndexDto postElasticIndexDto);
        Task<bool> BulkInsertAsync(List<ProductElasticIndexDto> productList);
    }
}