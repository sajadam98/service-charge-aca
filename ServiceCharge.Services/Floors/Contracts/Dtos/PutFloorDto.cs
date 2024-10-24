namespace ServiceCharge.Services.Floors.Contracts;

public class PutFloorDto
{
    public required int Id { get; set; }
    public string? Name { get; set; }
}