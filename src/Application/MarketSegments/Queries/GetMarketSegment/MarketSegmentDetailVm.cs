using Porcupine.Domain.Entities;

namespace Porcupine.Application.MarketSegments.Queries.GetMarketSegment;

public record MarketSegmentDetailVm(int Id, string Name, string Description)
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<MarketSegment, MarketSegmentDetailVm>();
        }
    }
}