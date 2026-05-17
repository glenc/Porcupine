using Porcupine.Domain.Triggers;

namespace Porcupine.Application.Triggers.Queries.ListTriggers;

public record TriggerDto(string Name, string? DisplayName, string? Description)
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TriggerDescriptor, TriggerDto>();
        }
    }
}