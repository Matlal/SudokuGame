using Sudoku.Core.Entities;
using Sudoku.Infrastructure.Helpers;
using Sudoku.Infrastructure.Repositories;

namespace Sudoku.Infrastructure.Services;

public class SudokuService : ISudokuService
{
    private readonly ISudokuRepository _sudokuRepo;

    public SudokuService(ISudokuRepository sudokuRepo) => _sudokuRepo = sudokuRepo;

    public async Task<(InternalStatusCodeResult Result, SudokuGame? Game)> GetCurrentGameStateAsync(CancellationToken cancellationToken = default)
    {
        var game = await _sudokuRepo.GetCurrentGameAsync(cancellationToken);
        return ((game != null ? InternalStatusCodeResult.Success : InternalStatusCodeResult.NoCurrentGame), game);
    }

    public async Task<SudokuGame> NewGameAsync(DifficultyLevel level, CancellationToken cancellationToken = default)
    {
        var boardState = await SudokuBoardGenerator.Create(level, cancellationToken);
        return _sudokuRepo.Create(boardState);
    }

    public async Task<int?> GetStateAtPositionAsync(
        (int x, int y) position,
        CancellationToken cancellationToken = default)
    {
        (var res, var game) = await GetCurrentGameStateAsync(cancellationToken);
        if (res == InternalStatusCodeResult.Success && game != null)
        {
            return game.GetStateAtPosition(position);
        }
        return null;
    }

    public async Task<InternalStatusCodeResult> UpdateAsync(
        (int x, int y) position,
        int number,
        CancellationToken cancellationToken = default)
        => await _sudokuRepo.UpdateAsync(position, number, cancellationToken);
}