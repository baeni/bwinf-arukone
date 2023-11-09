namespace Arukone.Logic.Models
{
    public class ArukoneBoardDefinition
    {
        private static readonly Random _rnd = new();

        public ArukoneBoardDefinition(int size)
        {
            var numbersCount = _rnd.Next(
                (int) Math.Ceiling(size * MagicNumbers.MinNumbersMultiplier),
                (int) Math.Ceiling(size * MagicNumbers.MaxNumbersMultiplier));

            ThrowIfInvalidInput(size, numbersCount);

            Size = size;
            NumbersCount = numbersCount;
        }

        public int Size { get; init; }

        public int NumbersCount { get; set; }

        private void ThrowIfInvalidInput(int size, int numbersCount)
        {
            if (size < MagicNumbers.MinBoardSize)
            {
                throw new ArgumentOutOfRangeException(nameof(size), $"The size cannot be less than {MagicNumbers.MinBoardSize}.");
            }

            if (numbersCount < size * MagicNumbers.MinNumbersMultiplier || numbersCount > size * MagicNumbers.MaxNumbersMultiplier)
            {
                throw new ArgumentOutOfRangeException(nameof(numbersCount), $"The numbersCount cannot be smaller than size * {MagicNumbers.MinNumbersMultiplier} or greater than size * {MagicNumbers.MaxNumbersMultiplier}.");
            }
        }
    }
}