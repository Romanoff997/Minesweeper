using Minesweeper.Server.Domain.Repositories.Abstract;

namespace Minesweeper.Server.Domain.Repositories
{
    public class DataManager
    {
        public IMinesweeperModelRepository MinesweeperRepository { get; set; }

        public DataManager(IMinesweeperModelRepository commitRepository)
        {
            MinesweeperRepository = commitRepository;
        }
    }

}
