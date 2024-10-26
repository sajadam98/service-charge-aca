namespace ServiceCharge.Service.Unit.Tests.Floors.FactoryBuilder;

public class GetAllFloorsDtoBuilder
{
    private readonly GetAllFloorsDto _floorDto;

    public GetAllFloorsDtoBuilder()
    {
        _floorDto = new GetAllFloorsDto
        {
            Name = "Floor1"
        };
    }

    public GetAllFloorsDtoBuilder WithName(string name)
    {
        _floorDto.Name = name;
        return this;
    }

    public GetAllFloorsDtoBuilder WithUnitCount(int unitCount)
    {
        _floorDto.UnitCount = unitCount;
        return this;
    }

    public GetAllFloorsDtoBuilder WithId(int id)
    {
        _floorDto.Id = id;
        return this;
    }

    public GetAllFloorsDtoBuilder WithBlockId(int blockId)
    {
        _floorDto.BlockId = blockId;
        return this;
    }

    public GetAllFloorsDto Build()
    {
        return _floorDto;
    }
}