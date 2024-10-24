using ServiceCharge.Entities;

namespace ServiceCharge.Services.Blocks.Contracts;

public class GetBlockDto
{
    public int Id { get; set; }
    public int FloorCount { get; set; }
    public required string Name { get; set; }
    public DateTime CreationDate { get; set; }
    
}