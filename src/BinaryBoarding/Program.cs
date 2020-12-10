// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

var seatIds = File.ReadLines("Input.txt")
  .Select(GetSeatId)
  .ToImmutableSortedSet();

Console.WriteLine(seatIds.Max());

Console.WriteLine(Enumerable
  .Range(seatIds.Min(), seatIds.Count + 1)
  .SingleOrDefault(id => !seatIds.Contains(id)));

static int GetSeatId(string pass)
{
  var binary = new StringBuilder(pass)
    .Replace('F', '0')
    .Replace('B', '1')
    .Replace('L', '0')
    .Replace('R', '1')
    .ToString();

  return Convert.ToInt32(binary, 2);
}
