using Minesweeper.Server.Domain.Entities;
using Minesweeper.Server.Models;

namespace Minesweeper.Server.Domain.Repositories.Abstract
{
    public interface IMinesweeperModelRepository
    {
        public Task<Guid> CreateGameAsync(GameEntity newGame);
        public Task<Guid> UpdateGame(GameEntity currGame);
        public Task<GameEntity?> GetGame(Guid id);
    }
}
