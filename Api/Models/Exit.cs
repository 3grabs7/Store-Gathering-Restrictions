using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class Exit
    {
        public int Id { get; set; }
        public Section Section { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
