using System.Linq;
using Todo.Data.Entities;

namespace Todo.Services
{
    public interface IApplicationDbContextWrapper
    {
        IQueryable<TodoList> RelevantTodoLists(string userId);

        TodoList SingleTodoList(int todoListId);

        TodoItem SingleTodoItem(int todoItemId);
    }
}