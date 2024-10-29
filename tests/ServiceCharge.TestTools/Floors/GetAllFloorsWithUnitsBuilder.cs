namespace ServiceCharge.TestTools.Floors;

public class GetAllFloorsWithUnitsBuilder
{
    private readonly GetAllFloorsWithUnitsDto _floorDto;

    public GetAllFloorsWithUnitsBuilder()
    {
        _floorDto = new GetAllFloorsWithUnitsDto
        {
            Units = new HashSet<GetAllUnitsDto>(),
            Name = "Floor"
        };
    }

    public GetAllFloorsWithUnitsBuilder WithName(string name)
    {
        _floorDto.Name = name;
        return this;
    }

    public GetAllFloorsWithUnitsBuilder WithUnitCount(int unitCount)
    {
        _floorDto.UnitCount = unitCount;
        return this;
    }

    public GetAllFloorsWithUnitsBuilder WithBlockId(int blockId)
    {
        _floorDto.BlockId = blockId;
        return this;
    }

    public GetAllFloorsWithUnitsBuilder WithId(int id)
    {
        _floorDto.Id = id;
        return this;
    }

    public GetAllFloorsWithUnitsBuilder AddUnit(GetAllUnitsDto unit)
    {
        _floorDto.Units.Add(unit);
        return this;
    }

    public GetAllFloorsWithUnitsDto Build()
    {
        return _floorDto;
    }
}