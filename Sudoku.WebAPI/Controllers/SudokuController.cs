using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sudoku.Infrastructure.Services;
using SudokuWebAPI.Models;

namespace SudokuWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SudokuController : ControllerBase
{
    private readonly ILogger<SudokuController> _logger;
    private readonly ISudokuService _sudokuService;

    public SudokuController(ILogger<SudokuController> logger, ISudokuService sudokuService)
    {
        _logger = logger;
        _sudokuService = sudokuService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        (var result, var game) = await _sudokuService.GetCurrentGameStateAsync();
        if (result != InternalStatusCodeResult.Success || game == null)
        {
            return NotFound();
        }

        return base.Ok(SudokuGameModel.ToModel(game));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(DifficultyLevel level)
    {
        var newGame = await _sudokuService.NewGameAsync(level);
        return CreatedAtAction(nameof(Get), SudokuGameModel.ToModel(newGame));
    }
    
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Patch(
        [FromBody] JsonPatchDocument<SudokuOperationModel> patchDocument)
    {
        var allowedPaths = new[]
        {
            $"/{nameof(SudokuOperationModel.Number)}",
            $"/{nameof(SudokuOperationModel.Position)}",
        };

        for (var i = patchDocument.Operations.Count - 1; i >= 0; i--)
        {
            var op = patchDocument.Operations[i];
            if (!allowedPaths.Any(x =>
                    string.Equals(x, op.path, StringComparison.CurrentCultureIgnoreCase)))
            {
                patchDocument.Operations.RemoveAt(i);
                _logger.LogWarning($"Removed not allowed operation: Type: {op.OperationType}, Path: {op.path}");
            }
        }
        
        var operationModel = new SudokuOperationModel();
        patchDocument.ApplyTo(operationModel, ModelState);
        
        if (!ModelState.IsValid || !TryValidateModel(operationModel))
        {
            return BadRequest(ModelState);
        }
        
        var number = await _sudokuService.GetStateAtPositionAsync(operationModel.Position);
        if (number != null)
            return Problem(
                $"Current state on position (x:{operationModel.Position.x}, y:{operationModel.Position.y}) can not be updated with number {operationModel.Number}",
                    statusCode: 500
                );
        
        var updatedGame = await _sudokuService.UpdateAsync(operationModel.Position, operationModel.Number);
        if (updatedGame != InternalStatusCodeResult.Success)
            return Problem(
                $"Current state on position (x:{operationModel.Position.x}, y:{operationModel.Position.y}) can not be updated with number {operationModel.Number}",
                statusCode: 500
            );
        
        return new ObjectResult(operationModel);
    }
}