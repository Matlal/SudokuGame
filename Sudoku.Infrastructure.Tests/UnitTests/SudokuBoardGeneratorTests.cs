using Sudoku.Infrastructure.Helpers;

namespace Sudoku.Infrastructure.Tests.UnitTests;

public class SudokuBoardGeneratorTests
{
    [Fact]
    public async Task TryFindPossibleCell_PositionExistsAndGoodValue_ReturnTrue()
    {
        //ARRANGE
        var board = new int?[,] {{1, 2, 3}, {4, 5, null}, {7, 8, 9}};
        
        //ACT
        var (result, position) = await SudokuBoardGenerator.TryFindPossibleCell(board, 6);
        
        //ASSERT
        Assert.True(result);
        Assert.Equal((1,2), position);
    }
    
    [Fact]
    public async Task TryFindPossibleCell_NoPositionExists_ReturnFalse()
    {
        //ARRANGE
        var board = new int?[,] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
        
        //ACT
        var (result, position) = await SudokuBoardGenerator.TryFindPossibleCell(board, 5);
        
        //ASSERT
        Assert.False(result);
        Assert.Equal((-1,-1), position);
    }
    
    [Fact]
    public async Task TryFindPossibleCell_PositionExistsButBadValue_ReturnFalse()
    {
        //ARRANGE
        var board = new int?[,] {{1, 2, 3}, {4, 5, null}, {7, 8, 9}};
        
        //ACT
        var (result, position) = await SudokuBoardGenerator.TryFindPossibleCell(board, 3);
        
        //ASSERT
        Assert.False(result);
        Assert.Equal((-1,-1), position);
    }
}