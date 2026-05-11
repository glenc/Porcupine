using Porcupine.Domain.Entities;

namespace Porcupine.Application.MarketSegments.Queries.ListMarketSegments;

public record MarketSegmentSummaryDto(int Id, string Name, string Description)
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<MarketSegment, MarketSegmentSummaryDto>();
        }
    }
}