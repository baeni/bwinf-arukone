namespace Arukone.Logic
{
    public static class MagicNumbers
    {
        /*
         * All multipliers are processed with the following format:
         *
         *      boardSize * multiplier
         *
         * E.g. with a boardSize of 6 and a MaxNumbersMultiplier of .9,
         * there can be a maximum of 5.4 numbers on the board (rounded off to 5).
         */

        public const int MinBoardSize = 4;

        public const double MinNumbersMultiplier = .5;
        public const double MaxNumbersMultiplier = 2;

        public const double MinMoveCountMultiplier = 1;
        public const double MaxMoveCountMultiplier = 4;

        public const int BoardGenerationFailThreshold = 750;
    }
}
