using Chillgo.Api;
using Chillgo.Api.Middlewares;
using System.Text.Json.Serialization;
using AutoMapper;
using Chillgo.BusinessService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.DependencyInjectionServices(builder.Configuration, builder.Environment);

// Add AutoMapper service and register the MappingProfile
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts(); // Tun on HSTS in environment Production
}

app.UseResponseCaching();

// Allow all http
app.UseCors("AllowAll");

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

// Jwt Authentication Middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
