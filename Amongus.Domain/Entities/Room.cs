using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amongus.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Password { get; set; }
        public int? CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        public User? Creator { get; set; }
        public bool IsStartGame { get; set; }
    }
}
