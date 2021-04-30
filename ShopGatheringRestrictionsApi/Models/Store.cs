﻿using System;
using System.Collections.Generic;

namespace ShopGatheringRestrictionsApi.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Section> Sections { get; set; }
    }
}
