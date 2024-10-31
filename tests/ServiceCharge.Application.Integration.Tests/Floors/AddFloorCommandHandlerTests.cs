using FluentAssertions;
using Moq;
using ServiceCharge.Application.Floors.Handlers.AddFloors;
using ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts;
using ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts.DTOs;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using ServiceCharge.Services.Floors.Contracts.Dtos;
using ServiceCharge.Services.Floors.Contracts.Interfaces;
using ServiceCharge.Services.Floors.Exceptions;
using ServiceCharge.Services.UnitOfWorks;
using ServiceCharge.Services.Units;
using ServiceCharge.Services.Units.Contracts.Dtos;
using ServiceCharge.Services.Units.Contracts.Interfaces;
using ServiceCharge.TestTools.Application.Floors;
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
    private readonly Mock<UnitOfWork> _unitOfWork;

    public AddFloorCommandHandlerTests()
    {
        _floorService = new Mock<FloorService>();
        _unitService = new Mock<UnitService>();
        _unitOfWork = new Mock<UnitOfWork>();
        _sut = AddFloorHandlerFactory.CreateHandler(SetupContext,
            _floorService.Object, _unitService.Object, _unitOfWork.Object);
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

        var actual = _sut.Handle(block.Id, command);

        actual.Should().Be(floorId);
        _floorService.Verify(s => s.Add(block.Id,
            It.Is<AddFloorDto>(dto =>
                dto.Name == command.Name &&
                dto.UnitCount == command.UnitCount)));
        _unitService.Verify(s => s.AddRange(floorId,
            It.Is<List<AddUnitDto>>(l =>
                l.Count(u => command.Units.Any(uu =>
                    u.Name == uu.Name && u.IsActive == uu.IsActive)) ==
                command.Units.Count &&
                l.All(u => command.Units.Any(uu =>
                    u.Name == uu.Name && u.IsActive == uu.IsActive)))));
    }

    [Theory]
    [InlineData(1)]
    public void Update_floor_with_units_properly(int floorId)
    {
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
        _floorService.Setup(s => s.Add(floorId,
            It.Is<AddFloorDto>(dto =>
                dto.Name == command.Name &&
                dto.UnitCount == command.UnitCount))).Returns(floorId);

        var actual = _sut.Handle(floorId, command);

        _floorService.Verify(s => s.Update(floorId,
            It.Is<UpdateFloorDto>(dto =>
                dto.Name == command.Name &&
                dto.UnitCount == command.UnitCount)));
        _unitService.Verify(s => s.AddRange(floorId,
            It.Is<List<AddUnitDto>>(l =>
                l.Count(u => command.Units.Any(uu =>
                    u.Name == uu.Name && u.IsActive == uu.IsActive)) ==
                command.Units.Count &&
                l.All(u => command.Units.Any(uu =>
                    u.Name == uu.Name && u.IsActive == uu.IsActive)))));

    }
}