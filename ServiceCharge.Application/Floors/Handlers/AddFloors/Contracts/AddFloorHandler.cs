﻿using ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts.DTOs;

namespace ServiceCharge.Application.Floors.Handlers.AddFloors.Contracts;

public interface AddFloorHandler
{
    int Handle(int blockId, AddFloorWithUnitsCommand command);
}