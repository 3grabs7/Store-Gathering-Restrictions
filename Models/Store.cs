using System.Collections.Generic;

namespace Models
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaximumPeopleAllowed { get; set; }
        public ICollection<Section> Sections { get; set; }
    }
}
