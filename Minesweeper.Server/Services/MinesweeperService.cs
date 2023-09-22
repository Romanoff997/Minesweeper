using CommitExplorerOAuth2AspNET.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Minesweeper.Server.Domain.Entities;
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
                var curr = fieldList[index];
                if (curr.Boomb)
                {
                    i--;
                    continue;
                }
                curr.Boomb = true;
            }
            var newGame = new GameEntity()
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
            var game = await _manager.MinesweeperRepository.GetGame(id);
            if (game == null)
                throw new ApplicationException("Не найдена игра");

            int height = game.Height;
            int width = game.Width;
            var field = new string[height][];

            var boombField = game.FieldEntity.Where(x => x.Opened == true && x.Boomb == true).FirstOrDefault();

            for (int i = 0; i < height; i++)
            {
                var row = new string[width];
                for (int j = 0; j < width; j++)
                {
                    var currField = game.FieldEntity.ElementAt((i * width) + j);
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
                field[i] = row;
            }

            var gameInfo = new GameInfoResponse()
            {
                game_id = game.Id,
                width = game.Width,
                height = game.Height,
                mines_count = game.Mines_count,
                completed = game.Completed,
                field = field
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

        public async Task GameTurn(GameTurnRequest turn)
        {
            var game = await _manager.MinesweeperRepository.GetGame(turn.game_id);
            if (game == null)
                throw new ApplicationException("Не найдена игра");

            else if(game.Completed)
                throw new ApplicationException("Игра закончена");

            int index = (game.Width * turn.row) + turn.col;
            var field = game?.FieldEntity?.ElementAtOrDefault(index);

            if (field == null)
                throw new ApplicationException("Не найдено поле");

            field.Opened = true;
            var lastField = game.FieldEntity.Where(x => x.Opened == false).Where(x => x.Boomb == false).FirstOrDefault();
            if (field.Boomb || lastField == null)
            {
                game.Completed = true;
            }
            
            await _manager.MinesweeperRepository.UpdateGame(game);
        }
    }
}
