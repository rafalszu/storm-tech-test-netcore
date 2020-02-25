using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Data.Entities;

namespace Todo.Services
{
    public class ApplicationDbContextWrapper : IApplicationDbContextWrapper 
    {
        private readonly ApplicationDbContext dbContext;
        public ApplicationDbContextWrapper(ApplicationDbContext dbContext) => this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public IQueryable<TodoList> RelevantTodoLists(string userId)
        {
            return dbContext.TodoLists.Include(tl => tl.Owner)
                .Include(tl => tl.Items)
                .Where(tl => tl.Owner.Id == userId);
        }

        public TodoItem SingleTodoItem(int todoItemId)
        {
            return dbContext.TodoItems.Include(ti => ti.TodoList).Single(ti => ti.TodoItemId == todoItemId);
        }

        public TodoList SingleTodoList(int todoListId)
        {
            return dbContext.TodoLists.Include(tl => tl.Owner)
                .Include(tl => tl.Items)
                .ThenInclude(ti => ti.ResponsibleParty)
                .Single(tl => tl.TodoListId == todoListId);
        }
    }
}