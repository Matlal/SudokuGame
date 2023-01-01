using Sudoku.Core.Entities;

namespace SudokuWebAPI.Models;

public class SudokuOperationModel
{
    public static SudokuOperationModel ToModel((int x, int y) position, int number) =>
        new()
        {
            Position = position,
            Number = number,
        };

    public (int x, int y) Position { get; set; }
    public int Number { get; set; }
}
