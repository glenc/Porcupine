using Porcupine.Domain.Entities;

namespace Porcupine.Application.Organizations.Queries.ListOrganizations;

public record OrganizationSummaryDto(int Id, string Name)
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Organization, OrganizationSummaryDto>();
        }
    }
}