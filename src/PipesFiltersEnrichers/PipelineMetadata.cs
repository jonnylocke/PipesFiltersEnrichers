using System;

namespace PipesFiltersEnrichers
{
    public class PipelineMetadata
    {
        public string Filter { get; set; }
        public TimeSpan FilterDeltaTime { get; set; }

        public int InboundDataCount { get; set; }
        public int OutboundDataCount { get; set; }

        public override string ToString()
        {
            return $"Filter: {Filter}, DeltaTime: '{FilterDeltaTime}', InboundDataCount: {InboundDataCount} , OutboundNumberOfKeys: {OutboundDataCount}";
        }
    }
}