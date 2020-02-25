using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoLists;
using Todo.Models.TodoLists;
using Todo.Services;

namespace Todo.Controllers
{
    [Authorize]
    public class TodoListController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IUserStore<IdentityUser> userStore;
        private readonly IApplicationDbContextWrapper dbContextWrapper;

        public TodoListController(ApplicationDbContext dbContext, IApplicationDbContextWrapper dbContextWrapper, IUserStore<IdentityUser> userStore)
        {
            this.dbContext = dbContext;
            this.userStore = userStore;
            this.dbContextWrapper = dbContextWrapper;
        }

        public IActionResult Index()
        {
            var userId = User.Id();
            var todoLists = dbContextWrapper.RelevantTodoLists(userId);
            var viewmodel = TodoListIndexViewmodelFactory.Create(todoLists);
            return View(viewmodel);
        }

        public IActionResult Detail(int todoListId, bool? hideDoneItems)
        {
            var todoList = dbContextWrapper.SingleTodoList(todoListId); //dbContext.SingleTodoList(todoListId);
            if (todoList != null && (todoList.Items?.Any() ?? false))
            {
                if (hideDoneItems == true)
                    todoList.Items = todoList.Items.Where(item => item.IsDone == false).ToList();
            }

            var viewmodel = TodoListDetailViewmodelFactory.Create(todoList, hideDoneItems ?? false);
            return View(viewmodel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TodoListFields());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoListFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            var currentUser = await userStore.FindByIdAsync(User.Id(), CancellationToken.None);

            var todoList = new TodoList(currentUser, fields.Title);

            await dbContext.AddAsync(todoList);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Create", "TodoItem", new { todoList.TodoListId });
        }
    }
}