﻿using SharedKernel.Entities;

namespace PropertyService.Domain.Entities;

public class Image : AuditableEntity
{
    public required string HashId { get; set; }
    public string? Caption { get; set; }
    public required string OriginalFilename { get; set; }
    public required string BucketName { get; set; }
    public required string ObjectName { get; set; }
    public required string ContentType { get; set; }
    public required double FileSize { get; set; }
    public bool IsMain { get; set; }
    public Guid? PropertyId { get; set; }
    public Guid? ProjectId { get; set; }

}
