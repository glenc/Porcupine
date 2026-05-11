using Porcupine.Domain.Entities;

namespace Porcupine.Application.Industries.Queries.ListIndustries;

public record IndustrySummaryDto(int Id, string Name, string Description)
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Industry, IndustrySummaryDto>();
        }
    }
}