using Arukone.Logic;
using Arukone.Logic.Models;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics;

namespace Arukone.Input
{
    internal class Program
    {
        private const int BoardsToGenerate = 3;

        private const string OutputDirPath = "./output";
        private const string OutputFileName = "board{0}.txt";

        private const string LogsDirPath = "./logs";
        private const string LogsFileName = "latest.log";

        private static ArukoneService _arukoneService = default!;
        private static ILogger<ArukoneService> _logger = default!;

        static async Task Main(string[] args)
        {
            ConfigureLogger();
            _arukoneService = new ArukoneService(_logger);

            var boardSize = ReadInput();

            AnnounceStart(boardSize);

            #region Generating multiple ArukoneBoards in specified size.
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var definitions = new List<ArukoneBoardDefinition>();
            for (int i = 0; i < BoardsToGenerate; i++)
            {
                var definition = new ArukoneBoardDefinition(boardSize);
                definitions.Add(definition);
            }
            var boards = await _arukoneService.GenerateManyBoardsAsync(definitions.ToArray());

            stopwatch.Stop();
            #endregion

            #region Outputting results on disk.
            for (int i = 0; i < boards.Length; i++)
            {
                var serializedBoard = _arukoneService.SerializeBoard(boards[i]);
                await SaveBoardToDiskAsync(serializedBoard, i);
            }
            #endregion

            AnnounceEnd(boards.Length, boardSize, stopwatch.ElapsedMilliseconds);
        }

        private static int ReadInput()
        {
            int n;
            do
            {
                Console.Clear();
                Console.Write(string.Format(Messages.SpecifyBoardSize, MagicNumbers.MinBoardSize));
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

            var path = Path.Combine(OutputDirPath, string.Format(OutputFileName, iteration));
            await File.WriteAllTextAsync(path, serializedBoard);
        }

        private static void AnnounceStart(int boardSize)
        {
            Console.Clear();
            Console.WriteLine(string.Format(Messages.StartedBoardGeneration, BoardsToGenerate, boardSize));
        }

        private static void AnnounceEnd(int boardsAmount, int boardSize, long timeElapsed)
        {
            var fullOutputpath = Path.GetFullPath(OutputDirPath);
            var fullLogsPath = Path.GetFullPath(LogsDirPath);

            Console.Clear();
            Console.Write(string.Format(Messages.FinishedBoardGeneration, boardsAmount, boardSize, timeElapsed / 1000.0, fullOutputpath, fullLogsPath));

            OpenFileExplorer(fullOutputpath);
            Console.ReadKey();
        }

        private static void OpenFileExplorer(string path)
        {
            try
            {
                Process.Start("explorer.exe", path);
            }
            catch (Exception)
            {
                Console.WriteLine(Messages.ErrorOpeningExplorer);
            }
        }

        private static void ConfigureLogger()
        {
            if (!Directory.Exists(LogsDirPath))
            {
                Directory.CreateDirectory(LogsDirPath);
            }

            var logFile = Path.Combine(LogsDirPath, LogsFileName);

            var seriLog = new LoggerConfiguration()
                .WriteTo.File(logFile)
                .MinimumLevel.Information()
                .CreateLogger();

            var loggerFactory = new LoggerFactory().AddSerilog(seriLog);
            _logger = loggerFactory.CreateLogger<ArukoneService>();
        }
    }
}