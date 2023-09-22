using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Server.Models
{
    public class NewGameRequest
    {
        public int width { get; set; }
        public int height { get; set; }
        public int mines_count { get; set; }
    }
}
