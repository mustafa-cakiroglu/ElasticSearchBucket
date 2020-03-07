using HyphenProject.Business.ObjectDtos.AggregationModels;
using HyphenProject.Business.ObjectDtos.Post;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyphenProject.Business.ObjectDtos.Product
{
    public class ProductOutput
    {
        public List<ProductElasticIndexDto> ProductElasticIndexDtoList { get; set; }
        public List<Aggregation> AggregationList { get; set; }
    }
}
