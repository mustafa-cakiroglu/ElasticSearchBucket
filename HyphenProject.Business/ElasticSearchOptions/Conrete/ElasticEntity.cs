using HyphenProject.Business.ElasticSearchOptions.Abstract;
using Nest;

namespace HyphenProject.Business.ElasticSearchOptions.Conrete
{
    public class ElasticEntity<TEntityKey> : IElasticEntity<TEntityKey>
    {
        public virtual TEntityKey Id { get; set; }
        public virtual string SearchingArea { get; set; }
        public virtual double? Score { get; set; }
    } 
}