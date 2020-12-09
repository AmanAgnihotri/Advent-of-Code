// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using MoreLinq;

const int size = 25;

var numbers = File.ReadLines("Input.txt")
  .Select(long.Parse)
  .ToImmutableList();

var invalidNumber = numbers
  .Skip(size)
  .Where((number, index) =>
  {
    var set = numbers
      .Skip(index)
      .Take(size)
      .Where(value => value < number)
      .ToImmutableSortedSet();

    return !set.Any(value => set.Contains(number - value));
  })
  .First();

var sequence = Enumerable.Range(0, numbers.Count)
  .Select(index =>
  {
    var sums = numbers
      .Skip(index)
      .Scan((a, b) => a + b)
      .TakeWhile(sum => sum <= invalidNumber)
      .ToImmutableList();

    return sums[^1] == invalidNumber
      ? numbers
        .Skip(index)
        .Take(sums.Count)
        .OrderBy(value => value)
        .ToImmutableList()
      : ImmutableList<long>.Empty;
  })
  .First(values => values.Count > 0);

Console.WriteLine(invalidNumber);
Console.WriteLine(sequence[0] + sequence[^1]);
