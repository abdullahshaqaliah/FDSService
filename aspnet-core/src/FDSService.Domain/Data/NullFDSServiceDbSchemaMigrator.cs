using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace FDSService.Data;

/* This is used if database provider does't define
 * IFDSServiceDbSchemaMigrator implementation.
 */
public class NullFDSServiceDbSchemaMigrator : IFDSServiceDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
