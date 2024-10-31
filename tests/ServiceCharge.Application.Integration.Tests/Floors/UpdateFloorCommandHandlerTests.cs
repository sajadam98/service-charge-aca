using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Moq;
using ServiceCharge.Application.Floors.Handlers.UpdateFloors;
using ServiceCharge.Application.Floors.Handlers.UpdateFloors.Contracts;
using ServiceCharge.Application.Floors.Handlers.UpdateFloors.Contracts.DTOs;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using ServiceCharge.Services.Floors.Contracts.Dtos;
using ServiceCharge.Services.Floors.Contracts.Interfaces;
using ServiceCharge.Services.Units.Contracts.Interfaces;
using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Infrastructure.DataBaseConfig.Integration;
using ServiceCharge.TestTools.Units;

namespace ServiceCharge.Application.Integration.Tests.Floors;

public class UpdateFloorCommandHandlerTests : BusinessIntegrationTest
{
    private readonly UpdateFloorHandler _sut;
    private readonly Mock<FloorService> _floorService;
    private readonly Mock<UnitService> _unitService;

    public UpdateFloorCommandHandlerTests()
    {
        _floorService = new Mock<FloorService>();
        _unitService = new Mock<UnitService>();
        var unitOfWork = new EfUnitOfWork(SetupContext);
        _sut = new UpdateFloorWithUnitCommandHandler(_floorService.Object,
            _unitService.Object, unitOfWork);
    }

    [Fact]
    public void Update_update_floor_with_units_properly()
    {
        var block = new BlockBuilder().Build();
        Save(block);
        var floor = FloorFactory.Generate(block.Id);
        Save(floor);
        var unit1 = UnitFactory.Create(floor.Id);
        Save(unit1);
        var unit2 = UnitFactory.Create(floor.Id);
        Save(unit2);
        var command = new UpdateFloorWithUnitsCommand
        {
            Name = floor.Name,
            IsActive = true,
            UnitCount = 5,
            Units =
            [
                new UpdateUnitOfFloorCommand
                {
                    Name = "unit1 new Name",
                    IsActive = false,
                    Id = unit1.Id
                }
            ]
        };
        _sut.Handle(floor.Id, command);

        _floorService.Verify(s => s.Update
        (floor.Id, It.Is<UpdateFloorDto>(
            dto =>
                dto.Name == command.Name &&
                dto.UnitCount == command.UnitCount
        )));

        _unitService.Verify(s => s.UpdateRange(floor.Id,
            It.Is<HashSet<UpdateUnitsOfFloorDto>>(l =>
                l.Count(u => command.Units.Any(uu =>
                    uu.Name == u.Name &&
                    uu.IsActive == u.IsActive &&
                    uu.Id == u.Id
                )) == command.Units.Count
            )));
    }
}