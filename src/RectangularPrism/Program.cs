// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Dimensions = System.Collections.Immutable.ImmutableArray<int>;

var data = File.ReadLines("Input.txt")
  .Select(line => Regex.Match(line, @"(\d+)x(\d+)x(\d+)", RegexOptions.Compiled)
    .Groups
    .Values
    .Skip(1)
    .Select(group => int.Parse(group.Value))
    .OrderBy(value => value)
    .ToImmutableArray())
  .ToImmutableList();

Console.WriteLine(data.Sum(GetWrapArea));
Console.WriteLine(data.Sum(GetRibbonLength));

static int GetWrapArea(Dimensions ds) =>
  2 * ds[0] * ds[1] + 2 * ds[1] * ds[2] + 2 * ds[0] * ds[2] + ds[0] * ds[1];

static int GetRibbonLength(Dimensions ds) =>
  2 * ds[0] + 2 * ds[1] + ds[0] * ds[1] * ds[2];
