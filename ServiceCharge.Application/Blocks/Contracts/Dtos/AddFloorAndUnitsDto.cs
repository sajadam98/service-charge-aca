namespace ServiceCharge.Application.Floors.AddFloorAndUnits.Dtos;

public class AddFloorAndUnitsDto
{
    public string Name { get; set; }
    public List<AddUnitDto> Units { get; set; }
}