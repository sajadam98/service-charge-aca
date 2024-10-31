using ServiceCharge.Persistence.Ef;
using ServiceCharge.TestTools.Infrastructure.DataBaseConfig.Integration.Fixtures;

namespace ServiceCharge.TestTools.Infrastructure.DataBaseConfig.Integration;

public class BusinessIntegrationTest : EFDataContextDatabaseFixture
{
    protected EfDataContext Context { get; init; }
    protected EfDataContext SetupContext { get; init; }
    protected EfDataContext ReadContext { get; init; }
    
    protected BusinessIntegrationTest()
    {

        SetupContext = CreateDataContext();
        Context = CreateDataContext();
        ReadContext = CreateDataContext();
    }
    protected void Save<T>(T entity)
        where T : class
    {
        Context.Manipulate(_ => _.Add(entity));
    }

    protected void Save<T>(params T[] entities) 
        where T : class
    {
        foreach (var entity in entities)
        {
            Context.Save(entity);
        }
    }
}