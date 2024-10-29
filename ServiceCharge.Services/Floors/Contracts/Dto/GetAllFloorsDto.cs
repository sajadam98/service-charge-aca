namespace ServiceCharge.Services.Floors.Contracts.Dto
{
    public class GetAllFloorsDto
    {
        public int BlockId { get; set; }
        public int UnitCount { get; set; }
        public string Name { get; set; }
    }
}