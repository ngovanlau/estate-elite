using Microsoft.EntityFrameworkCore;

namespace PropertyService.Infrastructure.Data;

using Domain.Entities;
using Common.Infrastructure.Extensions;

public static class DbInitializer
{
    public static async Task InitializeAsync(PropertyContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Available<PropertyType>(false).AnyAsync())
        {
            return;
        }
        var propertyTypes = new PropertyType[]
        {
            new() {Id = Guid.NewGuid(), Name = "Căn hộ", CreatedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, IsDelete = false },
            new() {Id = Guid.NewGuid(), Name = "Nhà phố", CreatedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, IsDelete = false },
            new() {Id = Guid.NewGuid(), Name = "Biệt thự", CreatedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, IsDelete = false },
            new() {Id = Guid.NewGuid(), Name = "Văn phòng", CreatedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, IsDelete = false },
            new() {Id = Guid.NewGuid(), Name = "Đất nền", CreatedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, IsDelete = false },
            new() {Id = Guid.NewGuid(), Name = "Studio", CreatedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, IsDelete = false },
        };
        await context.PropertyTypes.AddRangeAsync(propertyTypes);

        if (await context.Available<Room>(false).AnyAsync())
        {
            return;
        }
        var rooms = new Room[]
        {
            new() {Id = Guid.NewGuid(), Name = "Phòng ngủ", CreatedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, IsDelete = false },
            new() {Id = Guid.NewGuid(), Name = "Phòng tắm", CreatedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, IsDelete = false },
            new() {Id = Guid.NewGuid(), Name = "Phòng bếp", CreatedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, IsDelete = false },
        };
        await context.Rooms.AddRangeAsync(rooms);

        if (await context.Available<Utility>(false).AnyAsync())
        {
            return;
        }
        var utilities = new Utility[]
        {
            new() { Id = Guid.NewGuid(), Name = "Điều hòa", CreatedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, IsDelete = false },
            new() { Id = Guid.NewGuid(), Name = "Bãi đỗ xe", CreatedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, IsDelete = false },
            new() { Id = Guid.NewGuid(), Name = "Hồ bơi", CreatedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, IsDelete = false },
            new() { Id = Guid.NewGuid(), Name = "Sân vườn", CreatedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, IsDelete = false },
            new() { Id = Guid.NewGuid(), Name = "An ninh", CreatedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, IsDelete = false },
            new() { Id = Guid.NewGuid(), Name = "Thang máy", CreatedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, IsDelete = false },
            new() { Id = Guid.NewGuid(), Name = "Phòng tập gym", CreatedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, IsDelete = false },
        };
        await context.Utilities.AddRangeAsync(utilities);

        await context.SaveChangesAsync();
    }
}
