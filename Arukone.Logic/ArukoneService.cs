using Arukone.Logic.Enums;
using Arukone.Logic.Models;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Arukone.Logic
{
    public class ArukoneService
    {
        private readonly ArukoneMove[] _arukoneMoveEnums = (ArukoneMove[]) Enum.GetValues(typeof(ArukoneMove));
        private readonly Random _rnd = new();

        private readonly ILogger<ArukoneService>? _logger;

        public ArukoneService(ILogger<ArukoneService>? logger = null)
        {
            _logger = logger;
        }

        public async Task<ArukoneBoard> GenerateBoardAsync(ArukoneBoardDefinition definition)
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

                    board = TryGenerateRandomBoard(definition);

                    if (board is null)
                    {
                        failedTries++;
                    }
                }
            });

            return board!;
        }

        public async Task<ArukoneBoard[]> GenerateManyBoardsAsync(params ArukoneBoardDefinition[] definitions)
        {
            var definitionsCount = definitions.Length;
            var boardsToGenerate = new List<Task<ArukoneBoard>>();

            for (int i = 0; i < definitionsCount; i++)
            {
                var board = GenerateBoardAsync(definitions[i]);
                boardsToGenerate.Add(board);
            }

            var boards = await Task.WhenAll(boardsToGenerate);

            return boards.ToArray();
        }

        private ArukoneBoard? TryGenerateRandomBoard(ArukoneBoardDefinition definition)
        {
            var size = definition.Size;
            var numbersCount = definition.NumbersCount;

            var filledBoardArr = new int[size, size];
            var boardArr = new int[size, size];

            for (int i = 1; i <= numbersCount; i++)
            {
                Tuple<int, int>? startPosition = TryGetValidStartPosition(definition, filledBoardArr);
                if (startPosition is null)
                {
                    _logger?.LogError("Board is already completely filled.");
                    return null;
                }

                var x = startPosition.Item1;
                var y = startPosition.Item2;

                filledBoardArr[y, x] = i;
                boardArr[y, x] = i;

                var moveCount = Math.Ceiling(size * MagicNumbers.MoveCountMultiplier);

                for (int j = 1; j <= moveCount; j++)
                {
                    var shuffledMoves = _arukoneMoveEnums.OrderBy(_ => _rnd.Next()).ToArray();
                    var enumerator = shuffledMoves.GetEnumerator();

                    var moveSuccessful = false;

                    do
                    {
                        if (!enumerator.MoveNext())
                        {
                            // TODO: This happens too often
                            _logger?.LogError("No more possible moves.");
                            return null;
                        }

                        var potentialMove = enumerator.Current;
                        var upcommingX = x;
                        var upcommingY = y;

                        _logger?.LogDebug($"Checking if {i} can move {potentialMove} in {j}th step..");

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
                            _logger?.LogInformation($"{i} moves {potentialMove} in {j}th step.");

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
            }

            return new ArukoneBoard(definition, boardArr);
        }

        private Tuple<int, int>? TryGetValidStartPosition(ArukoneBoardDefinition definition, int[,] filledBoardArr)
        {
            if (!filledBoardArr.Cast<int>().Any(x => x is 0))
            {
                return null;
            }

            var size = definition.Size;

            int x;
            int y;

            do
            {
                _logger?.LogInformation("Evaluation a start position.");

                x = _rnd.Next(size);
                y = _rnd.Next(size);
            }
            while (filledBoardArr[y, x] != 0);

            return new Tuple<int, int>(x, y);
        }

        private bool IsBoardValid(ArukoneBoard board)
        {
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

        public string SerializeBoard(ArukoneBoard board)
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
    }
}
