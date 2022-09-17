using FDSService.Clients;
using FDSService.DataFilters;
using FDSService.DataLookups;
using FDSService.Packages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;
using System;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using FDSService.Attachments;

namespace FDSService.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class FDSServiceDbContext :
    AbpDbContext<FDSServiceDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    protected virtual bool IsActiveFilterEnabled => DataFilter?.IsEnabled<IIsActive>() ?? false;

    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }


    //Package
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<PackageVersion> PackageVersions => Set<PackageVersion>();

    public DbSet<PackageVersionChannel> PackageVersionChannels => Set<PackageVersionChannel>();

    public DbSet<PackageVersionCountry> PackageVersionCountries => Set<PackageVersionCountry>();


    //Data lookups
    public DbSet<Channel> Channels => Set<Channel>();
    public DbSet<Country> Countries => Set<Country>();

    //Client
    public DbSet<ClientPackage> ClientPackages => Set<ClientPackage>();

    //Others
    public DbSet<Attachment> Attachments => Set<Attachment>();
    #endregion

    public FDSServiceDbContext(DbContextOptions<FDSServiceDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureIdentityServer();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        // Get All entities flunt api settings at runtime
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    }

    protected override bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType)
    {
        if (typeof(IIsActive).IsAssignableFrom(typeof(TEntity)))
        {
            return true;
        }


        return base.ShouldFilterEntity<TEntity>(entityType);
    }

    protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
    {
        var expression = base.CreateFilterExpression<TEntity>();

        if (typeof(IIsActive).IsAssignableFrom(typeof(TEntity)))
        {
            Expression<Func<TEntity, bool>> isActiveFilter =
                e => !IsActiveFilterEnabled || EF.Property<bool>(e, nameof(IIsActive.IsActive));
            expression = expression == null
                ? isActiveFilter
                : CombineExpressions(expression, isActiveFilter);
        }


        return expression;
    }
}
