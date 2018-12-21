using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutService
{
    public class BaseModel
    {
        public const int InvalidId = -1;

        public int Id { get; set; } = InvalidId;
    }
}
