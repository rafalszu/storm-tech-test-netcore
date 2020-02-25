using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Todo.Controllers;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Models.TodoLists;
using Todo.Services;
using Xunit;

namespace Todo.Tests
{
    public class TodoListControllerTests
    {
        Mock<IUserStore<IdentityUser>> userStoreMock = new Mock<IUserStore<IdentityUser>>();
        Mock<IApplicationDbContextWrapper> applicationDbContextMock = new Mock<IApplicationDbContextWrapper>();

        [Theory]
        [InlineData(null)]
        [InlineData(false)]
        [InlineData(true)]
        public void DetailTakesHideDoneItemsIntoAccount(bool? hide)
        {
            var todoList = new TestTodoListBuilder(new IdentityUser("alice@example.com"), "shopping")
                    .WithItem("bread", Importance.High)
                    .WithItem("milk", Importance.Low)
                    .WithItem("eggs", Importance.Medium)
                    .Build();
            
            // set first item as done
            todoList.Items.First().IsDone = true;

            applicationDbContextMock
                .Setup(x => 
                    x.SingleTodoList(It.IsAny<int>())
                ).Returns(
                    todoList
                );
                
            TodoListController controller = new TodoListController(null, applicationDbContextMock.Object, userStoreMock.Object);
            IActionResult actual = controller.Detail(1, hide, null);

            ViewResult viewResult = actual as ViewResult;
            viewResult
                .Should()
                .NotBeNull();

            viewResult.Model
                .Should()
                .BeOfType<TodoListDetailViewmodel>();

            var viewmodel = viewResult.Model as TodoListDetailViewmodel;

            viewmodel.Items
                .Should()
                .NotBeNull();
            
            int expectedCount = 3;
            if(hide == true)
                expectedCount = 2;

            viewmodel.Items
                .Should()
                .HaveCount(expectedCount);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(false)]
        [InlineData(true)]
        public void DetailTakesOrderByRankIntoAccount(bool? orderByRank)
        {
            var todoList = new TestTodoListBuilder(new IdentityUser("alice@example.com"), "shopping")
                    .WithItem("bread", Importance.High)
                    .WithItem("milk", Importance.Low)
                    .WithItem("eggs", Importance.Medium)
                    .Build();

            // set rank on items
            todoList.Items.ElementAt(0).Rank = 2;
            todoList.Items.ElementAt(1).Rank = 1;
            todoList.Items.ElementAt(2).Rank = 3;

            applicationDbContextMock
                .Setup(x => 
                    x.SingleTodoList(It.IsAny<int>())
                ).Returns(
                    todoList
                );
                
            TodoListController controller = new TodoListController(null, applicationDbContextMock.Object, userStoreMock.Object);
            IActionResult actual = controller.Detail(1, null, orderByRank);

            ViewResult viewResult = actual as ViewResult;
            viewResult
                .Should()
                .NotBeNull();

            viewResult.Model
                .Should()
                .BeOfType<TodoListDetailViewmodel>();

            var viewmodel = viewResult.Model as TodoListDetailViewmodel;

            viewmodel.Items
                .Should()
                .NotBeNull();
            
            string expectedFirstItemTitle = "bread"; // beacuse by default this should be sorted by importance
            if(orderByRank == true)
                expectedFirstItemTitle = "milk";

            viewmodel.Items
                .Should()
                .HaveCount(3);

            viewmodel.Items
                .ElementAt(0)
                .Title
                .Should()
                .BeSameAs(expectedFirstItemTitle);
        }
    }
}