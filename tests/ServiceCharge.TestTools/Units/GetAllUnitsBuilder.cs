namespace ServiceCharge.TestTools.Units;

public class GetAllUnitsBuilder
{
    private readonly GetAllUnitsDto _unitDto;

    public GetAllUnitsBuilder()
    {
        _unitDto = new GetAllUnitsDto
        {
            Name = "Unit"
        };
    }

    public GetAllUnitsBuilder WithName(string name)
    {
        _unitDto.Name = name;
        return this;
    }

    public GetAllUnitsBuilder WithFloorId(int floorId)
    {
        _unitDto.FloorId = floorId;
        return this;
    }

    public GetAllUnitsBuilder WithIsActive(bool isActive)
    {
        _unitDto.IsActive = isActive;
        return this;
    }

    public GetAllUnitsBuilder WithId(int id)
    {
        _unitDto.Id = id;
        return this;
    }

    public GetAllUnitsDto Build()
    {
        return _unitDto;
    }
}