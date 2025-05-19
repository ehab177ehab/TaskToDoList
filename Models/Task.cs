using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TaskToDoList.Models
{
    public class TaskModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        [Required]
        public string Priority { get; set; } = null!; // "Low", "Medium", "High"

        [Required]
        public string Status { get; set; } = null!; // "Pending" or "Completed"
        
        

    }
}
