using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WorkoutService
{
    public class User : BaseModel
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Username { get; set; }
        
        public string Email { get; set; }

        public string Hash { get; set; }

        public string Salt { get; set; }

        public int RoleID { get; set; }

        public string PictureUrl { get; set; }
    }
}
