namespace ServiceCharge.Services.Blocks.Contracts;

public class GetAllWithAddedFloorCountDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int FloorCount { get; set; }
    public int AddedFloorCount { get; set; }
}