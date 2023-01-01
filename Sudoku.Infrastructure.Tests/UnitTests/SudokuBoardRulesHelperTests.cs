using Sudoku.Infrastructure.Helpers;

namespace Sudoku.Infrastructure.Tests.UnitTests;

public class SudokuBoardRulesHelperTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ValueDoesNotExistInColumn_IfExists_ReturnTrue(bool exists)
    {
        //ARRANGE
        var board = new int?[9, 9];
        board[0, 0] = 1;
        board[0, 1] = exists ? 3 : 2;
        const int value = 3;
        var cell = (0,2);
        //ACT
        var result = SudokuBoardRulesHelper.ValueDoesNotExistInColumn(board, value, cell);
        //ASSERT
        Assert.True(exists ? !result : result);
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ValueDoesNotExistInRow_IfExists_ReturnTrue(bool exists)
    {
        //ARRANGE
        var board = new int?[9, 9];
        board[0, 0] = 1;
        board[1, 0] = exists ? 3 : 2;
        const int value = 3;
        var cell = (2,0);
        //ACT
        var result = SudokuBoardRulesHelper.ValueDoesNotExistInRow(board, value, cell);
        //ASSERT
        Assert.True(exists ? !result : result);
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ValueDoesNotExistInSquare_IfExists_ReturnTrue(bool exists)
    {
        //ARRANGE
        var board = new int?[9, 9];
        board[0, 0] = 1;
        board[1, 1] = exists ? 3 : 2;
        const int value = 3;
        var cell = (2,2);
        //ACT
        var result = SudokuBoardRulesHelper.ValueDoesNotExistInSquare(board, value, cell);
        //ASSERT
        Assert.True(exists ? !result : result);
    }
}