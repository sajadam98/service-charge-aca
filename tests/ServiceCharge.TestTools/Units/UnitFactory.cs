namespace ServiceCharge.Service.Unit.Tests.Units.FactoryBuilder;

public static class UnitFactory
{
    public static Entities.Units.Unit Create(string name = "unit",int floorId = 1, bool isActive = true)
    {
        return new Entities.Units.Unit()
        {
            Name = name,
            IsActive = isActive,
            FloorId = floorId
        };
    }

    public static AddUnitDto AddUnitDto(string name = "unit", bool isActive = true,int floorId = 0)
    {
        return new AddUnitDto
        {
            Name = name,
            IsActive = isActive,
            FloorId = floorId
        };
    }

    
    public static GetAllUnitsDto GetAll(int id,string name = "Unit", int floorId = 0,bool isActive = true)
    {
        return new GetAllUnitsDto()
        {
            Id = id,
            Name = name,
            FloorId = floorId,
            IsActive = isActive,
        };
    }

    public static UpdateUnitDto UpdateUnitDto(string name = "UnitUpdated",int floorId = 1,bool isActive = true)
    {
        return new UpdateUnitDto
        {
            Name = name,
            IsActive = isActive,
            FloorId = floorId
        };
    }
    
    public static GetAllUnitsWithBlockNameAndFloorNameDto GetAllUnitsWithInfo(int id, string name = "Unit"
            , int floorId = 0,bool isActive = true
            ,string floorName = "",string blockName = "")
    {
        return new GetAllUnitsWithBlockNameAndFloorNameDto()
        {
            Id = id,
            Name = name,
            FloorId = floorId,
            IsActive = isActive,
            BlockName = blockName,
            FloorName = floorName
        };
    }
}