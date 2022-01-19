using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Snake
{
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
    }
}
