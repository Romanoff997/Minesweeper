using Minesweeper.Server.Domain.Entities;
namespace Minesweeper.Server.Domain.Repositories.Abstract
{
    public interface IMinesweeperModelRepository
    {
        public Task<Guid> CreateGameAsync(GameEntity newGame);
        public Task<Guid> UpdateGame(GameEntity currGame);
        public Task<GameEntity?> GetGame(Guid id);
    }
}
