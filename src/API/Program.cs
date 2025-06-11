/*using DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Models;
using Others;
using System.Text.Json;
using Tokens;

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

var jwtConfigData = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtOptions>(jwtConfigData);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfigData["Issuer"],
        ValidAudience = jwtConfigData["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigData["Key"])),
        ClockSkew = TimeSpan.FromMinutes(10)
    };
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAuthentication();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<>();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/api/devices", async (MasterContext context, CancellationToken token) =>
{
    try
    {
        var devices = await context.Devices.Select(d => new AllDevicesResponseBody(d.Id, d.Name)).ToListAsync(token);
        return Results.Ok(devices);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/devices/{id}", async (int id, MasterContext context, CancellationToken token) =>
{
    try
    {
        var foundDevice = await context.Devices.Where(device => device.Id == id)
                                .Include(device => device.DeviceType)
                                .Include(device => device.DeviceEmployees)
                                .ThenInclude(deviceEmployee => deviceEmployee.Employee)
                                .ThenInclude(employee => employee.Person)
                                .FirstAsync(token);
        var additionalParam = JsonDocument.Parse(foundDevice.AdditionalProperties ?? "{}").RootElement;
        var currentlyUsing = foundDevice.DeviceEmployees.FirstOrDefault(de => de.ReturnDate == null);
        return Results.Ok(
            new DeviceByIdResponseBody(foundDevice.Name, foundDevice.DeviceType.Name, foundDevice.IsEnabled, additionalParam,
                new EmployeesShortDataResponseBody(currentlyUsing.EmployeeId, 
                    currentlyUsing.Employee.Person.MiddleName == null ? 
                    $"{currentlyUsing.Employee.Person.FirstName} {currentlyUsing.Employee.Person.LastName}" : 
                    $"{currentlyUsing.Employee.Person.FirstName} {currentlyUsing.Employee.Person.MiddleName} {currentlyUsing.Employee.Person.LastName}")
            )
        );
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
    
});

app.MapPost("/api/devices", async (DeviceRequestBody requestBody, MasterContext context, CancellationToken token) =>
{
    try
    {
        if (requestBody.AdditionalProperties.ValueKind == JsonValueKind.Null)
            return Results.BadRequest("AdditionalProperties cannot be null");

        int nextDeviceId = await context.Devices.Select(d => d.Id).Order().LastAsync(token);
        var deviceType = await context.DeviceTypes.Where(t => t.Name == requestBody.DeviceType).FirstAsync(token);

        Device createdDevice = new()
        {
            Id = nextDeviceId,
            Name = requestBody.Name,
            DeviceType = deviceType,
            IsEnabled = requestBody.IsEnabled,
            AdditionalProperties = requestBody.AdditionalProperties.GetRawText()
        };

        context.Devices.Add(createdDevice);
        await context.SaveChangesAsync(token);

        return Results.Created($"/api/devices/{createdDevice.Id}", createdDevice);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPut("/api/devices/{id}", async (int id, DeviceRequestBody requestBody, MasterContext context, CancellationToken token) =>
{
    try
    {
        var deviceToUpdate = await context.Devices.FirstAsync(d => d.Id == id, token);
        if (deviceToUpdate == null)
            return Results.NotFound($"Device {id} not found");

        var typeId = await context.DeviceTypes.FirstAsync(t => t.Name == requestBody.DeviceType);
        if (typeId == null)
            return Results.NotFound($"Didn't found device type: {requestBody.DeviceType}");

        deviceToUpdate.Name = requestBody.Name;
        deviceToUpdate.DeviceTypeId = typeId.Id;
        deviceToUpdate.IsEnabled = requestBody.IsEnabled;
        deviceToUpdate.AdditionalProperties = requestBody.AdditionalProperties.GetRawText();

        await context.SaveChangesAsync(token);

        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapDelete("/api/devices/{id}", async (int id, MasterContext context, CancellationToken token) =>
{
    try
    {
        var deviceToDelete = await context.Devices.FirstAsync(d => d.Id == id, token);
        if (deviceToDelete == null)
            return Results.NotFound($"Device to delete with this id: {id} was not found");

        var deviceConnectedToEmployees = await context.DeviceEmployees.Where(d => d.DeviceId == id).ToListAsync(token);

        if (!deviceConnectedToEmployees.IsNullOrEmpty())
            context.DeviceEmployees.RemoveRange(deviceConnectedToEmployees);
        context.Devices.Remove(deviceToDelete);

        await context.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/emplyees", async (MasterContext context, CancellationToken token) =>
{
    try
    {
        var employees = await context.People.Select(p => new EmployeesShortDataResponseBody(p.Id, 
            p.MiddleName == null ? $"{p.FirstName} {p.LastName}" : $"{p.FirstName} {p.MiddleName} {p.LastName}"
            )).ToListAsync(token);
        return Results.Ok(employees);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/employees/{id}", async (int id, MasterContext context, CancellationToken token) =>
{
    try
    {
        var employee = await context.Employees.Where(e => e.Id == id).Include(e => e.Person).Include(e => e.Position).FirstAsync(token);
        return Results.Ok(new EmployeeByIdResponseBody(
            new PersonResponseBody(employee.Person.Id, employee.Person.PassportNumber, employee.Person.FirstName, employee.Person.MiddleName, employee.Person.LastName, employee.Person.PhoneNumber, employee.Person.Email),
            employee.Salary,
            new PositionResponseBody(employee.Position.Id, employee.Position.Name),
            employee.HireDate
        ));
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});


app.Run();*/