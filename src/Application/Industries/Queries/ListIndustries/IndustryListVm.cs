namespace Porcupine.Application.Industries.Queries.ListIndustries;

public record IndustryListVm(IReadOnlyCollection<IndustrySummaryDto> Items)
{
}