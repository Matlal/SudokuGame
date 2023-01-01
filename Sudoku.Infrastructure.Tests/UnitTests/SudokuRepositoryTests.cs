using Sudoku.Infrastructure.Repositories;
using Sudoku.Infrastructure.Services;

namespace Sudoku.Infrastructure.Tests.UnitTests;

public class SudokuRepositoryTests
{
    [Fact]
    public async Task Create_OnAction_ReturnInputBoardAndIsNotFinished()
    {
        //ARRANGE
        var board = new int?[9,9];
        board[7, 8] = 3;
        var repo = new SudokuRepository();
        
        //ACT
        var result = repo.Create(board);
        var getResult = await repo.GetCurrentGameAsync();
        //ASSERT
        Assert.Equal(board, result.Board);
        Assert.Equal(board, getResult!.Board);
        Assert.False(result.IsFinished);
    }
    
    [Fact]
    public async Task Update_GoodUpdate_ReturnSuccess()
    {
        //ARRANGE
        var board = new int?[9,9];
        var repo = new SudokuRepository();
        repo.Create(board);
        (int x, int y) position = (7, 8);
        const int number = 6;

        //ACT
        var result = await repo.UpdateAsync(position, number);

        //ASSERT
        Assert.Equal(InternalStatusCodeResult.Success, result);
    }
}