// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

const char tree = '#';

var map = File.ReadAllLines("Input.txt")
  .Select(line => new BitArray(line.Select(ch => ch == tree).ToArray()))
  .ToImmutableList();

Console.WriteLine(GetTreeCount(3, 1));

Console.WriteLine(new[] {(1, 1), (3, 1), (5, 1), (7, 1), (1, 2)}
  .Select(pair => GetTreeCount(pair.Item1, pair.Item2))
  .Select(Convert.ToInt64)
  .Aggregate((a, b) => a * b));

int GetTreeCount(int dx, int dy)
{
  var xc = map.First().Count;
  var yc = map.Count / dy + map.Count % dy - 1;

  var xs = Enumerable.Range(1, yc).Select(x => x * dx % xc);
  var ys = Enumerable.Range(1, yc).Select(y => y * dy);

  return xs.Zip(ys, (x, y) => map[y][x]).Count(v => v);
}
