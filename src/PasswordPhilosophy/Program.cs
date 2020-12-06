// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PasswordPhilosophy
{
  public static class Program
  {
    public static void Main()
    {
      var inputs = GetInputs();

      PrintCount(inputs, HasValidOccurrences);
      PrintCount(inputs, HasValidPositioning);
    }

    private static void PrintCount(
      ImmutableList<Input> inputs,
      Func<Input, bool> predicate) =>
      Console.WriteLine(inputs.Count(predicate));

    private static bool HasValidOccurrences(Input input)
    {
      var (min, max, character, password) = input;

      var count = password.Count(c => c == character);

      return count >= min && count <= max;
    }

    private static bool HasValidPositioning(Input input)
    {
      var (initial, final, character, password) = input;

      return (password[initial - 1] == character) ^
             (password[final - 1] == character);
    }

    public sealed record Input(
      int Initial,
      int Final,
      char Character,
      string Password);

    private static ImmutableList<Input> GetInputs()
    {
      var regex = new Regex(
        @"^(\d+)-(\d+)\s{1}(\w):\s{1}(\w+)$",
        RegexOptions.Singleline | RegexOptions.Compiled,
        TimeSpan.FromSeconds(1));

      return File.ReadAllLines("Input.txt")
        .Select(line => regex.Match(line))
        .Where(match => match.Success)
        .Select(match => ToInput(match.Groups))
        .ToImmutableList();

      static Input ToInput(GroupCollection groups) => new(
        int.Parse(groups[1].Value),
        int.Parse(groups[2].Value),
        char.Parse(groups[3].Value),
        groups[4].Value);
    }
  }
}
