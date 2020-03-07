using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyphenProject.Business.ObjectDtos.AggregationModels
{
    public class Aggregation
    {
        public long? DocCount { get; set; }
        public string Key { get; set; }
    }
}
