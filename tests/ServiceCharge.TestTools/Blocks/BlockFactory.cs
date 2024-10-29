namespace ServiceCharge.TestTools.Blocks;

public static class BlockFactory
{
    public static Block Create(string name = "dummy", int floorCount = 1, DateTime? creationDate = null)
    {
        creationDate ??= new DateTime(2022,1,1);
        return new Block
        {
            Name = name,
            CreationDate = creationDate.Value,
            FloorCount = floorCount
        };
    }
    public static GetAllBlocksDto GetAll(int id,string name = "Block", int floorCount = 1, DateTime? creationDate = null)
    {
        creationDate ??= new DateTime(2021,1,1);
        return new GetAllBlocksDto()
        {
            Id = id,
            Name = name,
            CreationDate = creationDate.Value,
            FloorCount = floorCount
        };
    }
    public static AddBlockDto AddBlockDto(string name = "Block", int floorCount = 1)
    {
        return new AddBlockDto()
        {
            Name = name,
            FloorCount = floorCount
        };
    }

    public static UpdateBlockDto UpdateBlockDto(string name = "Block", int floorCount = 1)
    {
        return new UpdateBlockDto
        {
            Name = name,
            FloorCount = floorCount
        };
    }

    public static AddBlockWithFloorDto AddBlockWithFloorDto(string name = "Block" , string floorname = "Floor")
    {
        return new AddBlockWithFloorDto()
        {
            Name = "block1",
            Floors =
            [
                new AddFloorDto
                {
                    Name = floorname,
                    UnitCount = 2
                }
            ]
        };
    }
}