
using Minesweeper.Server.Domain.Entities;
using Minesweeper.Server.Domain.Repositories;
using Minesweeper.Server.Models;

namespace Minesweeper.Server.Services
{
    public class MinesweeperService
    {
        private readonly DataManager _manager;
        private const string voidSymbol = " ";
        private const string boombSymbol = "X";
        private const string minesSymbol = "M";
        public MinesweeperService(DataManager manager)
        {
            _manager = manager;
        }

        public async Task<Guid> CreateGame(NewGameRequest game)
        {
            int fieldSize = game.width * game.height;
            List<FieldEntity> fieldList = new();
            for (int i = 0; i < fieldSize; i++)
            {
                fieldList.Add(new());
            }
            Random rand = new Random();
            for (int i = 0; i < game.mines_count; i++)
            {
                int index = rand.Next(0, fieldSize-1);
                FieldEntity curr = fieldList[index];
                if (curr.Boomb)
                {
                    i--;
                    continue;
                }
                curr.Boomb = true;
            }
            GameEntity newGame = new GameEntity()
            {
                Width = game.width,
                Height = game.height,
                Mines_count = game.mines_count,
                FieldEntity = fieldList
            };
            Guid game_id = await _manager.MinesweeperRepository.CreateGameAsync(newGame);
            return game_id;
        }

        public async Task<GameInfoResponse> GetGame(Guid id)
        {
            GameEntity game = await GetGameEntity(id);

            int height = game.Height;
            int width = game.Width;
            string[][] fields = new string[height][];

            FieldEntity boombField = game.FieldEntity.Where(x => x.Opened == true && x.Boomb == true).FirstOrDefault();

            for (int i = 0; i < height; i++)
            {
                var row = new string[width];
                for (int j = 0; j < width; j++)
                {
                    FieldEntity currField = game.FieldEntity.ElementAt((i * width) + j);
                    string symbol = voidSymbol;
                    if (game.Completed)
                    {
                        if (boombField != null)
                        {
                            if (boombField.Equals(currField))
                            {
                                symbol = boombSymbol;
                            }
                            else if (!currField.Boomb)
                            {
                                symbol = GetNeighbours(game.FieldEntity.ToList(), i, j, height, width).ToString();
                            }
                        }
                        else
                        {
                            if (currField.Boomb)
                            {
                                symbol = minesSymbol;
                            }
                            else
                            {
                                symbol = GetNeighbours(game.FieldEntity.ToList(), i, j, height, width).ToString();
                            }
                        }
                    }
                    else if (currField.Opened)
                    {
                        symbol = GetNeighbours(game.FieldEntity.ToList(), i, j, height, width).ToString();
                    }
                    row[j] = symbol;
                }
                fields[i] = row;
            }

            GameInfoResponse gameInfo = new GameInfoResponse()
            {
                game_id = game.Id,
                width = game.Width,
                height = game.Height,
                mines_count = game.Mines_count,
                completed = game.Completed,
                field = fields
            };

            return gameInfo;
        }
        private int GetNeighbours(List <FieldEntity> fields, int row, int column, int height, int weight)
        {
            int neighbor = 0;
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = column - 1; j <= column + 1; j++)
                {
                    if (i >= 0 && i < height && j >= 0 && j < weight)
                    {
                        if (fields[i * weight + j].Boomb)
                            neighbor++;
                    }
                }
            }
            return neighbor;
        }

        private async Task<GameEntity> GetGameEntity(Guid id)
        {
            GameEntity game = await _manager.MinesweeperRepository.GetGame(id);
            if (game == null)
                throw new ApplicationException("Не найдена игра");
            return game;
        }
        public async Task GameTurn(GameTurnRequest turn)
        {
            GameEntity game = await GetGameEntity(turn.game_id);

            if (game.Completed)
                throw new ApplicationException("Игра закончена");

            int index = (game.Width * turn.row) + turn.col;

            FieldEntity field = game.FieldEntity?.ElementAtOrDefault(index);

            if (field == null)
                throw new ApplicationException("Не найдено поле");

            field.Opened = true;
            FieldEntity lastField = game.FieldEntity.Where(x => x.Opened == false && x.Boomb == false).FirstOrDefault();
            if (field.Boomb || lastField == null)
            {
                game.Completed = true;
            }
            
            await _manager.MinesweeperRepository.UpdateGame(game);
        }
    }
}
