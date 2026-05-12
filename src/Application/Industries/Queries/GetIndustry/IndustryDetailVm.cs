using Porcupine.Domain.Entities;

namespace Porcupine.Application.Industries.Queries.GetIndustry;

public record IndustryDetailVm(int Id, string Name, string Description)
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Industry, IndustryDetailVm>();
        }
    }
}