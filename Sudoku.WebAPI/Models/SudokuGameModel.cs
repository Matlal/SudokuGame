using Sudoku.Core.Entities;

namespace SudokuWebAPI.Models;

public class SudokuGameModel
{
    public static SudokuGameModel ToModel(SudokuGame entity) =>
        new()
        {
            StartedAt = entity.StatedAt,
            IsFinished = entity.IsFinished,
            Board = entity.Board,
        };

    public DateTime StartedAt { get; set; }
    public bool IsFinished { get; set; }
    public int?[,] Board { get; set; }
}
