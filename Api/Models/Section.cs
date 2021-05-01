using System.Collections.Generic;

namespace Api.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Store Store { get; set; }
        public ICollection<Enter> Enters { get; set; }
        public ICollection<Exit> Exits { get; set; }
    }
}
