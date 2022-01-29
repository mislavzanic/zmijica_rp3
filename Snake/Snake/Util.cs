using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Snake
{
    using coordinates = Tuple<int, int>;

    internal static class Util
    {
        public static T[,] ToRectangularArray<T>(this IReadOnlyList<T[]> arrays)
        {
            var ret = new T[arrays.Count, arrays[0].Length];
            for (var i = 0; i < arrays.Count; i++)
                for (var j = 0; j < arrays[0].Length; j++)
                    ret[i, j] = arrays[i][j];
            return ret;
        }
        public static List<List<ItemType>> LoadMatrix(string filepath) =>
            File.ReadAllLines(filepath)   // read from File
                     .Select(x => x.Split(' ')
                                   .Select(int.Parse)
                                   .Select(xx => (ItemType)xx)
                                   .ToList()
                      ) // split line into array of ItemType
                     .ToList();

        public static bool In<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }

        public static void FillRect(
            Graphics graphics, 
            Brush brush, 
            coordinates coords,
            float rW, 
            float rH,
            float relativeMargin = .1f
        )
        {
            graphics.FillRectangle(
                brush,
                (coords.Item1 + relativeMargin / 2) * rW,
                (coords.Item2 + relativeMargin / 2) * rH,
                rW * ( 1 - relativeMargin),
                rH * ( 1 - relativeMargin)
                );
        }

        public static void FillAndOutlineRect(Graphics graphics, Brush brush, Pen pen, coordinates coords, float rW, float rH, float relativeMargin=.1f)
        {
            FillRect(graphics, brush, coords, rW, rH, relativeMargin);

            graphics.DrawRectangle(
                pen,
                (coords.Item1 + relativeMargin / 2) * rW,
                (coords.Item2 + relativeMargin / 2) * rH,
                rW * (1 - relativeMargin),
                rH * (1 - relativeMargin)
            );
        }
    }
}
