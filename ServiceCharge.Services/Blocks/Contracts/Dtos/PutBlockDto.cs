using System.ComponentModel.DataAnnotations;

namespace ServiceCharge.Services.Blocks.Contracts;

public class PutBlockDto
{
    
    public required int Id { get; set; } 
    public string? Name { get; set; }
    public DateTime? CreationDate { get; set; }
    public int? FloorCount { get; set; }
}