using Microsoft.Extensions.Configuration;
using HyphenProject.Business.ElasticSearchOptions.Abstract;

namespace HyphenProject.Business.ElasticSearchOptions.Conrete
{
    public class ElasticSearchConfigration : IElasticSearchConfigration
    {
        public IConfiguration Configuration { get; }
        public ElasticSearchConfigration(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public string ConnectionString { get { return Configuration.GetSection("ElasticSearchOptions:ConnectionString:HostUrls").Value; } }
    }
}