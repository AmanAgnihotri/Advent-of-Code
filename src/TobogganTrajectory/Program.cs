// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace TobogganTrajectory
{
  public static class Program
  {
    public static void Main()
    {
      var map = GetMap();

      Console.WriteLine(GetTreeCount(map, 3, 1));

      Console.WriteLine(new[] {(1, 1), (3, 1), (5, 1), (7, 1), (1, 2)}
        .Select(pair => GetTreeCount(map, pair.Item1, pair.Item2))
        .Select(value => (long) value)
        .Aggregate((a, b) => a * b));
    }

    private static int GetTreeCount(ImmutableList<BitArray> map, int dx, int dy)
    {
      var xc = map.First().Count;
      var yc = map.Count / dy + map.Count % dy - 1;

      var xs = Enumerable.Range(1, yc).Select(x => x * dx % xc);
      var ys = Enumerable.Range(1, yc).Select(y => y * dy);

      return xs.Zip(ys, (x, y) => map[y][x]).Count(v => v);
    }

    private static ImmutableList<BitArray> GetMap()
    {
      const char tree = '#';

      return File.ReadAllLines("Input.txt")
        .Select(AsBitArray)
        .ToImmutableList();

      static BitArray AsBitArray(string line) =>
        new(line.Select(ch => ch == tree).ToArray());
    }
  }
}
