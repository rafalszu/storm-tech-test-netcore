using FluentAssertions;
using Todo.Models.TodoItems;
using Xunit;

namespace Todo.Tests
{
    public class ModelTests
    {
        [Fact]
        public void TodoItemCreateFieldsDerivedFromTodoItemBaseClass()
        {
            typeof(TodoItemCreateFields)
                .Should()
                .BeDerivedFrom<TodoItemFieldsBase>();
        }

        [Fact]
        public void TodoItemEditFieldsDerivedFromTodoItemBaseClass()
        {
            typeof(TodoItemEditFields)
                .Should()
                .BeDerivedFrom<TodoItemFieldsBase>();
        }
    }
}