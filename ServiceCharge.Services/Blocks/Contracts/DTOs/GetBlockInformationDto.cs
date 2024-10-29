namespace ServiceCharge.Services.Blocks.Contracts;

public class GetBlockInformationDto
{
    public required string Name { get; set; }
    public int FloorCapacity { get; set; }
    public int FloorCount { get; set; }
    
}