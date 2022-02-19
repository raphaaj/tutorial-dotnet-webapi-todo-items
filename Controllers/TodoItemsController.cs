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

    public TodoItemsController(TodoContext context, IOptions<ExecutionOptionsDTO> executionOptions)
    {
      _context = context;
      _executionOptions = executionOptions.Value;
    }

    // GET: api/TodoItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
    {
      return await _context.TodoItems.Select(i => TodoItemDTO.FromModel(i)).ToListAsync();
    }

    // GET: api/TodoItems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
      var todoItem = await _context.TodoItems.FindAsync(id);

      if (todoItem == null)
      {
        return NotFound();
      }

      return TodoItemDTO.FromModel(todoItem);
    }

    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoItemDTO)
    {
      if (IsReadOnlyExecution()) return UnprocessableEntity();

      if (id != todoItemDTO.Id) return BadRequest();

      var todoItem = await _context.TodoItems.FindAsync(id);
      if (todoItem == null) return NotFound();

      todoItem.Name = todoItemDTO.Name;
      todoItem.IsComplete = todoItemDTO.IsComplete;

      try
      {
        await _context.SaveChangesAsync();
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
      if (IsReadOnlyExecution()) return UnprocessableEntity();

      var todoItem = new TodoItem
      {
        Name = todoItemDTO.Name,
        IsComplete = todoItemDTO.IsComplete,
      };

      _context.TodoItems.Add(todoItem);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, TodoItemDTO.FromModel(todoItem));
    }

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
      if (IsReadOnlyExecution()) return UnprocessableEntity();

      var todoItem = await _context.TodoItems.FindAsync(id);
      if (todoItem == null) return NotFound();

      _context.TodoItems.Remove(todoItem);
      await _context.SaveChangesAsync();

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
