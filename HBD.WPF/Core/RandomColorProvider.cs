#region

using System;
using System.Windows.Media;

#endregion

namespace HBD.WPF.Core
{
    public class RandomColorProvider
    {
        private static readonly Color[] DefaultColors =
        {
            Colors.Blue, Colors.BlueViolet,
            Colors.Brown, Colors.BurlyWood,
            Colors.CadetBlue, Colors.Chocolate,
            Colors.Coral, Colors.CornflowerBlue,
            Colors.Crimson, Colors.DarkBlue,
            Colors.DarkCyan, Colors.DarkGoldenrod,
            Colors.DarkGreen, Colors.DarkKhaki,
            Colors.DarkMagenta, Colors.DarkOliveGreen,
            Colors.DarkOrange, Colors.DarkOrchid,
            Colors.DarkRed, Colors.DarkSeaGreen,
            Colors.DarkSlateGray, Colors.DarkTurquoise,
            Colors.DarkViolet, Colors.DeepPink,
            Colors.DeepSkyBlue, Colors.DimGray,
            Colors.DodgerBlue, Colors.Firebrick,
            Colors.ForestGreen, Colors.Fuchsia,
            Colors.Goldenrod, Colors.Green,
            Colors.HotPink, Colors.IndianRed,
            Colors.Indigo, Colors.Orange,
            Colors.OrangeRed, Colors.Purple,
            Colors.Red, Colors.RosyBrown,
            Colors.RoyalBlue, Colors.SaddleBrown,
            Colors.Salmon, Colors.SandyBrown,
            Colors.Tomato, Colors.Teal
        };

        private readonly Random _random = new Random();

        private int _currentIndex;

        /// <summary>
        ///     Next Random Color.
        /// </summary>
        /// <returns></returns>
        public Color Next()
        {
            var index = _random.Next(0, DefaultColors.Length);
            return DefaultColors[index];
        }

        /// <summary>
        ///     Get Next color by Sequence.
        /// </summary>
        /// <returns></returns>
        public Color NextSequence()
        {
            var c = DefaultColors[_currentIndex++];
            if (_currentIndex == DefaultColors.Length)
                _currentIndex = 0;

            return c;
        }
    }
}