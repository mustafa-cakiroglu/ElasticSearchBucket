using HyphenProject.Business.ElasticSearchOptions.Conrete;
using HyphenProject.Business.ObjectDtos.AggregationModels;
using HyphenProject.Core.Dtos;
using System;
using System.Collections.Generic;

namespace HyphenProject.Business.ObjectDtos.Post
{
    public class ProductElasticIndexDto : ElasticEntity<int>, IDto
    {
        public string Brand { get; set; }
        public string ModelName { get; set; }
        public string ScreenSize { get; set; }
        public string Price { get; set; }
        public int Stock { get; set; }
        public string SearchText { get; set; }
        public int Size { get; set; }
    }
}