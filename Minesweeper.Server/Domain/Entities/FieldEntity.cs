using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Minesweeper.Server.Domain.Entities
{
    public class FieldEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool Boomb { get; set; } = false;
        public bool Opened { get; set; } = false;
        public Guid? GameEntityId { get; set; }
        public virtual GameEntity GameEntity { get; set; }
    }
}
