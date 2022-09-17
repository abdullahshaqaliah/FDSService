using AutoMapper;
using FDSService.Clients;
using FDSService.Clients.Dtos;
using FDSService.DataLookups;
using FDSService.DataLookups.Dtos;
using FDSService.Packages;
using FDSService.Packages.Dtos;
using System.Linq;

namespace FDSService;

public class FDSServiceApplicationAutoMapperProfile : Profile
{
    public FDSServiceApplicationAutoMapperProfile()
    {
        PackageMapping();
        ClientMapping();
    }

    private void PackageMapping()
    {
        CreateMap<Package, PackageDto>();
        CreateMap<CreateOrUpdatePackageDto, Package>();

        CreateMap<PackageVersion, PackageVersionDto>()
            .ForMember(dst => dst.Package, opt => opt.MapFrom(src => src.Package.Name))
            .ForMember(dst => dst.Countries, opt => opt.MapFrom(src => src.Countries.Select(c => c.CountryId)))
            .ForMember(dst => dst.Channels, opt => opt.MapFrom(src => src.Channels.Select(c => c.ChannelId)));
        CreateMap<CreateOrUpdatePackageVersionDto, PackageVersion>()
           .ForMember(dst => dst.Countries, opt => opt.MapFrom(src => src.Countries.Select(c=> new PackageVersionCountry { CountryId=c })))
           .ForMember(dst => dst.Channels, opt => opt.MapFrom(src => src.Channels.Select(c => new PackageVersionChannel { ChannelId = c })));
    }

    private void ClientMapping()
    {
        CreateMap<ClientPackage, ClientPackageDto>()
            .ForMember(dst => dst.Package, opt => opt.MapFrom(src => src.Package.Name))
            .ForMember(dst => dst.CurrentVersion, opt => opt.MapFrom(src => src.CurrentVersion.Name));
        CreateMap<CreateClientPackageDto, ClientPackage>();
        CreateMap<PackageVersion, ClientPackageVersionDownloadDto>()
        .ForMember(dst => dst.FileName, opt => opt.MapFrom(src => src.Attachment !=null ? src.Attachment.Name:""));

    }

}
