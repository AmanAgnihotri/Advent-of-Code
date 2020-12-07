// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using MoreLinq;

var steps = File.ReadLines("Input.txt")
  .SelectMany(line => line.Select(ch => ch switch
  {
    '(' => +1,
    ')' => -1,
    _ => throw new NotImplementedException()
  }))
  .ToImmutableList();

Console.WriteLine(steps.Sum());

Console.WriteLine(steps.Scan((a, b) => a + b)
  .TakeUntil(floor => floor == -1)
  .Count());
