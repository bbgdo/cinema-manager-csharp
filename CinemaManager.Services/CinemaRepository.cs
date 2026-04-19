using CinemaManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManager.Services;

public class CinemaRepository(IDbContextFactory<CinemaDbContext> factory) : ICinemaRepository
{
    public async Task<IReadOnlyList<CinemaHall>> GetAllHallsAsync()
    {
        await using var ctx = await factory.CreateDbContextAsync();
        return await ctx.Halls.AsNoTracking().ToListAsync();
    }

    public async Task<CinemaHall?> GetHallByIdAsync(int id)
    {
        await using var ctx = await factory.CreateDbContextAsync();
        return await ctx.Halls.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<IReadOnlyList<Screening>> GetScreeningsByHallAsync(int hallId)
    {
        await using var ctx = await factory.CreateDbContextAsync();
        return await ctx.Screenings.AsNoTracking().Where(s => s.HallId == hallId).ToListAsync();
    }

    public async Task<Screening?> GetScreeningByIdAsync(int id)
    {
        await using var ctx = await factory.CreateDbContextAsync();
        return await ctx.Screenings.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<int> AddHallAsync(CinemaHall hall)
    {
        await using var ctx = await factory.CreateDbContextAsync();
        ctx.Halls.Add(hall);
        await ctx.SaveChangesAsync();
        return hall.Id;
    }

    public async Task UpdateHallAsync(CinemaHall hall)
    {
        await using var ctx = await factory.CreateDbContextAsync();
        ctx.Halls.Update(hall);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteHallAsync(int hallId)
    {
        await using var ctx = await factory.CreateDbContextAsync();
        var hall = await ctx.Halls.FindAsync(hallId);
        if (hall is null) return;
        ctx.Halls.Remove(hall);
        await ctx.SaveChangesAsync();
    }

    public async Task<int> AddScreeningAsync(Screening screening)
    {
        await using var ctx = await factory.CreateDbContextAsync();
        ctx.Screenings.Add(screening);
        await ctx.SaveChangesAsync();
        return screening.Id;
    }

    public async Task UpdateScreeningAsync(Screening screening)
    {
        await using var ctx = await factory.CreateDbContextAsync();
        ctx.Screenings.Update(screening);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteScreeningAsync(int screeningId)
    {
        await using var ctx = await factory.CreateDbContextAsync();
        var screening = await ctx.Screenings.FindAsync(screeningId);
        if (screening is null) return;
        ctx.Screenings.Remove(screening);
        await ctx.SaveChangesAsync();
    }
}
