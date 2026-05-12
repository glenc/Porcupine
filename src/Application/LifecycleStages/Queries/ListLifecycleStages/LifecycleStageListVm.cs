namespace Porcupine.Application.LifecycleStages.Queries.ListLifecycleStages;

public record LifecycleStageListVm(IReadOnlyCollection<LifecycleStageSummaryDto> Items)
{
}