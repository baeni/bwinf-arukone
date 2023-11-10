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

        internal const double MinNumbersMultiplier = .5;
        internal const double MaxNumbersMultiplier = 2;

        internal const double MoveCountMultiplier = 1;

        internal const int BoardGenerationFailThreshold = 750;
    }
}
