using Sudoku.Core.Entities;
using Sudoku.Infrastructure.Services;

namespace Sudoku.Infrastructure.Repositories;

public interface ISudokuRepository 
{
    Task<SudokuGame?> GetCurrentGameAsync(CancellationToken cancellationToken = default);
    SudokuGame Create(int?[,] startStates, CancellationToken cancellationToken = default);
    Task<InternalStatusCodeResult> UpdateAsync((int x, int y) position, int number, CancellationToken cancellationToken = default);
}