using System.ComponentModel.DataAnnotations;
using Todo.Data.Entities;

namespace Todo.Models.TodoItems
{
    public class TodoItemFieldsBase
    {
        public int TodoListId { get; set; }
        
        [Required] // ensure tasks have a title
        public string Title { get; set; }

        public string TodoListTitle { get; set; }

        [Display(Name = "Assigned to")]
        public string ResponsiblePartyId { get; set; }

        public Importance Importance { get; set; }

    }
}