using FluentAssertions;
using ServiceCharge.Entities;
using ServiceCharge.Persistence.Ef.Floors;
using ServiceCharge.Service.Unitt.Tests.Infrastructure.DataBaseConfig.Integration;
using ServiceCharge.Services.Floors.Contracts;
using ServiceCharge.TestTools.Blocks;
using ServiceCharge.TestTools.Floors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCharge.Service.Unitt.Tests.Floors
{
    public class FloorQueryTests : BusinessIntegrationTest
    {
        private readonly FloorQuery _sut;
        public FloorQueryTests()
        {
            _sut = new EFFloorQuery(Context);
        }

        [Fact]
        public void Get_get_all_floors_properly()
        {

            var block = new BlockBuilder().Build();
            Save(block);
            var floor = new FloorBuilder().WithBlockId(block.Id).WithName("f1").Build();
            Save(floor);
            var floor2 = new FloorBuilder().WithBlockId(block.Id).WithName("f2").Build();
            Save(floor2);

            var actual = _sut.GetAll();



            actual.Should().Contain(_ => _.Name == floor.Name && _.UnitCount == floor.UnitCount && _.BlockId == block.Id);
            actual.Should().Contain(_ => _.Name == floor2.Name && _.UnitCount == floor2.UnitCount && _.BlockId == block.Id);

        }
    }
}
