using System;

namespace FDSService.Clients.Dtos;
public class GetClientPackageInputDto : PagedAndSortedResultRequestDto
{
    public Guid ClientId { get; set; }
}
