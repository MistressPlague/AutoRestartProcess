#if !AVTest
using System.Drawing;
using System.Globalization;

namespace DiscordWebhook
{
    internal static class Extensions
    {
        internal static int ToRgb(this Color color)
        {
            return int.Parse(ColorTranslator.ToHtml(Color.FromArgb(color.ToArgb())).Replace("#", ""), NumberStyles.HexNumber);
        }
    }
}
#endif