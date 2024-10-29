using ServiceCharge.Services.Floors.Contracts;

namespace ServiceCharge.TestTools.Floors.DTOs;

public class GetAllFloorsDtoBuilder(int id, int blockId)
{
    private GetAllFloorsDto _getAllFloorsDto = new GetAllFloorsDto
    {
        Name = "Dummy_Floor_Name",
        UnitCount = 2,
        Id = id,
        BlockId = blockId,
    };

    public GetAllFloorsDtoBuilder WithName(string name)
    {
        _getAllFloorsDto.Name = name;
        return this;
    }

    public GetAllFloorsDtoBuilder WithUnitCount(int unitCount)
    {
        _getAllFloorsDto.UnitCount = unitCount;
        return this;
    }

    public GetAllFloorsDto Build()
    {
        return _getAllFloorsDto;
    }
}