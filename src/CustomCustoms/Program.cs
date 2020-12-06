// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Set = System.Collections.Immutable.ImmutableHashSet<char>;

var groups = Regex
  .Split(File.ReadAllText("Input.txt"), @"\s{2}", RegexOptions.Compiled)
  .Select(entry => entry.Split("\n", StringSplitOptions.RemoveEmptyEntries)
    .Select(v => v.ToImmutableHashSet())
    .ToImmutableList())
  .ToImmutableList();

void PrintAggregatedSum(Func<Set, Set, Set> reduce) => Console.WriteLine(
  groups.Sum(group => group.Aggregate(group.First(), reduce).Count));

PrintAggregatedSum((a, b) => a.Union(b));
PrintAggregatedSum((a, b) => a.Intersect(b));
