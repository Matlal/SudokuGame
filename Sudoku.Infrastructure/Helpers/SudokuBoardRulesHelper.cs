namespace Sudoku.Infrastructure.Helpers;

public static class SudokuBoardRulesHelper
{
    public static async Task<(bool Result, SudokuRuleViolation Violation)> IsOk(
        int?[,] board, int value, (int x, int y) cell ,CancellationToken cancellationToken = default)
    {
        if (board[cell.x, cell.y] != null)
            return (false, SudokuRuleViolation.ValueAlreadyExistForPosition);

        var tasks = new List<Task<(bool Result, SudokuRuleViolation Rule)>>
        {
            Task.Run(() =>
                (ValueDoesNotExistInSquare(board, value, cell), SudokuRuleViolation.ValueExistInSquare), cancellationToken), 
            Task.Run(() => 
                (ValueDoesNotExistInRow(board, value, cell), SudokuRuleViolation.ValueExistInRow), cancellationToken), 
            Task.Run(() => 
                (ValueDoesNotExistInColumn(board, value, cell), SudokuRuleViolation.ValueExistInColumn), cancellationToken)
        };
        
        while (tasks.Any())
        {
            var completedTask = await Task.WhenAny(tasks);
            var valueTuple = await completedTask;
            if (!valueTuple.Result)
                return valueTuple;

            tasks.Remove(completedTask);
        }
        
        return (true, SudokuRuleViolation.None);
    }

    public static bool ValueDoesNotExistInColumn(
        int?[,] board, int value, (int x, int y) cell)
    {
        for (var y = 0; y < board.GetUpperBound(1); y++)
        {
            if (cell.y != y && board[cell.x, y] == value) 
                return false;
        }

        return true;
    }

    public static bool ValueDoesNotExistInRow(
        int?[,] board, int value, (int x, int y) cell)
    {
        for (var x = 0; x < board.GetUpperBound(0); x++)
        {
            if (cell.x != x && board[x, cell.y] == value) 
                return false;
        }

        return true;
    }

    public static bool ValueDoesNotExistInSquare(
        int?[,] board, int value, (int x, int y) cell)
    {
        var startX = Math.Max(cell.x - cell.x % 3 - 1, 0);
        var startY = Math.Max(cell.y - cell.y % 3 - 1, 0);
        for (var x = startX; x < startX + 3; x++)
        {
            for (var y = startY; y < startY + 3; y++)
            {
                if (value == board[x, y] && (cell.x != x || cell.y != y))
                    return false;
            }
        }

        return true;
    }
}

public enum SudokuRuleViolation
{
    None = 0,
    ValueExistInSquare = 1,
    ValueExistInRow = 2,
    ValueExistInColumn = 3,
    ValueAlreadyExistForPosition = 4,
}