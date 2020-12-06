// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

var numbers = File.ReadAllLines("Input.txt")
  .Select(int.Parse)
  .ToImmutableHashSet();

Print(GetProduct(2020, 2));
Print(GetProduct(2020, 3));

int? GetProduct(int sum, int count, int product = 1) => count switch
{
  < 1 => null,
  1 when numbers.Contains(sum) => sum * product,
  1 => null,
  _ when sum < 1 => null,
  _ => numbers
    .Select(number => GetProduct(sum - number, count - 1, number * product))
    .FirstOrDefault(result => result.HasValue)
};

static void Print(int? value) =>
  Console.WriteLine(value is null ? "None" : value.ToString());
