using Arukone.Logic.Enums;
using Arukone.Logic.Exceptions;
using Arukone.Logic.Models;
using System.Text;

namespace Arukone.Logic
{
    public static class ArukoneHelper
    {
        private static readonly ArukoneMove[] _arukoneMoveEnums = (ArukoneMove[]) Enum.GetValues(typeof(ArukoneMove));
        private static readonly Random _rnd = new();

        public static async Task<ArukoneBoard> GenerateBoardAsync(ArukoneBoardDefinition definition)
        {
            ArukoneBoard? board = null;

            await Task.Run(() =>
            {
                var failedTries = 0;

                while (board is null || !IsBoardValid(board))
                {
                    if (failedTries >= MagicNumbers.BoardGenerationFailThreshold && definition.NumbersCount > definition.Size / 2)
                    {
                        definition.NumbersCount--;
                        failedTries = 0;
                    }

                    try
                    {
                        board = TryGenerateRandomBoard(definition);
                    }
                    catch (Exception ex) when (ex is IndexOutOfRangeException || ex is BoardFilledException)
                    {
                        failedTries++;
                        //Console.WriteLine(ex.Message);
                    }
                }
            });

            return board!;
        }

        public static async Task<ArukoneBoard[]> GenerateManyBoardsAsync(params ArukoneBoardDefinition[] definitions)
        {
            var definitionsCount = definitions.Length;
            var boards = new List<ArukoneBoard>();

            for (int i = 0; i < definitionsCount; i++)
            {
                var board = await GenerateBoardAsync(definitions[i]);
                boards.Add(board);

                Console.WriteLine($"{i + 1} von {definitionsCount} Spielfelder generiert.");
            }

            return boards.ToArray();
        }

        public static string SerializeBoard(ArukoneBoard board)
        {
            var builder = new StringBuilder();

            var size = board.Definition.Size;
            var numbersCount = board.Definition.NumbersCount;

            builder.AppendLine(size.ToString());
            builder.AppendLine(numbersCount.ToString());

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var number = board.BoardArr[y, x];
                    builder.Append(number);

                    if (x != size - 1)
                    {
                        builder.Append(' ');
                    }
                }

                if (y != size - 1)
                {
                    builder.AppendLine();
                }
            }

            return builder.ToString();
        }

        private static ArukoneBoard TryGenerateRandomBoard(ArukoneBoardDefinition definition)
        {
            var size = definition.Size;
            var numbersCount = definition.NumbersCount;

            var filledBoardArr = new int[size, size];
            var boardArr = new int[size, size];

            for (int i = 1; i <= numbersCount; i++)
            {
                //Console.WriteLine("Started new path.");

                Tuple<int, int> startPosition = TryGetValidStartPosition(definition, filledBoardArr);
                var x = startPosition.Item1;
                var y = startPosition.Item2;

                filledBoardArr[y, x] = i;
                boardArr[y, x] = i;

                var moveCount = _rnd.Next(
                    (int) Math.Ceiling(size * MagicNumbers.MinMoveCountMultiplier),
                    (int) Math.Ceiling(size * MagicNumbers.MaxMoveCountMultiplier));

                for (int j = 1; j <= moveCount; j++)
                {
                    var shuffledMoves = _arukoneMoveEnums.OrderBy(_ => _rnd.Next()).ToArray();
                    var enumerator = shuffledMoves.GetEnumerator();

                    var moveSuccessful = false;

                    do
                    {
                        if (!enumerator.MoveNext())
                        {
                            throw new IndexOutOfRangeException("No potential moves left.");
                        }

                        var potentialMove = enumerator.Current;
                        var upcommingX = x;
                        var upcommingY = y;

                        //Console.WriteLine($"Checking if {i} can move {potentialMove} in {j}th step..");

                        switch (potentialMove)
                        {
                            case ArukoneMove.Forward:
                                upcommingX++;
                                break;
                            case ArukoneMove.Backward:
                                upcommingX--;
                                break;
                            case ArukoneMove.Down:
                                upcommingY++;
                                break;
                            case ArukoneMove.Up:
                                upcommingY--;
                                break;
                        }

                        if (upcommingX >= 0 && upcommingX < size && upcommingY >= 0 && upcommingY < size && filledBoardArr[upcommingY, upcommingX] is 0)
                        {
                            //Console.WriteLine($"{i} can indeed move {potentialMove} in {j}th step.");

                            filledBoardArr[upcommingY, upcommingX] = i;
                            x = upcommingX;
                            y = upcommingY;

                            if (j == moveCount)
                            {
                                boardArr[y, x] = i;
                            }

                            moveSuccessful = true;
                        }
                    }
                    while (!moveSuccessful);
                }

                //Console.WriteLine("Path for number done.");
            }

            //Console.WriteLine("Board generation done.");

            return new ArukoneBoard(definition, boardArr);
        }

        private static Tuple<int, int> TryGetValidStartPosition(ArukoneBoardDefinition definition, int[,] filledBoardArr)
        {
            if (!filledBoardArr.Cast<int>().Any(x => x == 0))
            {
                throw new BoardFilledException();
            }

            var size = definition.Size;

            int x;
            int y;

            do
            {
                //Console.WriteLine("Evaluating start position.");

                x = _rnd.Next(size);
                y = _rnd.Next(size);
            }
            while (filledBoardArr[y, x] != 0);

            return new Tuple<int, int>(x, y);
        }

        private static bool IsBoardValid(ArukoneBoard? board)
        {
            if (board is null)
            {
                return false;
            }

            var numbers = board.BoardArr.Cast<int>();
            for (int i = 1; i <= board.Definition.NumbersCount; i++)
            {
                var count = numbers.Count(x => x == i);

                if (count != 2)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
