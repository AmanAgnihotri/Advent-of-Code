// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Instructions =
  System.Collections.Immutable.IImmutableList<(string Operation, int Value)>;

var regex = new Regex(@"^(\w+) ([+-]\d+)$", RegexOptions.Compiled);

Instructions instructions = File.ReadLines("Input.txt")
  .Select(line => regex.Match(line).Groups)
  .Select(groups => (groups[1].Value, int.Parse(groups[2].Value)))
  .ToImmutableList();

Console.WriteLine(Execute(instructions).Accumulator);

Console.WriteLine(GetAlternatives(instructions)
  .Select(Execute)
  .FirstOrDefault(result => result.IsTerminated)
  .Accumulator);

static (int Accumulator, bool IsTerminated) Execute(Instructions instructions)
{
  var visitedSet = new HashSet<int>();
  var isTerminated = false;

  int Execute(int index, int acc)
  {
    if (index >= instructions.Count) isTerminated = true;

    if (isTerminated || visitedSet.Contains(index)) return acc;

    visitedSet.Add(index);

    var (operation, value) = instructions[index];

    return operation switch
    {
      "jmp" => Execute(index + value, acc),
      "nop" => Execute(index + 1, acc),
      "acc" => Execute(index + 1, acc + value),
      _ => throw new NotImplementedException()
    };
  }

  return (Execute(0, 0), isTerminated);
}

static IEnumerable<Instructions> GetAlternatives(Instructions instructions) =>
  Enumerable.Range(0, instructions.Count)
    .Where(index => instructions[index].Operation != "acc")
    .Select(index =>
    {
      var (operation, value) = instructions[index];

      return instructions.SetItem(index, (operation switch
      {
        "jmp" => "nop",
        "nop" => "jmp",
        _ => throw new NotImplementedException()
      }, value));
    });
