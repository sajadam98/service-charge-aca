using FluentAssertions;
using ServiceCharge.Entities;
using ServiceCharge.Service.Unitt.Tests.Infrastructure.DataBaseConfig.Integration;

using ServiceCharge.Services.Units.Contracts;
using ServiceCharge.Services.Units.Contracts.Dtos;
using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using ServiceCharge.TestTools.Units;


namespace ServiceCharge.Service.Unitt.Tests.Units
{
    public class UnitServiceTests : BusinessIntegrationTest
    {
        private readonly UnitService _sut;
        public UnitServiceTests()
        {
            _sut = UnitServiceFactory.CreateService(SetupContext);
        }

        [Fact]
        public void Add_add_a_unit_properly()
        {
            var block = new BlockBuilder().Build();
            Save(block);
            var floor = new FloorBuilder().WithBlockId(block.Id).Build();
            Save(floor);
            var dto = new AddUnitDto()
            {
                Name = "u1",
                IsActive = true
            };

            var actual = _sut.Add(floor.Id, dto);


            var expected = ReadContext.Set<Unit>().FirstOrDefault();
            expected.Id.Should().Be(actual);
            expected.Name.Should().Be(dto.Name);
            expected.IsActive.Should().Be(dto.IsActive);
            expected.FloorId.Should().Be(floor.Id);

        }
        [Fact]
        public void Update_update_a_unit_properly()
        {

            var block = new BlockBuilder().Build();
            Save(block);
            var floor = new FloorBuilder().WithBlockId(block.Id).Build();
            Save(floor);
            var unit = new UnitBuilder().Build();
            Save(unit);
            var dto = new UpdateUnitDto()
            {
                Name = "u2",
                IsActive = false
            };

            _sut.Update(unit.Id, dto);

            var expected = ReadContext.Set<Unit>().Single();
            expected.Name.Should().Be(dto.Name);
            expected.IsActive.Should().Be(dto.IsActive);
        }
        [Fact]
        public void Delete_delete_a_unit_properly()
        {

            var block = new BlockBuilder().Build();
            Save(block);
            var floor = new FloorBuilder().WithBlockId(block.Id).Build();
            Save(floor);
            var unit = new UnitBuilder().Build();
            Save(unit);



            _sut.Delete(unit.Id);

            var expected = ReadContext.Set<Unit>().FirstOrDefault();
            expected.Should().BeNull();

        }
    }
}
