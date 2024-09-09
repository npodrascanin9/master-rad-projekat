using BackgroundRealTimeSignalrProject.Hubs;
using BackgroundRealTimeSignalrProject.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Specify the Angular app's origin
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials(); // This is important for credentials
        });
});
/*
 The CORS protocol does not allow specifying a wildcard (any) origin and credentials at the same time. Configure the CORS policy by listing individual origins if credentials needs to be supported.'
 */

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddHostedService<VehiclePositionBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting(); // Dodajte ovu liniju

app.UseAuthorization();

app.UseCors("AllowSpecificOrigin"); // Use the new policy name

app.MapControllers();

app.UseEndpoints(e =>
{
    e.MapHub<PozicijeVozilaHab>("/pozicijeVozilaHab");
});

app.Run();
