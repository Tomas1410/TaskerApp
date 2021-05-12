using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskerApp.Models
{
    public class ApplicationUser :IdentityUser
    {
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
