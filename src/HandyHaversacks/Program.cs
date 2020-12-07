// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Text.RegularExpressions.RegexOptions;
using Bags = System.Collections.Immutable.ImmutableDictionary<string, int>;

const string myBag = "shiny gold";

var rules = File.ReadLines("Input.txt")
  .ToImmutableDictionary(
    line => Regex.Match(line, @"^(\w+ \w+)", Compiled).Groups[1].Value,
    line => line.Contains("no other bags.")
      ? Bags.Empty
      : Regex.Matches(line, @"(\d+) (\w+ \w+) bags?[,.]\s?", Compiled)
        .ToImmutableDictionary(
          match => match.Groups[2].Value,
          match => int.Parse(match.Groups[1].Value)));

Console.WriteLine(GetContainerBagsCount(myBag));
Console.WriteLine(GetContainedBagsCount(myBag));

int GetContainerBagsCount(string bag)
{
  bool IsBagContainedIn(Bags bags) =>
    bags.ContainsKey(bag) ||
    bags.Keys.Any(b => IsBagContainedIn(rules[b]));

  return rules.Values.Count(IsBagContainedIn);
}

int GetContainedBagsCount(string bag)
{
  int GetCount(Bags bags) => bags.Count == 0
    ? 1
    : 1 + bags.Sum(b => b.Value * GetCount(rules[b.Key]));

  return GetCount(rules[bag]) - 1;
}
