using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TaskerApp.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Boolean IsFinished { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime FinishDate { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
