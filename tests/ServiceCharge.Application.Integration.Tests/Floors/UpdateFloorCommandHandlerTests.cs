using Moq;
using ServiceCharge.Application.Floors.Handlers.AddFloors;
using ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts;
using ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts.DTOs;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using ServiceCharge.Services.Floors.Contracts.Dtos;
using ServiceCharge.Services.Floors.Contracts.Interfaces;
using ServiceCharge.Services.Units.Contracts.Dtos;
using ServiceCharge.Services.Units.Contracts.Interfaces;
using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Infrastructure.DataBaseConfig.Integration;

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
        _sut = new UpdateFloorCommandHandler(
            _floorService.Object,
            _unitService.Object,
            unitOfWork);
    }

    [Theory]
    [InlineData(1)]
    public void Update_updates_floor_with_units_properly(int floorId)
    {
        var block = new BlockBuilder().Build();
        Save(block);
        var command = new UpdateFloorWithUnitsCommand
        {
            Name = "Updated_Floor_Name",
            UnitCount = 2,
            Units = new HashSet<UpdateUnitOfFloorCommand>
            {
                new UpdateUnitOfFloorCommand
                {
                    Name = "Unit_1",
                    IsActive = true
                },
                new UpdateUnitOfFloorCommand
                {
                    Name = "Unit_2",
                    IsActive = false
                }
            }
        };
        
        _floorService.Setup(s => s.Update(floorId, block.Id,
            It.Is<UpdateFloorDto>(dto =>
                dto.Name == command.Name &&
                dto.UnitCount == command.UnitCount)));

        _sut.Handle(block.Id, floorId, command);
        
        _floorService.Verify(s => s.Update(floorId, block.Id,
            It.Is<UpdateFloorDto>(dto =>
                dto.Name == command.Name &&
                dto.UnitCount == command.UnitCount)));
        
        _unitService.Verify(s => s.AddRange(floorId,
            It.Is<List<AddUnitDto>>(l =>
                l.Count == command.Units.Count &&
                l.All(u => command.Units.Any(uu =>
                    u.Name == uu.Name && u.IsActive == uu.IsActive)))));
    }
}