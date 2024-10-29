using System.Net.NetworkInformation;

namespace ServiceCharge.Services.Units.Contracts
{
    public class UpdateUnitDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}