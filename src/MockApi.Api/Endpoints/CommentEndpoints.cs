using Microsoft.EntityFrameworkCore;
using MockApi.Api.Data;
using MockApi.Api.Models;

namespace MockApi.Api.Endpoints;

public static class CommentEndpoints
{
    public static void MapCommentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/comments").WithTags("Comments");

        group.MapGet("/", async (AppDbContext db) =>
            await db.Comments.ToListAsync());

        group.MapGet("/{id:int}", async (int id, AppDbContext db) =>
            await db.Comments.FindAsync(id) is Comment comment
                ? Results.Ok(comment)
                : Results.NotFound());

        group.MapPost("/", (Comment comment) =>
        {
            comment.Id = Random.Shared.Next(1000, 9999);
            return Results.Created($"/comments/{comment.Id}", comment);
        });

        group.MapPut("/{id:int}", (int id, Comment updatedComment) =>
        {
            updatedComment.Id = id;
            return Results.Ok(updatedComment);
        });

        group.MapDelete("/{id:int}", (int id) =>
            Results.NoContent());
    }
}