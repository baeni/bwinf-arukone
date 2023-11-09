using Arukone.Logic;
using Arukone.Logic.Models;

namespace Arukone.Input
{
    internal class Program
    {
        private const int BoardsToGenerate = 3;
        private const string OutputDirPath = "./output";

        static async Task Main(string[] args)
        {
            var n = ReadInput();

            Console.WriteLine("Spielfelder werden generiert... Dieser Vorgang kann gegebenenfalls einige Minuten dauern.");

            var definitions = new List<ArukoneBoardDefinition>();
            for (int i = 0; i < BoardsToGenerate; i++)
            {
                var definition = new ArukoneBoardDefinition(n);
                definitions.Add(definition);
            }
            var boards = await ArukoneHelper.GenerateManyBoardsAsync(definitions.ToArray());

            Console.Clear();
            Console.WriteLine($"""
                Es wurden {boards.Length} Spielfelder mit einer Größe von {n} erstellt.
                Die Ergebnisse kannst du unter {OutputDirPath} einsehen.
                """);

            for (int i = 0; i < boards.Length; i++)
            {
                var serializedBoard = ArukoneHelper.SerializeBoard(boards[i]);
                await SaveBoardToDiskAsync(serializedBoard, i);
                PrintBoard(serializedBoard);
            }
        }

        private static int ReadInput()
        {
            int n;
            do
            {
                Console.Clear();
                Console.Write($"Bestimme die Größe des Spielfeldes (>= {MagicNumbers.MinBoardSize}): ");
            }
            while (!int.TryParse(Console.ReadLine(), out n) || n < MagicNumbers.MinBoardSize);

            return n;
        }

        private static async Task SaveBoardToDiskAsync(string serializedBoard, int iteration = 1)
        {
            if (!Directory.Exists(OutputDirPath))
            {
                Directory.CreateDirectory(OutputDirPath);
            }

            var path = Path.Combine(OutputDirPath, $"board{iteration}.txt");
            await File.WriteAllTextAsync(path, serializedBoard);
        }

        private static void PrintBoard(string serializedBoard)
        {
            Console.WriteLine();
            Console.Write(serializedBoard);
            Console.WriteLine();
        }
    }
}