using Identity.DAL;

namespace Identity.Api.Infrastuctures.Middlewares;

public class IdentityDbTransactionalMiddleware
{
    private readonly RequestDelegate _next;

    public IdentityDbTransactionalMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext context, IdentityDbContext dbContext)
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
