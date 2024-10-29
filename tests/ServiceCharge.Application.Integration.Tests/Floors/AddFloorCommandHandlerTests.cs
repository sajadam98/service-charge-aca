using Moq;
using ServiceCharge.Application.Floors.Handlers.AddFloors;
using ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts;
using ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts.DTOs;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using ServiceCharge.Services.Floors.Contracts.Dtos;
using ServiceCharge.Services.Floors.Contracts.Interfaces;
using ServiceCharge.Services.Units;
using ServiceCharge.Services.Units.Contracts.Dtos;
using ServiceCharge.Services.Units.Contracts.Interfaces;
using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Infrastructure.DataBaseConfig.Integration;
using ServiceCharge.TestTools.Units;

namespace ServiceCharge.Application.Integration.Tests.Floors;

public class AddFloorCommandHandlerTests : BusinessIntegrationTest
{
    private readonly AddFloorHandler _sut;
    private readonly Mock<FloorService> _floorService;
    private readonly Mock<UnitService> _unitService;

    public AddFloorCommandHandlerTests()
    {
        _floorService = new Mock<FloorService>();
        _unitService = new Mock<UnitService>();
        var unitOfWork = new EfUnitOfWork(SetupContext);
        _sut = new AddFloorCommandHandler(
            _floorService.Object,
            _unitService.Object,
            unitOfWork);
    }

    [Theory]
    [InlineData(1)]
    public void Add_adds_floor_with_units_properly(int floorId)
    {
        var block = new BlockBuilder()
            .Build();
        Save(block);
        var command = new AddFloorWithUnitsCommand
        {
            Name = "Dummy_Floor_Name",
            UnitCount = 3,
            Units =
            [
                new AddUnitOfFloorCommand
                {
                    Name = "1",
                    IsActive = true
                },
                new AddUnitOfFloorCommand
                {
                    Name = "2",
                    IsActive = true
                }
            ]
        };
        _floorService.Setup(s => s.Add(block.Id,
            It.Is<AddFloorDto>(dto =>
                dto.Name == command.Name &&
                dto.UnitCount == command.UnitCount))).Returns(floorId);

        _sut.Handle(block.Id, command);

        _floorService.Verify(s => s.Add(block.Id,
            It.Is<AddFloorDto>(dto =>
                dto.Name == command.Name &&
                dto.UnitCount == command.UnitCount)));
        // _unitService.Verify(s => s.AddRange(floorId,
        //     It.Is<List<AddUnitDto>>(l =>
        //         l.Count == command.Units.Count &&
        //         l.All(u => command.Units.Any(uu =>
        //             u.Name == uu.Name && u.IsActive == uu.IsActive)))));
        _unitService.Verify(s => s.AddRange(floorId,
            It.Is<List<AddUnitDto>>(l =>
                l.Count(u => command.Units.Any(uu =>
                    u.Name == uu.Name && u.IsActive == uu.IsActive)) ==
                command.Units.Count &&
                l.All(u => command.Units.Any(uu =>
                    u.Name == uu.Name && u.IsActive == uu.IsActive)))));
    }
}