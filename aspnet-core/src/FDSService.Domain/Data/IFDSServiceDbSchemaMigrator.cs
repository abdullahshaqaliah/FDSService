using System.Threading.Tasks;

namespace FDSService.Data;

public interface IFDSServiceDbSchemaMigrator
{
    Task MigrateAsync();
}
