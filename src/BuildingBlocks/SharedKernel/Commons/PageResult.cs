﻿namespace SharedKernel.Commons;

public sealed record class PageResult<T> where T : class
{
    public IEnumerable<T> Items { get; set; } = [];
    public int TotalRecords { get; set; }
}
