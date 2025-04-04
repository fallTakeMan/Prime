using Extensions.Logging.Prime;
using Microsoft.EntityFrameworkCore;
using Sample.PrimeApi;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Logging.AddPrime(cfg =>
{
    cfg.HttpLogIgnoreRouteMatch = "/swagger";

    //cfg.DbProvider = Extensions.Logging.Prime.Model.DatabaseType.MSSQL;
    //cfg.ConnectionString = builder.Configuration.GetConnectionString("MSSQL") ?? "";

    //cfg.DbProvider = Extensions.Logging.Prime.Model.DatabaseType.MySql;
    //cfg.ConnectionString = builder.Configuration.GetConnectionString("MySql") ?? "";

    cfg.DbProvider = Extensions.Logging.Prime.Model.DatabaseType.PostgreSQL;
    cfg.ConnectionString = builder.Configuration.GetConnectionString("Npgsql") ?? "";

    //cfg.PrimeUserName = "test";
    //cfg.PrimePassword = "12345";
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = ApiAuthenticationSchemeDefaults.Scheme;
    options.DefaultChallengeScheme = ApiAuthenticationSchemeDefaults.Scheme;
}).AddScheme<ApiAuthenticationSchemeOptions, ApiBasicAuthenticationHandler>(ApiAuthenticationSchemeDefaults.Scheme, null);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(ApiAuthorizationPolicyDefaults.Policy, policy =>
    {
        policy.AddAuthenticationSchemes(ApiAuthenticationSchemeDefaults.Scheme);
        policy.RequireAuthenticatedUser();
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("basic", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Description = "Basic {token}"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                { 
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "basic"
                }

            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<ApiDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PrimeApi"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
