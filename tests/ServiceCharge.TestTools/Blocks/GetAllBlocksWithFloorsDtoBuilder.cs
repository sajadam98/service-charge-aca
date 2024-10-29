namespace ServiceCharge.TestTools.Blocks;

public class GetAllBlocksWithFloorsDtoBuilder
{
    private readonly GetAllBlocksWithFloorsDto _blockDto;

    public GetAllBlocksWithFloorsDtoBuilder()
    {
        _blockDto = new GetAllBlocksWithFloorsDto
        {
            Floors = new List<GetAllFloorsDto>(),
            Name = "Block1"
        };
    }

    public GetAllBlocksWithFloorsDtoBuilder WithName(string name)
    {
        _blockDto.Name = name;
        return this;
    }

    public GetAllBlocksWithFloorsDtoBuilder WithFloorCount(int floorCount)
    {
        _blockDto.FloorCount = floorCount;
        return this;
    }

    public GetAllBlocksWithFloorsDtoBuilder WithCreationDate(DateTime creationDate)
    {
        _blockDto.CreationDate = creationDate;
        return this;
    }

    public GetAllBlocksWithFloorsDtoBuilder WithId(int id)
    {
        _blockDto.Id = id;
        return this;
    }

    public GetAllBlocksWithFloorsDtoBuilder AddFloor(GetAllFloorsDto floor)
    {
        _blockDto.Floors.Add(floor);
        return this;
    }

    public GetAllBlocksWithFloorsDto Build()
    {
        return _blockDto;
    }
}