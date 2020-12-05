// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace ReportRepair
{
  public static class Program
  {
    public static void Main()
    {
      var numbers = File.ReadAllLines("Input.txt")
        .Select(int.Parse)
        .ToImmutableHashSet();

      Print(GetProduct(numbers, 2020, 2));
      Print(GetProduct(numbers, 2020, 3));
    }

    private static void Print(int? value) =>
      Console.WriteLine(value is null ? "None" : value.ToString());

    private static int? GetProduct(
      IImmutableSet<int> numbers,
      int sum,
      int count,
      int product = 1)
    {
      if (count < 1 || sum < 1) return null;

      return count switch
      {
        1 when numbers.Contains(sum) => sum * product,
        1 => null,
        _ => numbers
          .Select(number => GetProduct(
            numbers.Except(new[] {number}),
            sum - number,
            count - 1,
            number * product))
          .FirstOrDefault(result => result.HasValue)
      };
    }
  }
}
