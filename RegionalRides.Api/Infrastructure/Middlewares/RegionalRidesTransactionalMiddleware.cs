using RegionalRides.DAL;

namespace RegionalRides.Api.Infrastructure.Middlewares;

public class RegionalRidesTransactionalMiddleware
{
    private readonly RequestDelegate _next;

    public RegionalRidesTransactionalMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext context, RegionalRidesContext dbContext)
    {
        if (context.Request.Method == HttpMethod.Get.Method)
        {
            await _next.Invoke(context);
            return;
        }

        await _next.Invoke(context);
        try
        {
            var entityCount = await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            if (dbContext.Database.CurrentTransaction != null)
                dbContext.RollBack();
        }
    }
}