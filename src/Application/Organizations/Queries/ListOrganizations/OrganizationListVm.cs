namespace Porcupine.Application.Organizations.Queries.ListOrganizations;

public record OrganizationListVm(IReadOnlyCollection<OrganizationSummaryDto> Items)
{
}