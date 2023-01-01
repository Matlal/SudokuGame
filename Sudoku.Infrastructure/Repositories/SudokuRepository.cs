using Sudoku.Core.Entities;
using Sudoku.Infrastructure.Helpers;
using Sudoku.Infrastructure.Services;

namespace Sudoku.Infrastructure.Repositories;

public class SudokuRepository : ISudokuRepository
{
    private SudokuGame? _game;
    
    public Task<SudokuGame?> GetCurrentGameAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_game);

    public SudokuGame Create(int?[,] startStates, CancellationToken cancellationToken = default) => 
        _game = new SudokuGame(startStates);

    public async Task<InternalStatusCodeResult> UpdateAsync(
        (int x, int y) position,
        int number,
        CancellationToken cancellationToken = default)
    {
        if (_game == null)
        {
            return InternalStatusCodeResult.NoCurrentGame;
        }
        if (!SudokuGame.PositionIsInsideBoarder(position))
        {
            return InternalStatusCodeResult.OutOfBounds;
        }
        if (!(await SudokuBoardRulesHelper.IsOk(_game.Board, number, position, cancellationToken)).Result)
        {
            return InternalStatusCodeResult.RuleViolation;
        }

        _game.Board[position.x, position.y] = number;
        return InternalStatusCodeResult.Success;
    }
}