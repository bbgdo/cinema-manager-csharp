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
}
