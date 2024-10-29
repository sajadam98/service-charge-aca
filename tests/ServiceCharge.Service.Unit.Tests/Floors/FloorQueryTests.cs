using FluentAssertions;
using ServiceCharge.Entities;
using ServiceCharge.Persistence.Ef.Floors;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.Services.Floors.Contracts.Dto;

namespace ServiceCharge.Service.Unit.Tests.Floors
{
    public class FloorQueryTests : BusinessIntegrationTest
    {
        private readonly FloorQuery _sut;

        public FloorQueryTests()
        {
            _sut = new EFFloorQuery(Context);
        }

        [Fact]
        public void GetAll()
        {
            var floors = new HashSet<Floor>()
        {
            new Floor()
            {
                Name = "1",
                UnitCount = 5,
            },
            new Floor()
            {
                Name = "2",
                UnitCount = 5
            }
        };

            var block = CreateBlockWithFloorsList("name", DateTime.UtcNow, 2, floors);

            Save(block);

            var result = _sut.GetAll(block.Id);

            result.Should().HaveCount(2);

            result[0].Should().BeEquivalentTo(new GetAllFloorsBlockDto()
            {
                Name = "1",
                UnitCount = 5

            });

            result[1].Should().BeEquivalentTo(new GetAllFloorsBlockDto()
            {
                Name = "2",
                UnitCount = 5

            });
        }

        private static Block CreateBlockWithFloorsList(string name, DateTime dateTime, int flourCount, HashSet<Floor> floors)
        {
            return new Block()
            {
                Name = name,
                CreationDate = dateTime,
                FloorCount = flourCount,
                Floors = floors
            };
        }
    }
}

