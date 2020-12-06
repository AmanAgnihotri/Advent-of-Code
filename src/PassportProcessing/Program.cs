// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Text.RegularExpressions.RegexOptions;

namespace PassportProcessing
{
  using Entries = ImmutableList<ImmutableDictionary<string, string>>;

  public static class Program
  {
    public static void Main()
    {
      var entries = GetEntries();

      var mandatoryKeys = new[]
        {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"}.ToImmutableHashSet();

      Console.WriteLine(entries
        .Count(entry => mandatoryKeys.IsSubsetOf(entry.Keys)));

      Console.WriteLine(entries
        .Where(entry => mandatoryKeys.IsSubsetOf(entry.Keys))
        .Count(pairs => pairs.All(IsStrictlyValid)));
    }

    private static bool IsStrictlyValid(KeyValuePair<string, string> pair)
    {
      var (key, value) = pair;

      return key switch
      {
        "byr" => IsValidBirthYear(),
        "iyr" => IsValidIssueYear(),
        "eyr" => IsValidExpirationYear(),
        "hgt" => IsValidHeight(),
        "hcl" => IsValidHairColor(),
        "ecl" => IsValidEyeColor(),
        "pid" => IsValidPassportId(),
        "cid" => true,
        _ => false
      };

      bool IsValidBirthYear() =>
        int.TryParse(value, out var year) &&
        year >= 1920 && year <= 2002;

      bool IsValidIssueYear() =>
        int.TryParse(value, out var year) &&
        year >= 2010 && year <= 2020;

      bool IsValidExpirationYear() =>
        int.TryParse(value, out var year) &&
        year >= 2020 && year <= 2030;

      bool IsValidHeight()
      {
        if (!int.TryParse(value.Substring(0, value.Length - 2), out var height))
          return false;

        if (value.EndsWith("cm")) return height >= 150 && height <= 193;

        if (value.EndsWith("in")) return height >= 59 && height <= 76;

        return false;
      }

      bool IsValidHairColor() =>
        Regex.IsMatch(value, @"^#[0-9a-f]{6}$", Compiled);

      bool IsValidEyeColor() => ValidEyeColors.Contains(value);

      bool IsValidPassportId() =>
        Regex.IsMatch(value, @"^\d{9}$", Compiled);
    }

    private static readonly IImmutableSet<string> ValidEyeColors = new[]
      {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"}.ToImmutableHashSet();

    private static Entries GetEntries() =>
      Regex.Split(File.ReadAllText("Input.txt"), @"\s{2}", Compiled)
        .Select(entry => Regex.Split(entry, @"\s+", Compiled)
          .Where(value => !string.IsNullOrEmpty(value))
          .Select(value =>
          {
            var pair = value.Split(":");

            return new KeyValuePair<string, string>(pair[0], pair[1]);
          })
          .ToImmutableDictionary())
        .ToImmutableList();
  }
}
