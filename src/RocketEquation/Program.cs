// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

var masses = File.ReadLines("Input.txt")
  .Select(int.Parse)
  .ToImmutableList();

Console.WriteLine(masses.Sum(GetFuel));
Console.WriteLine(masses.Sum(GetTotalFuel));

static int GetFuel(int mass) => mass / 3 - 2;

static int GetTotalFuel(int mass)
{
  static int Loop(int mass, int total)
  {
    var fuel = GetFuel(mass);

    return fuel switch
    {
      <= 0 => total,
      _ => Loop(fuel, total + fuel)
    };
  }

  return Loop(mass, 0);
}
