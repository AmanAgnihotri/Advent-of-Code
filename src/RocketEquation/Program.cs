// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

var masses = File.ReadAllLines("Input.txt")
  .Select(int.Parse)
  .ToImmutableList();

static int GetFuel(int mass) => mass / 3 - 2;

static int GetTotalFuel(int mass)
{
  static int GetTotalFuel(int mass, int total)
  {
    var fuel = GetFuel(mass);

    return fuel switch
    {
      <= 0 => total,
      _ => GetTotalFuel(fuel, total + fuel)
    };
  }

  return GetTotalFuel(mass, 0);
}

Console.WriteLine(masses.Sum(GetFuel));
Console.WriteLine(masses.Sum(GetTotalFuel));
