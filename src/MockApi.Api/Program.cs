using Microsoft.EntityFrameworkCore;
using MockApi.Api.Data;
using MockApi.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// --- Services ---

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=mockapi.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- Seed data on startup ---

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedData.EnsureSeeded(db);
}

// --- Middleware pipeline ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseHttpsRedirection();

// --- Endpoint mapping (one line per resource, added as we build each) ---

app.MapUserEndpoints();
app.MapPostEndpoints();
app.MapCommentEndpoints();

app.Run();