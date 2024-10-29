using ServiceCharge.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCharge.TestTools.Floors
{
    public class FloorBuilder
    {


        private readonly Floor _floor = new()
        {
            Name = "Dummy_Block_Name",
            UnitCount = 1,
            BlockId=1

        };
        public FloorBuilder WithBlockId(int blockId) 
        {
            _floor.BlockId = blockId;
            return this;
        }
        public FloorBuilder WithFloorCount(int unitCount)
        {
            _floor.UnitCount = unitCount;
            return this;
        }
        public FloorBuilder WithName(string name)
        {
            _floor.Name = name;
            return this;
        }
        public Floor Build()
        {
            return _floor;
        }



    }
}
