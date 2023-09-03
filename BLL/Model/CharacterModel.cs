using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Model
{
    public class CharacterModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? Species { get; set; }
        public string? Location { get; set; }
        public string? Image { get; set; }
        public int? OriginalId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public List<string>?Episode { get; set; }

    }
}
