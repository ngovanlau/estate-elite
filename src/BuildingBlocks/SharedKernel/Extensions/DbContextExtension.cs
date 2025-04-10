using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;

namespace SharedKernel.Extensions;

public static class DbContextExtension
{
    public static IQueryable<T> Available<T>(this DbContext context, bool isTracking = true)
        where T : AuditableEntity
    {
        var result = context.Set<T>().Where(x => !x.IsDelete);
        return isTracking ? result : result.AsNoTracking();
    }
}
