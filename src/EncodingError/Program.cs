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

var sums = numbers.Scan((a, b) => a + b).ToImmutableSortedSet();

var start = sums
  .Select(sum => sums.IndexOf(sum - invalidNumber))
  .FirstOrDefault(index => index >= 0);

var end = sums.IndexOf(sums[start] + invalidNumber);

var sequence = numbers
  .Skip(start)
  .Take(end - start + 1)
  .ToImmutableSortedSet();

Console.WriteLine(invalidNumber);
Console.WriteLine(sequence[0] + sequence[^1]);
