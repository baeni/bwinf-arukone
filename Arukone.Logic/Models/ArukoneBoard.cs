namespace Arukone.Logic.Models
{
    public class ArukoneBoard
    {
        internal ArukoneBoard(ArukoneBoardDefinition definition, int[,] boardArr)
        {
            Definition = definition;
            BoardArr = boardArr;
        }

        internal ArukoneBoardDefinition Definition { get; init; }
        internal int[,] BoardArr { get; init; }
    }
}
