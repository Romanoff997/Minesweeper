using Microsoft.AspNetCore.Mvc;
using Minesweeper.Server.Models;
using Minesweeper.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Server.Controllers
{
    [ApiController]
    public class MinesweeperController : ControllerBase
    {
        private readonly MinesweeperService _minesweeperService;
        public MinesweeperController(MinesweeperService minesweeperService)
        {
            _minesweeperService = minesweeperService;
        }

        [Route("new")]
        [HttpPost]
        public async Task<IActionResult> NewGame(NewGameRequest newGameRequest)
        {
            var id = await _minesweeperService.CreateGame(newGameRequest);
            var result = await _minesweeperService.GetGame(id);
            return Ok(result);
        }

        [Route("turn")]
        [HttpPost]
        public async Task<IActionResult> GameTurn(GameTurnRequest gameTurnRequest)
        {
            await _minesweeperService.GameTurn(gameTurnRequest);
            var result = await _minesweeperService.GetGame(gameTurnRequest.game_id);
            return Ok(result);
        }

    }
}
