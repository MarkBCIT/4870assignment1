using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace assignment.ModelViews
{
    public class Boat
    {
        [Key]
        public int BoatId { set; get; }
        public string BoatName { set; get; }
        public string Picture { set; get; }
        public double LengthInFeet { set; get; }
        public string Make { set; get; }
        public string Description { set; get; }
    }
}
