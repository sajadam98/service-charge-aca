﻿using ServiceCharge.Entities;

namespace ServiceCharge.Services.Blocks.Contracts;

public interface BlockRepository
{
    void Add(Block block);
    bool IsDuplicate(string name);
    Block? FindById(int id);
    bool IsNameDuplicate(string name);
    bool IsExistById(int id);
}