using ServiceCharge.Persistence.Ef;
using Xunit;

namespace ServiceCharge.TestTools.Infrastructure.DataBaseConfig.Integration.
    Fixtures;

public class EFDataContextDatabaseFixture : DatabaseFixture
{
    public static EfDataContext CreateDataContext()
    {
        return new EfDataContext(
            $"server=.;database=BlocksDb;Trusted_Connection=True;Encrypt=false;TrustServerCertificate=true;");
    }
}