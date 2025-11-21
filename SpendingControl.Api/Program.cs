using Microsoft.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SpendingControl.Api.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ModelValidationFilter>();
    options.Filters.Add<ValidationExceptionFilter>();
})
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var jwt = configuration.GetSection("Jwt");
var key = jwt["Key"] ?? string.Empty;
var issuer = jwt["Issuer"] ?? "spendingcontrol";
var audience = jwt["Audience"] ?? "spendingcontrol";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SpendingControl API",
        Version = "v1",
        Description = "API para gestionar gastos",
    });

});

var app = builder.Build();

var isLambda = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LAMBDA_FUNCTION_NAME"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS policy
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(e =>
{
    e.MapControllers();

    e.MapGet("/", c =>
    {
        c.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });
});

app.Run();