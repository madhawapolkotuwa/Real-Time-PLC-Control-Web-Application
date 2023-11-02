using PLC.WebApp.Hubs;
using PLC.WebApp.Models.Dtos;
using PLC.WebApp.Services;
using SLMP;
using SLMP.SlmpClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddAuthentication();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder => 
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()   
            .AllowAnyHeader()
            .AllowCredentials()
            .WithOrigins("http://localhost:4200");
    });
});

//builder.Services.AddSingleton<SlmpConfig>();
//builder.Services.AddSingleton<SlmpClient>();

builder.Services.AddSingleton<dDto>();
builder.Services.AddSingleton<xDto>();

builder.Services.AddSingleton<SLMPConnection>();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyPolicy");

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ConnectionHub>("/hubs/connection");

app.Run();
