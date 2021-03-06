﻿using System.Collections.Generic;
using PipesFiltersEnrichers.Models;

namespace PipesFiltersEnrichers
{
    internal class MyResultObj
    {
        public MyResultObj()
        {
            PipelineMetadata = new List<PipelineMetadata>();
        }
        public IEnumerable<Patient> InboundData { get; set; }
        public IEnumerable<Patient> OutboundData { get; set; }
        public List<PipelineMetadata> PipelineMetadata { get; set; }
    }
}