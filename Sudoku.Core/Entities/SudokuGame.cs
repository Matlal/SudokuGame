namespace Sudoku.Core.Entities;

public class SudokuGame
{
    public SudokuGame(int?[,] startStates)
    {
        StatedAt = DateTime.Now;
        if (startStates.Rank != Board.Rank || startStates.Length != Board.Length)
            throw new Exception("could not set the new boardState, wrong dimensions");
        Board = startStates;
    }
    
    public DateTime StatedAt { get; }
    public bool IsFinished { get; set; }
    public int?[,] Board { get; } = new int?[9, 9];

    public int? GetStateAtPosition((int X, int Y) position)
    {
        if (!PositionIsInsideBoarder(position))
        {
            throw new ArgumentOutOfRangeException(nameof(position));
        }

        return Board[position.X, position.Y];
    }

    public static bool PositionIsInsideBoarder((int X, int Y) position) =>
        position.X is >= 0 and <= 8 && position.Y is >= 0 and <= 8;
}
