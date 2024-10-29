using ServiceCharge.Entities;

namespace ServiceCharge.TestTools.Floors;

public static class FloorFactory
{
     public static Floor Create(int blockid,string floorName="test",int unitCount=2)
     {
          return new Floor()
          {
               BlockId = blockid,
               Name = floorName,
               UnitCount = unitCount
          };
          
     }

     public static GetAllFloorDto GetAll(int BlockId,string floorName="test",int unitCount=2)
     {
          return new GetAllFloorDto()
          {
               BlockId = BlockId,
               Name = floorName,
               UnitCout = unitCount
          };

     }
}

public class GetAllFloorDto
{
     public int UnitCout { get; set; }
     public string Name { get; set; }
     public int BlockId { get; set; }
}