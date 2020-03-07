using HyphenProject.Core.DataAccess.EntityFramework;
using HyphenProject.DataAccess.Abstract;
using HyphenProject.Entities.Models;

namespace HyphenProject.DataAccess.Concrete.EntityFramework
{
    public class ProductDal : EntityRepositoryBase<Product, HyphenProjectDbContext>, IProductDal
    {

    }
}