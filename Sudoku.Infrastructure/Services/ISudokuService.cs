using Sudoku.Core.Entities;
using System.Text.Json.Serialization;

namespace Sudoku.Infrastructure.Services;

public interface ISudokuService
{
    Task<(InternalStatusCodeResult Result, SudokuGame? Game)> GetCurrentGameStateAsync(CancellationToken cancellationToken = default);
    Task<SudokuGame> NewGameAsync(DifficultyLevel level, CancellationToken cancellationToken = default);
    Task<int?> GetStateAtPositionAsync((int x, int y) position, CancellationToken cancellationToken = default);
    Task<InternalStatusCodeResult> UpdateAsync((int x, int y) position, int number, CancellationToken cancellationToken = default);
}

public enum DifficultyLevel
{
    Easy,
    Normal,
    Hard,
    Veteran,
    Insane,
}

public enum InternalStatusCodeResult
{
    Success = 0,
    Error = 1,
    OutOfBounds,
    NoCurrentGame,
    RuleViolation
}