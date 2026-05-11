namespace Porcupine.Application.MarketSegments.Queries.ListMarketSegments;

public record MarketSegmentListVm(IReadOnlyCollection<MarketSegmentSummaryDto> Items)
{
}