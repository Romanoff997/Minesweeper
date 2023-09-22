
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Minesweeper.Server.Domain.Entities
{
    public class GameEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Mines_count { get; set; }
        public bool Completed { get; set; }

        public virtual ICollection<FieldEntity> FieldEntity { get; set; }
    }

   
}
