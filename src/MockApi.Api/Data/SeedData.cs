using Bogus;
using MockApi.Api.Models;

namespace MockApi.Api.Data;

public static class SeedData
{
    public static void EnsureSeeded(AppDbContext db)
    {
        db.Database.EnsureCreated();

        if (db.Users.Any()) return; // already seeded this run

        var userFaker = new Faker<User>()
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.City, f => f.Address.City());

        var users = userFaker.Generate(10);
        db.Users.AddRange(users);
        db.SaveChanges(); // save first so users have real Ids

        var postFaker = new Faker<Post>()
            .RuleFor(p => p.UserId, f => f.PickRandom(users).Id)
            .RuleFor(p => p.Title, f => f.Lorem.Sentence())
            .RuleFor(p => p.Body, f => f.Lorem.Paragraph());

        var posts = postFaker.Generate(50);
        db.Posts.AddRange(posts);
        db.SaveChanges();

        var commentFaker = new Faker<Comment>()
            .RuleFor(c => c.PostId, f => f.PickRandom(posts).Id)
            .RuleFor(c => c.Name, f => f.Name.FullName())
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Body, f => f.Lorem.Sentence());

        db.Comments.AddRange(commentFaker.Generate(100));
        db.SaveChanges();
    }
}