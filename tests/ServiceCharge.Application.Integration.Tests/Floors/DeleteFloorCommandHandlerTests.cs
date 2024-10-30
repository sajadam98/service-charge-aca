using FluentAssertions;
using Moq;
using ServiceCharge.Application.Floors.Handlers.DeleteFloors;
using ServiceCharge.Application.Floors.Handlers.DeleteFloors.Contracts;
using ServiceCharge.Persistence.Ef.UnitOfWorks;
using ServiceCharge.Services.Floors.Contracts.Interfaces;
using ServiceCharge.Services.Units.Contracts.Interfaces;
using ServiceCharge.Services.Units.Exceptions;
using ServiceCharge.TestTools.Infrastructure.DataBaseConfig.Integration;

namespace ServiceCharge.Application.Integration.Tests.Floors;

public class DeleteFloorCommandHandlerTests : BusinessIntegrationTest
{
    private readonly DeleteFloorHandler _sut;
    private readonly Mock<FloorService> _floorService;
    private readonly Mock<UnitService> _unitService;

    public DeleteFloorCommandHandlerTests()
    {
        _floorService = new Mock<FloorService>();
        _unitService = new Mock<UnitService>();
        var unitOfWork = new EfUnitOfWork(SetupContext);
        _sut = new DeleteFloorCommandHandler(
            _floorService.Object,
            _unitService.Object,
            unitOfWork);
    }
    [Theory]
    [InlineData(1)]
    public void Delete_deletes_floor_and_units_properly(int floorId)
    {
        
        _unitService.Setup(s => s.DeleteByFloorId(floorId));
        _floorService.Setup(s => s.Delete(floorId));
        
        _sut.Handle(floorId);
        
        _unitService.Verify(s => s.DeleteByFloorId(floorId), Times.Once);
        _floorService.Verify(s => s.Delete(floorId), Times.Once);
    }
    
    //Google.com _unitService.Invocations
    [Theory]
    [InlineData(1)]
    public void Delete_delete_floor_and_units_properly(int floorId)
    {
        _unitService.Setup(s => s.DeleteByFloorId(floorId));
        _floorService.Setup(s => s.Delete(floorId));
        
        _sut.Handle(floorId);
        
        _unitService.Verify(s => s.DeleteByFloorId(floorId));
        _floorService.Verify(s => s.Delete(floorId));
        
        _unitService.Invocations.Should().HaveCount(1);
        _floorService.Invocations.Should().HaveCount(1);
    }
    //ChatGPT
    [Theory]
    [InlineData(1)]
    public void Delete_throws_exception_and_rollbacks_on_failure(int floorId)
    {
        _unitService.Setup(s => s.DeleteByFloorId(floorId))
            .Throws(new Exception("Unit deletion failed"));
        
        var actual = () => _sut.Handle(floorId);
        
        actual.Should().Throw<Exception>().WithMessage("Unit deletion failed");

        _unitService.Invocations.Should().NotBeEmpty();
        _floorService.Invocations.Should().BeEmpty();
    }
}