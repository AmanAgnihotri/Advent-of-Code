// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var regex = new Regex(
  @"^(\d+)-(\d+)\s{1}(\w):\s{1}(\w+)$",
  RegexOptions.Singleline | RegexOptions.Compiled,
  TimeSpan.FromSeconds(1));

var inputs = File.ReadLines("Input.txt")
  .Select(line => regex.Match(line))
  .Where(match => match.Success)
  .Select(match => (
    int.Parse(match.Groups[1].Value),
    int.Parse(match.Groups[2].Value),
    char.Parse(match.Groups[3].Value),
    match.Groups[4].Value))
  .ToImmutableList();

Console.WriteLine(inputs.Count(data =>
{
  var (min, max, key, password) = data;

  var count = password.Count(ch => ch == key);

  return count >= min && count <= max;
}));

Console.WriteLine(inputs.Count(data =>
{
  var (first, other, key, password) = data;

  return password[first - 1] == key ^
         password[other - 1] == key;
}));
