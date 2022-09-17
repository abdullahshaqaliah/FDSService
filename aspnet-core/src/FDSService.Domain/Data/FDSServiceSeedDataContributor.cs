using FDSService.Clients;
using FDSService.DataLookups;
using FDSService.Packages;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.MultiTenancy;
using IdentityRole = Volo.Abp.Identity.IdentityRole;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace FDSService.Data
{
    public class FDSServiceSeedDataContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Package, Guid> _packageRepository;
        private readonly IRepository<PackageVersion, Guid> _packageVersionRepository;



        private readonly IRepository<Channel, int> _channelRepository;
        private readonly IRepository<Country, int> _countryRepository;
        private readonly IRepository<ClientPackage, int> _clientPackageRepository;

        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IdentityUserManager _userManager;
        private readonly IdentityRoleManager _roleManager;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IOptions<IdentityOptions> IdentityOptions;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;


        public FDSServiceSeedDataContributor(IRepository<Package, Guid> packageRepository, IRepository<Channel, int> channelRepository, IRepository<Country, int> countryRepository, IIdentityRoleRepository roleRepository, IIdentityUserRepository userRepository, IdentityUserManager userManager, IdentityRoleManager roleManager, ILookupNormalizer lookupNormalizer, IOptions<IdentityOptions> identityOptions, IGuidGenerator guidGenerator, ICurrentTenant currentTenant, IRepository<ClientPackage, int> clientPackageRepository, IRepository<PackageVersion, Guid> packageVersionRepository)
        {
            _packageRepository = packageRepository;
            _channelRepository = channelRepository;
            _countryRepository = countryRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _lookupNormalizer = lookupNormalizer;
            IdentityOptions = identityOptions;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
            _clientPackageRepository = clientPackageRepository;
            _packageVersionRepository = packageVersionRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await PackageSeedDataAsync().ConfigureAwait(false);
            await ChannelSeedDataAsync().ConfigureAwait(false);
            await CountrySeedDataAsync().ConfigureAwait(false);
            await ClientSeedDataAsync().ConfigureAwait(false);
        }

        private async Task PackageSeedDataAsync()
        {
            if (await _packageRepository.GetCountAsync() <= 0)
            {


                await _packageRepository.InsertManyAsync(new List<Package>() 
                {
                                      new Package (Guid.NewGuid(),"Excel",true)
                                      ,
                                      new Package (Guid.NewGuid(),"Microsoft Word",true)
                                      ,
                                      new Package (Guid.NewGuid(),"Windows",true)
                                   {
                                       Versions=new List<PackageVersion>()
                                       {
                                           new PackageVersion (Guid.NewGuid())
                                           {
                                               Name="1.0.0",
                                               VersionNumber=1,
                                               IsActive=true,
                                               Type= PackageVersionType.Url,
                                               UrlPath="https://www.microsoft.com/en-us/software-download/windows10",
                                               AvailableDate=System.DateTime.Now.Date,
                                               Countries=( await _countryRepository.GetQueryableAsync().ConfigureAwait(false)).Take(10).Select(x=> new PackageVersionCountry(){  CountryId=x.Id}).ToList(),
                                               Channels=( await _channelRepository.GetQueryableAsync().ConfigureAwait(false)).Select(x=> new PackageVersionChannel(){  ChannelId=x.Id}).ToList()
                                               

                                           }
                                       }
                                       
                                   },
                                       new Package (Guid.NewGuid(),"Visual Studio",true)
                                   {
                                       Versions=new List<PackageVersion>()
                                       {
                                           new PackageVersion(Guid.NewGuid())
                                           {
                                               Name="1.0.0",
                                               VersionNumber=1,
                                               IsActive=true,
                                               Type= PackageVersionType.Url,
                                               UrlPath="https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&channel=Release&version=VS2022&source=VSLandingPage&cid=2030&passive=false",
                                               AvailableDate=System.DateTime.Now.Date,
                                               Countries=( await _countryRepository.GetQueryableAsync().ConfigureAwait(false)).Take(10).Select(x=> new PackageVersionCountry(){  CountryId=x.Id}).ToList(),
                                               Channels=( await _channelRepository.GetQueryableAsync().ConfigureAwait(false)).Select(x=> new PackageVersionChannel(){  ChannelId=x.Id}).ToList()

                                           }
                                       }
                                   },

                }, autoSave: true);

            }
        }


        private async Task ChannelSeedDataAsync()
        {
            if (await _channelRepository.GetCountAsync() <= 0)
            {
                await _channelRepository.InsertManyAsync(new List<Channel>()
                {
                                      new Channel
                                   {
                                       Name = "Internal beta",
                                   },
                                      new Channel
                                   {
                                       Name = "Insiders",
                                   },
                                      new Channel
                                   {
                                       Name = "Public",
                                   },

                }, autoSave: true);
            }

        }
        private async Task CountrySeedDataAsync()
        {
            if (await _countryRepository.GetCountAsync().ConfigureAwait(false) > 0) return;
            var currentDirectory = GetDomainProjectFolderPath();
            var path = System.IO.Path.Combine(currentDirectory, "Data/countries.json") ;
            string countryJson = await System.IO.File.ReadAllTextAsync(path).ConfigureAwait(false);
            List<Country> countriesList = JsonConvert.DeserializeObject<List<Country>>(countryJson);
            await _countryRepository.InsertManyAsync(countriesList, true).ConfigureAwait(false);
        }

        private async Task ClientSeedDataAsync()
        {
            using (_currentTenant.Change(default))
            {
                await IdentityOptions.SetAsync();

                var result = new IdentityDataSeedResult();
                
                const string clientUserName = "client";
                const string clientEmail = "client@fds.com";
                const string clientPassword = "Pop@12345";
                var clientUser = await _userRepository.FindByNormalizedUserNameAsync(
                    _lookupNormalizer.NormalizeName(clientUserName)
                );

                if (clientUser != null)
                {
                    return ;
                }

                clientUser = new IdentityUser(
                    _guidGenerator.Create(),
                    clientUserName,
                    clientEmail,
                    default
                )
                {
                    Name = clientUserName
                };
                var country= await _countryRepository.FirstOrDefaultAsync().ConfigureAwait(false);
                var channel= await _channelRepository.FirstOrDefaultAsync().ConfigureAwait(false);
                clientUser.ExtraProperties.Add("CountryId_Text", country.Name);
                clientUser.ExtraProperties.Add("ChannelId_Text", channel.Name);
                clientUser.ExtraProperties.Add("CountryId", country.Id);
                clientUser.ExtraProperties.Add("ChannelId", channel.Id);
                (await _userManager.CreateAsync(clientUser, clientPassword, validatePassword: false)).CheckErrors();

                //"client" role
                const string clientRoleName = "client";
                var clientRole =
                    await _roleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName(clientRoleName));
                if (clientRole == null)
                {
                    clientRole = new IdentityRole(
                        _guidGenerator.Create(),
                        clientRoleName,
                        default
                    )
                    {
                        IsDefault = true,
                        IsPublic = true
                    };

                    (await _roleManager.CreateAsync(clientRole)).CheckErrors();
                    result.CreatedAdminRole = true;
                }

                (await _userManager.AddToRoleAsync(clientUser, clientRoleName)).CheckErrors();

                // add package to client
                var packages = (await _packageVersionRepository.GetQueryableAsync()).Select(v => new ClientPackage(clientUser.Id, v.PackageId, v.Id)).ToList();
                await _clientPackageRepository.InsertManyAsync(packages,true);
            }
        }

        private string GetDomainProjectFolderPath()
        {
            var slnDirectoryPath = GetSolutionDirectoryPath();

            if (slnDirectoryPath == null)
            {
                throw new Exception("Solution folder not found!");
            }

            var srcDirectoryPath = Path.Combine(slnDirectoryPath, "src");

            return Directory.GetDirectories(srcDirectoryPath)
                .FirstOrDefault(d => d.EndsWith(".Domain"));
        }
        private string GetSolutionDirectoryPath()
        {
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            while (Directory.GetParent(currentDirectory.FullName) != null)
            {
                currentDirectory = Directory.GetParent(currentDirectory.FullName);

                if (Directory.GetFiles(currentDirectory.FullName).FirstOrDefault(f => f.EndsWith(".sln")) != null)
                {
                    return currentDirectory.FullName;
                }
            }

            return null;
        }
    }
}
