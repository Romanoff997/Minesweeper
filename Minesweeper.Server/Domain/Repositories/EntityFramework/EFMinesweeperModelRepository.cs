using Microsoft.EntityFrameworkCore;
using Minesweeper.Server.Domain.Entities;
using Minesweeper.Server.Domain.Repositories.Abstract;

namespace Minesweeper.Server.Domain.Repositories.EntityFramework
{
    public class EFMinesweeperModelRepository : IMinesweeperModelRepository
    {
        private readonly MyDbContext _context;
        public EFMinesweeperModelRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> UpdateGame(GameEntity currGame)
        {
            _context.Games.Update(currGame);
            await _context.SaveChangesAsync();
            return currGame.Id;
        }
        public async Task<GameEntity?> GetGame(Guid id)
        {
            return await _context.Games.Include(x=>x.FieldEntity).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Guid> CreateGameAsync(GameEntity newGame)
        {
            await _context.Games.AddAsync(newGame);
            await _context.SaveChangesAsync();
            return newGame.Id;
        }

    }
}
