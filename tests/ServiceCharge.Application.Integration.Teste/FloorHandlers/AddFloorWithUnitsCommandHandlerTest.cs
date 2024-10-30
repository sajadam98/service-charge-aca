using Moq;
using ServiceCharge.Application.Floors.Handler.AddFloorWithUnits;
using ServiceCharge.Application.Floors.Handler.AddFloorWithUnits.Contracts;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using
    ServiceCharge.Service.Unit.Tests.Infrastructure.DataBaseConfig.Integration;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Floors.Contracts.Dto;
using ServiceCharge.Services.Unit;
using ServiceCharge.TestTools.Blocks;

namespace ServiceCharge.Application.Integration.Teste.FloorHandlers;

public class AddFloorWithUnitsCommandHandlerTest : BusinessIntegrationTest
{
    private readonly AddFloorWitUnitsHandler _sut;
    private readonly Mock<FloorService> _floorServiceMock;
    private readonly Mock<UnitService> _unitServiceMock;

    public AddFloorWithUnitsCommandHandlerTest()
    {
        var unitOfWork = new EfUnitOfWork(SetupContext);
        _floorServiceMock = new Mock<FloorService>();
        _unitServiceMock = new Mock<UnitService>();
        _sut = new AddFloorWithUnitsCommandHandler(
            unitOfWork,
            _floorServiceMock.Object,
            _unitServiceMock.Object);
    }

    [Theory]
    [InlineData(1)]
    public void Add_floor_with_units_properly(int floorId)
    {
        var block = BlockFactory.Create();
        Save(block);
        var command = new AddFloorWithUnitsCommand
        {
            Name = "dummy_floor-name",
            UnitCount = 2,
            Units =
            [
                new AddUnitCommand()
                {
                    Name = "dummy_unit_name",
                    IsActive = true
                },
                new AddUnitCommand()
                {
                    Name = "dummy_unit_name_2",
                    IsActive = true
                }
            ]
        };
        _floorServiceMock.Setup(s => s.Add(block.Id,
            It.Is<AddFloorDto>(dto =>
                dto.Name == command.Name &&
                dto.UnitCount == command.UnitCount))).Returns(floorId);

        _sut.Handle(block.Id, command);

        _floorServiceMock.Verify(s => s.Add(block.Id,
            It.Is<AddFloorDto>(dto =>
                dto.Name == command.Name &&
                dto.UnitCount == command.UnitCount)));
        _unitServiceMock.Verify(s => s.AddRange(floorId,
            It.Is<List<AddUnitDto>>(l =>
                l.Count(u => command.Units.Any(cu =>
                    u.Name == cu.Name && u.IsActive == cu.IsActive)) ==
                command.Units.Count && l.All(u =>
                    command.Units.Any(cu =>
                        u.Name == cu.Name && u.IsActive == cu.IsActive)))));
    }
}