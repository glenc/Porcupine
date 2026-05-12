using Porcupine.Domain.Entities;

namespace Porcupine.Application.LifecycleStages.Queries.GetLifecycleStage;

public record LifecycleStageDetailVm(int Id, string Name, double Order)
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<LifecycleStage, LifecycleStageDetailVm>();
        }
    }
}