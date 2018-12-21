using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutService
{
    public class Exercise : BaseModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string VideoLink { get; set; }
        public string Description { get; set; }
    }
}
