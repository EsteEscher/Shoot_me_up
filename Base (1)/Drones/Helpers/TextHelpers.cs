using System.Drawing;

namespace Player.Helpers
{
    internal static class TextHelpers
    {
        public static Font drawFont = new Font("Arial", 10);
        public static Font bigFont = new Font("Arial", 28, FontStyle.Bold);
        public static Font mediumFont = new Font("Arial", 16, FontStyle.Bold);

        public static SolidBrush writingBrush = new SolidBrush(Color.Black);
        public static SolidBrush redBrush = new SolidBrush(Color.DarkRed);
        public static SolidBrush darkOverlayBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0));
        public static SolidBrush whiteBrush = new SolidBrush(Color.White);
    }
}