using Microsoft.EntityFrameworkCore;
using Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MasterContext>(option =>
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                              throw new InvalidOperationException("There is no default connection string");
    option.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/devices", async (MasterContext context) =>
{
    try
    {
        
    }
    catch
    {
        
    }
});

app.MapGet("/api/devices/{id}", async (MasterContext context, int id, CancellationToken token) =>
{

});

app.MapPost("/api/devices", async (MasterContext context, CancellationToken token) =>
{

});

app.MapPut("/api/devices/{id}", async (MasterContext context, int id, CancellationToken token) =>
{

});

app.MapDelete("/api/devices/{id}", async (MasterContext context, int id, CancellationToken token) =>
{

});

app.MapGet("/api/emplyees", async (MasterContext context, CancellationToken cancellationToken) =>
{

});

app.MapGet("/api/employees/{id}", async (MasterContext context, CancellationToken token) =>
{

});


app.Run();
