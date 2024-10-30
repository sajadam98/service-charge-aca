namespace ServiceCharge.Application.Integration.Tests.Floors;

public class UpdateFloorWithUnitsCommandHandlerTest : BusinessIntegrationTest
{
    private readonly UpdateFloorWithUnitsHandler _sut;
    private readonly Mock<FloorService> _floorService;
    private readonly Mock<UnitService> _unitService;

    public UpdateFloorWithUnitsCommandHandlerTest()
    {
        _floorService = new Mock<FloorService>();
        _unitService = new Mock<UnitService>();
        var unitOfWork = new EfUnitOfWork(SetupContext);
        _sut = new UpdateFloorWithUnitsCommandHandler(
            unitOfWork,
            _floorService.Object,
            _unitService.Object);
    }

    [Theory]
    [InlineData("newFloorName__2", 3)]
    public void Update_floor_with_units_properly(string newFloorName,
        int newUnitCount)
    {
        var block = new BlockBuilder().Build();
        Save(block);
        var floor = FloorFactory.Generate(blockId: block.Id, unitCount: 5);
        Save(floor);
        var unit = UnitFactory.Create(floorId: floor.Id);
        Save(unit);
        var unit2 = UnitFactory.Create(floorId: floor.Id, name: "unit_2");
        Save(unit2);

        var command = new UpdateFloorWithUnitsCommand()
        {
            Id = floor.Id,
            Name = newFloorName,
            UnitCount = newUnitCount,
            Units =
            [
                new UpdateUnitCommand()
                {
                    Id = unit.Id,
                    Name = "newUnitName",
                    IsActive = false
                },
                new UpdateUnitCommand()
                {
                    Id = unit2.Id,
                    Name = "newUnitName_2",
                    IsActive = false
                },
            ]
        };
        _floorService.Setup(s => s.Update(command.Id,
            It.Is<UpdateFloorDto>(dto =>
                dto.Name == command.Name &&
                dto.UnitCount == command.UnitCount)));

        _sut.Handle(command);

        _floorService.Verify(s => s.Update(command.Id,
            It.Is<UpdateFloorDto>(dto =>
                dto.Name == command.Name &&
                dto.UnitCount == command.UnitCount)));
        _unitService.Verify(s => s.UpdateRange(command.Id,
            It.Is<List<UpdateUnitDto>>(l =>
                l.Count == command.Units.Count && l.All(u =>
                    command.Units.Any(cu =>
                        cu.Name == u.Name && cu.Id == u.Id &&
                        cu.IsActive == u.IsActive)))));
    }
}