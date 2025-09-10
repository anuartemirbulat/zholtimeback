using Core;
using Identity.Api.Infrastuctures.Configurations;
using Identity.Api.Infrastuctures.Middlewares;
using Identity.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, services, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration)
    .Enrich.FromLogContext());

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "OpenCORSPolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers(x => { x.Filters.Add(typeof(IdentityExceptionFilterAttribute)); });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            LifetimeValidator = AuthOptions.CustomLifetimeValidator,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddDbContext<IdentityDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("IdentityDbConnection")));
builder.Services.AddOptions(builder.Configuration, builder.Environment);
builder.Services.AddImplServices(builder.Configuration, builder.Environment);
builder.Services.AddMobizonHttpClient(builder.Configuration);
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("OpenCORSPolicy");
app.UseAuthorization();
app.UseMiddleware<IdentityDbTransactionalMiddleware>();
app.MapControllers();

app.Run();
