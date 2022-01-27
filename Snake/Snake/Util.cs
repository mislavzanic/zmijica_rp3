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

    public static class Util
    {
        public static T[,] ToRectangularArray<T>(this IReadOnlyList<T[]> arrays)
        {
            var ret = new T[arrays.Count, arrays[0].Length];
            for (var i = 0; i < arrays.Count; i++)
                for (var j = 0; j < arrays[0].Length; j++)
                    ret[i, j] = arrays[i][j];
            return ret;
        }
        public static int[,] loadMatrix(String filepath)
        {
            return File.ReadAllLines(filepath)   // read from File
                     .Select(x => x.Split(' ').Select(int.Parse).ToArray()) // split line into array of int
                     .ToArray()
                     .ToRectangularArray();
        }

        public static void FillRect(
            Graphics graphics, 
            Brush brush, 
            coordinates coords,
            float rW, 
            float rH,
            float relativeMargin
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

        public static void FillandoutlineRect(Graphics graphics, Brush brush, Pen pen, coordinates coords, float rW, float rH, float relativeMargin)
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
