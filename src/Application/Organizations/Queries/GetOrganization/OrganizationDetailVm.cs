using Porcupine.Application.Common.Models;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.Organizations.Queries.GetOrganization;

public record OrganizationDetailVm(int Id, string Name, LookupDto LifecycleStage, LookupDto? Industry, LookupDto? MarketSegment)
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Organization, OrganizationDetailVm>();
        }
    }
}