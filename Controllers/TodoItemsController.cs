#nullable disable
using DotnetTutorialWebapiTodoItems.DTOs;
using DotnetTutorialWebapiTodoItems.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DotnetTutorialWebapiTodoItems.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TodoItemsController : ControllerBase
  {
    private readonly TodoContext _context;
    private readonly ExecutionOptionsDTO _executionOptions;
    private readonly ILogger _logger;

    public TodoItemsController(
      TodoContext context,
      IOptions<ExecutionOptionsDTO> executionOptions,
      ILogger<TodoItemsController> logger)
    {
      _context = context;
      _executionOptions = executionOptions.Value;
      _logger = logger;
    }

    // GET: api/TodoItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
    {
      _logger.LogDebug("Retrieving todo items");
      var todoItems = await _context.TodoItems.Select(i => TodoItemDTO.FromModel(i)).ToListAsync();
      _logger.LogDebug("Todo items retrieved");

      return todoItems;
    }

    // GET: api/TodoItems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
      _logger.LogDebug($"Retrieving todo item with id {id}");
      var todoItem = await _context.TodoItems.FindAsync(id);

      if (todoItem == null)
      {
        _logger.LogDebug($"No todo item found with id {id}");
        return NotFound();
      }

      _logger.LogDebug($"Retrieved todo item with id {id}");

      return TodoItemDTO.FromModel(todoItem);
    }

    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoItemDTO)
    {
      if (IsReadOnlyExecution())
      {
        _logger.LogDebug("API running in readonly mode. Writing operation blocked");
        return UnprocessableEntity();
      }

      if (id != todoItemDTO.Id) return BadRequest();

      _logger.LogDebug($"Retrieving todo item with id {id}");
      var todoItem = await _context.TodoItems.FindAsync(id);

      if (todoItem == null)
      {
        _logger.LogDebug($"No todo item found with id {id}");
        return NotFound();
      }

      _logger.LogDebug($"Retrieved todo item with id {id}. Updating item");
      todoItem.Name = todoItemDTO.Name;
      todoItem.IsComplete = todoItemDTO.IsComplete;

      try
      {
        await _context.SaveChangesAsync();
        _logger.LogDebug($"Todo item id {id} updated");
      }
      catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
      {
        return NotFound();
      }

      return NoContent();
    }

    // POST: api/TodoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoItemDTO)
    {
      if (IsReadOnlyExecution())
      {
        _logger.LogDebug("API running in readonly mode. Writing operation blocked");
        return UnprocessableEntity();
      }

      _logger.LogDebug("Creating new todo item");
      var todoItem = new TodoItem
      {
        Name = todoItemDTO.Name,
        IsComplete = todoItemDTO.IsComplete,
      };

      _context.TodoItems.Add(todoItem);
      await _context.SaveChangesAsync();
      _logger.LogDebug($"Todo item created with id {todoItem.Id}");

      return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, TodoItemDTO.FromModel(todoItem));
    }

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
      if (IsReadOnlyExecution())
      {
        _logger.LogDebug("API running in readonly mode. Writing operation blocked");
        return UnprocessableEntity();
      }

      _logger.LogDebug($"Retrieving todo item with id {id}");
      var todoItem = await _context.TodoItems.FindAsync(id);

      if (todoItem == null)
      {
        _logger.LogDebug($"No todo item found with id {id}");
        return NotFound();
      }

      _logger.LogDebug($"Retrieved todo item with id {id}. Deleting item");
      _context.TodoItems.Remove(todoItem);
      await _context.SaveChangesAsync();
      _logger.LogDebug("Item deleted");

      return NoContent();
    }

    private bool TodoItemExists(long id)
    {
      return _context.TodoItems.Any(e => e.Id == id);
    }

    private bool IsReadOnlyExecution()
    {
      return _executionOptions.ReadOnly;
    }
  }
}
