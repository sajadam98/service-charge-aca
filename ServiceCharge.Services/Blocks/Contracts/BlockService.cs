using System.ComponentModel.DataAnnotations;
using ServiceCharge.Services.Blocks.Contracts.DTOs;

namespace ServiceCharge.Services.Blocks.Contracts;

public interface BlockService
{
    void Add(AddBlockDto dto);
    void AddWithFloor(AddBlockWithFloorDto dto);
}