using DotnetTutorialWebapiTodoItems.Models;

namespace DotnetTutorialWebapiTodoItems.DTOs;

public class TodoItemDTO
{
  public long Id { get; set; }
  public string? Name { get; set; }
  public bool IsComplete { get; set; }

  public static TodoItemDTO FromModel(TodoItem todoItem)
  {
    return new TodoItemDTO
    {
      Id = todoItem.Id,
      Name = todoItem.Name,
      IsComplete = todoItem.IsComplete
    };
  }
}