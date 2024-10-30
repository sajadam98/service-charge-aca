namespace ServiceCharge.Application.Integration.Teste.FloorHandlers;

public class UpdateFloorWithUnitsCommandHandlerTest : BusinessIntegrationTest
{
    private readonly UpdateFloorWithUnitsHandler _sut;
    private readonly Mock<FloorService> _floorServiceMock;
    private readonly Mock<UnitService> _unitServiceMock;

    public UpdateFloorWithUnitsCommandHandlerTest()
    {
        var unitOfWork = new EfUnitOfWork(SetupContext);
        _floorServiceMock = new Mock<FloorService>();
        _unitServiceMock = new Mock<UnitService>();
        _sut = new UpdateFloorWithUnitsCommandHandler(
            unitOfWork,
            _floorServiceMock.Object,
            _unitServiceMock.Object);
    }

    [Fact]
    public void Update_floor_with_units_properly()
    {
        var block = BlockFactory.Create();
        Save(block);
        var floor = FloorFactory.Create(blockId: block.Id, unitCount: 3);
        Save(floor);
        var unit = UnitFactory.Create(floorId: floor.Id);
        Save(unit);
        var unit2 =
            UnitFactory.Create(floorId: floor.Id, name: "dummy_unit_name_2");
        Save(unit2);
        var command = new UpdateFloorWithUnitsCommand
        {
            Name = "new_floor_name",
            UnitCount = 4,
            Units =
            [
                new UpdateUnitCommand()
                {
                    Id = unit.Id,
                    Name = "new_unit_name",
                    IsActive = false
                },
                new UpdateUnitCommand()
                {
                    Id = unit2.Id,
                    Name = "new_unit_name_2",
                    IsActive = false
                }
            ]
        };
        _floorServiceMock.Setup(s => s.Update(
            It.Is<UpdateFloorDto>(dto =>
                dto.Id == floor.Id && dto.Name == command.Name &&
                dto.UnitCount == command.UnitCount)));

        _sut.Handle(floor.Id, command);

        _floorServiceMock.Verify(s => s.Update(It.Is<UpdateFloorDto>(dto =>
            dto.Id == floor.Id && dto.Name == command.Name &&
            dto.UnitCount == command.UnitCount)));
        _unitServiceMock.Verify(s =>
            s.UpdateRange(floor.Id,
                It.Is<List<UpdateUnitDto>>(l =>
                    l.Count(u => command.Units.Any(cu =>
                        u.Id == cu.Id && u.Name == cu.Name &&
                        u.IsActive == cu.IsActive)) == command.Units.Count &&
                    l.All(u =>
                        command.Units.Any(cu =>
                            u.Id == cu.Id && u.Name == cu.Name &&
                            u.IsActive == cu.IsActive)))));
    }
}