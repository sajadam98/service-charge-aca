using Moq;
using ServiceCharge.Application.Floors.Handlers.UpdateFloors;
using ServiceCharge.Application.Floors.Handlers.UpdateFloors.DTOs;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using ServiceCharge.Services.Floors.Contracts.Dtos;
using ServiceCharge.Services.Floors.Contracts.Interfaces;
using ServiceCharge.Services.Units.Contracts.Dtos;
using ServiceCharge.Services.Units.Contracts.Interfaces;
using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Infrastructure.DataBaseConfig.Integration;
using ServiceCharge.TestTools.Units;

namespace ServiceCharge.Application.Integration.Tests.Floors;

public class UpdateFloorWithUnitCommandHandlerTests:BusinessIntegrationTest
{
 private readonly UpdateFloorWithUnitsHandler _sut;
 private readonly Mock<FloorService> _floorService;
 private readonly Mock<UnitService> _unitService;

 public UpdateFloorWithUnitCommandHandlerTests()
 {
  _floorService = new Mock<FloorService>();
  _unitService = new Mock<UnitService>();
  var unitOfWork = new EfUnitOfWork(SetupContext);
  _sut = new UpdateFloorWithUnitsCommandHandler(
   _floorService.Object,
   _unitService.Object, 
   unitOfWork); 
 }

 [Fact]
  public void Update_update_floor_with_units_properly()
  {
      var blockBuilder = new BlockBuilder();
      var block = blockBuilder.WithFloorCount(2).Build();
      Save(block);
      var floor = FloorFactory.Generate(block.Id, unitCount:3); 
      Save(floor);
      var unit = UnitFactory.Create(floorId:floor.Id, name: "unit");
      Save(unit);
      var unit2 = UnitFactory.Create(floorId:floor.Id, name: "unit2");
      Save(unit2);
      var command = new UpdateFloorWithUnitsCommand
      {
       Name = "dummy-floor-name",
       UnitCount = 2,
       UnitsId = [unit.Id, unit2.Id],
       Units = [
        new UpdateUnitOfFloorCommand()
        {
         Name = "dummy-unit-name2",
         IsActive = true,
        },
        new UpdateUnitOfFloorCommand()
        {
         Name = "dummy-unit-name3",
         IsActive = false
        }
       ]
      };
      
      _sut.Handle(command, floor.Id );
      
      _floorService.Verify(s => s.Update(floor.Id,
                                     It.Is<UpdateFloorDto>(dto => 
                                      dto.Name == command.Name &&
                                      dto.UnitCount == command.UnitCount)));
   
      _unitService.Verify(s => s.Update(
       It.IsAny<int>(), 
       It.Is<UpdateUnitDto>(u => u.Name == command.Units.First().Name
                                 && u.IsActive == command.Units.First().IsActive)));

     
      _unitService.Verify(s => s.Update(
       It.IsAny<int>(), 
       It.Is<UpdateUnitDto>(u => u.Name == command.Units.Last().Name
                                 && u.IsActive == command.Units.Last().IsActive)));
  }

}