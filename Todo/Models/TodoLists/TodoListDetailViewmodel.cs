using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Todo.Models.TodoItems;

namespace Todo.Models.TodoLists
{
    public class TodoListDetailViewmodel
    {
        public int TodoListId { get; }
        public string Title { get; }
        public ICollection<TodoItemSummaryViewmodel> Items { get; }

        [Display(Name = "Hide done items")]
        public bool HideDoneItems { get; set; }
        public bool OrderByRank { get; set; }

        public TodoListDetailViewmodel(int todoListId, string title, ICollection<TodoItemSummaryViewmodel> items, bool hideDoneItems, bool orderByRank)
        {
            Items = items;
            TodoListId = todoListId;
            Title = title;
            HideDoneItems = hideDoneItems;
            OrderByRank = orderByRank;
        }
    }
}