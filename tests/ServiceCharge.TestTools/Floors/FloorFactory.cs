namespace ServiceCharge.TestTools.Floors;

public static class FloorFactory
{
    public static Floor Generate(int blockId, string name = "floor",
        int unitCount = 1)
    {
        return new Floor
        {
            Name = name,
            UnitCount = unitCount,
            BlockId = blockId
        };
    }

    public static GetAllFloorsDto GetAll(int id, int blockid,
        string name = "Floor", int unitCount = 1)
    {
        return new GetAllFloorsDto()
        {
            Id = id,
            Name = name,
            UnitCount = unitCount,
            BlockId = blockid
        };
    }

    public static AddFloorDto AddFloorDto(string name = "Floor",
        int unitCount = 1)
    {
        return new AddFloorDto()
        {
            Name = name,
            UnitCount = unitCount
        };
    }


    public static UpdateFloorDto UpdateFloorDto(string name = "Floor",
        int unitCount = 1)
    {
        return new UpdateFloorDto
        {
            Name = name,
            UnitCount = unitCount
        };
    }
    // public static GetAllFloorsWithUnitsDto GetAllWithUnits(int blockId, 
    //         int unitId, int floorId, bool isActive = true,
    //         string unitName = "Unit",
    //         string floorname = "Floor", int unitCount = 1)
    // {
    //     return new GetAllFloorsWithUnitsDto
    //     {
    //         Name = floorname,
    //         UnitCount = unitCount,
    //         BlockId = blockId,
    //         Id = floorId,
    //         Units = 
    //         [
    //             new GetAllUnitsDto()
    //             {
    //                 Name = unitName,
    //                 FloorId = floorId,
    //                 IsActive = isActive,
    //                 Id = unitId
    //             }
    //         ]
    //     };
    // }
}