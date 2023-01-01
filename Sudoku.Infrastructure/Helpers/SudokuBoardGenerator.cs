using Sudoku.Infrastructure.Services;

namespace Sudoku.Infrastructure.Helpers;

public static class SudokuBoardGenerator
{
    public static async Task<int?[,]> Create(DifficultyLevel level, CancellationToken token = default)
    {
        var rnd = new Random();
        var toBeAdded = level switch
        {
            DifficultyLevel.Easy => 61,
            DifficultyLevel.Normal => 51,
            DifficultyLevel.Hard => 41,
            DifficultyLevel.Veteran => 31,
            DifficultyLevel.Insane => 21,
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };

        var board = new int?[9, 9];

        for (var i = 0; i < toBeAdded; i++)
        {
            var possibleNumbers = Enumerable.Range(1, 9).ToList();
            while (possibleNumbers.Count > 0)
            {
                var index = rnd.Next(0, possibleNumbers.Count - 1);
                var number = possibleNumbers[index];
                var valueTuple = await TryFindPossibleCell(board, number, token);
                if (valueTuple.Result)
                {
                    board[valueTuple.Position.x, valueTuple.Position.y] = number;
                    break;
                }
                possibleNumbers.RemoveAt(index);
            }
        }

        return board;
    }

    public static async Task<(bool Result, (int x, int y) Position)> TryFindPossibleCell(
        int?[,] board, int number, CancellationToken token = default)
    {
        for (var x = 0; x < board.GetLength(0); x++)
        {
            for (var y = 0; y < board.GetLength(1); y++)
            {
                if (board[x, y] == null 
                    && (await SudokuBoardRulesHelper.IsOk(board, number, (x, y), token)).Result)
                {
                    return (true, (x, y));
                }
            }   
        }

        return (false, (-1, -1));
    }
}