using Porcupine.Domain.Entities;

namespace Porcupine.Application.LifecycleStages.Queries.ListLifecycleStages;

public record LifecycleStageSummaryDto(int Id, string Name, double Order)
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<LifecycleStage, LifecycleStageSummaryDto>();
        }
    }
}