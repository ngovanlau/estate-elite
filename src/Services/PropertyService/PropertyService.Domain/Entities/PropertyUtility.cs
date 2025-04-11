﻿namespace PropertyService.Domain.Entities;

public class PropertyUtility
{
    public int Count { get; set; }

    public Guid PropertyId { get; set; }
    public required Property Property { get; set; }

    public Guid UtilityId { get; set; }
    public required Utility Utility { get; set; }
}
