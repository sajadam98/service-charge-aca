using Microsoft.AspNetCore.Http.HttpResults;

namespace ServiceCharge.Service.Unit.Tests.Blocks;

public static class BlockFactory
{

    public static Block Create(
        string name="dummy",
        int floorCount=1,
        DateTime? creationDate=null)
    {
        creationDate ??= new DateTime(2024, 1, 1);
        return new Block()
        {
            Name = name,
            CreationDate = creationDate.Value,
            FloorCount = floorCount
        };
    }
}