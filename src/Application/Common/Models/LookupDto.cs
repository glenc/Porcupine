using System.Security.Cryptography.X509Certificates;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.Common.Models;

public record LookupDto
{
    public int Id { get; init; }
    public string? Name { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Industry, LookupDto>();
            CreateMap<MarketSegment, LookupDto>();
            CreateMap<LifecycleStage, LookupDto>();
        }
    }
}
