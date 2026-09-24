using Microsoft.EntityFrameworkCore;
using MockApi.Api.Data;
using MockApi.Api.Models;

namespace MockApi.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/users").WithTags("Users");

        group.MapGet("/", async (AppDbContext db) =>
            await db.Users.ToListAsync());

        group.MapGet("/{id:int}", async (int id, AppDbContext db) =>
            await db.Users.FindAsync(id) is User user
                ? Results.Ok(user)
                : Results.NotFound());

        group.MapPost("/", (User user) =>
        {
            // fake-response pattern: assign a fake id, return it, don't persist
            user.Id = Random.Shared.Next(1000, 9999);
            return Results.Created($"/users/{user.Id}", user);
        });

        group.MapPut("/{id:int}", (int id, User updatedUser) =>
        {
            // fake-response pattern: echo back what was sent with the id from the route
            updatedUser.Id = id;
            return Results.Ok(updatedUser);
        });

        group.MapDelete("/{id:int}", (int id) =>
            Results.NoContent());
    }
}