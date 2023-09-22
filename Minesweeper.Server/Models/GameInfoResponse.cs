
namespace Minesweeper.Server.Models
{
    public class GameInfoResponse
    {
        public Guid game_id { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int mines_count { get; set; }
        public bool completed { get; set; }
        
        public string[][]? field { get; set; }
    }
}
