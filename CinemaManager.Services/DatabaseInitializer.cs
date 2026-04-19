using Microsoft.EntityFrameworkCore;

namespace CinemaManager.Services;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IDbContextFactory<CinemaDbContext> factory)
    {
        await using var ctx = await factory.CreateDbContextAsync();
        await ctx.Database.EnsureCreatedAsync();

        if (await ctx.Halls.AnyAsync()) return;

        ctx.Halls.AddRange(SeedData.GetHalls());
        ctx.Screenings.AddRange(SeedData.GetScreenings());
        await ctx.SaveChangesAsync();
    }
}
