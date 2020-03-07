using HyphenProject.Core.Dtos;
using HyphenProject.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyphenProject.Business.ObjectDtos
{
    public class ProductInputDto : IDto
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
