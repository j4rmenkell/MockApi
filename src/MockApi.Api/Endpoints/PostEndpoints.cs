using Microsoft.EntityFrameworkCore;
using MockApi.Api.Data;
using MockApi.Api.Models;

namespace MockApi.Api.Endpoints;

public static class PostEndpoints
{
    public static void MapPostEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/posts").WithTags("Posts");

        group.MapGet("/", async (AppDbContext db) =>
            await db.Posts.ToListAsync());

        group.MapGet("/{id:int}", async (int id, AppDbContext db) =>
            await db.Posts.FindAsync(id) is Post post
                ? Results.Ok(post)
                : Results.NotFound());

        group.MapPost("/", (Post post) =>
        {
            post.Id = Random.Shared.Next(1000, 9999);
            return Results.Created($"/posts/{post.Id}", post);
        });

        group.MapPut("/{id:int}", (int id, Post updatedPost) =>
        {
            updatedPost.Id = id;
            return Results.Ok(updatedPost);
        });

        group.MapDelete("/{id:int}", (int id) =>
            Results.NoContent());
    }
}