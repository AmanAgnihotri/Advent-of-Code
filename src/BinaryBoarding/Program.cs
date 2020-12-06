// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

var regex = new Regex(@"^([FB]{7})([LR]{3})$", RegexOptions.Compiled);

var seatIds = File.ReadAllLines("Input.txt")
  .Select(GetSeatId)
  .ToImmutableSortedSet();

Console.WriteLine(seatIds.Max());

Console.WriteLine(Enumerable
  .Range(seatIds.Min(), seatIds.Count + 1)
  .SingleOrDefault(id => !seatIds.Contains(id)));

int GetSeatId(string pass)
{
  var groups = regex.Match(pass).Groups;

  var rowInBinary = new StringBuilder(groups[1].Value)
    .Replace('F', '0')
    .Replace('B', '1')
    .ToString();

  var columnInBinary = new StringBuilder(groups[2].Value)
    .Replace('L', '0')
    .Replace('R', '1')
    .ToString();

  var row = Convert.ToInt32(rowInBinary, 2);
  var column = Convert.ToInt32(columnInBinary, 2);

  return row * 8 + column;
}
