namespace RollicCase.Gameplay.View.Hud
{
    /// <summary>Writes a countdown as minutes and seconds ("m:ss") into a reused buffer, so updating the timer allocates nothing.</summary>
    public static class ClockText
    {
        /// <summary>Buffer size that fits any countdown the timer limits allow.</summary>
        public const int BufferLength = 8;

        private const int SecondsPerMinute = 60;
        private const int Base = 10;
        private const char Separator = ':';

        /// <summary>Writes the seconds as "m:ss" and returns the number of characters written.</summary>
        public static int Write(int totalSeconds, char[] buffer)
        {
            int minutes = totalSeconds / SecondsPerMinute;
            int seconds = totalSeconds % SecondsPerMinute;
            int length = WriteNumber(minutes, buffer);

            buffer[length++] = Separator;
            buffer[length++] = ToDigit(seconds / Base);
            buffer[length++] = ToDigit(seconds % Base);
            return length;
        }

        private static int WriteNumber(int value, char[] buffer)
        {
            int digits = 1;

            for (int rest = value / Base; rest > 0; rest /= Base)
            {
                digits++;
            }

            for (int i = digits - 1; i >= 0; i--)
            {
                buffer[i] = ToDigit(value % Base);
                value /= Base;
            }

            return digits;
        }

        private static char ToDigit(int value)
        {
            return (char)('0' + value);
        }
    }
}
