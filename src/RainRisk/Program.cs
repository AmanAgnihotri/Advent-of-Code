// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var instructions = File.ReadLines("Input.txt")
  .Select(line => Regex.Match(line, @"(\w)(\d+)", RegexOptions.Compiled).Groups)
  .Select(groups =>
    new Instruction(char.Parse(groups[1].Value), int.Parse(groups[2].Value)))
  .ToImmutableList();

var directedShip = new DirectedShip(0, 0, 0);
var waypointShip = new WaypointShip(0, 0, new Waypoint(10, 1));

instructions.ForEach(instruction =>
{
  directedShip = directedShip.Apply(instruction);
  waypointShip = waypointShip.Apply(instruction);
});

Console.WriteLine(directedShip.GetManhattanDistance());
Console.WriteLine(waypointShip.GetManhattanDistance());

internal abstract record Ship(int X, int Y)
{
  public int GetManhattanDistance() => Math.Abs(X) + Math.Abs(Y);
}

internal record DirectedShip(int X, int Y, int Direction) : Ship(X, Y)
{
  public DirectedShip Apply(Instruction instruction) => instruction.Type switch
  {
    'F' => Apply(instruction with {Type = DirectionMap[Direction]}),
    'E' => this with {X = X + instruction.Value},
    'N' => this with {Y = Y + instruction.Value},
    'W' => this with {X = X - instruction.Value},
    'S' => this with {Y = Y - instruction.Value},
    'L' => this with {Direction = (Direction + instruction.Value) % 360},
    'R' => this with {Direction = (360 + Direction - instruction.Value) % 360},
    _ => throw new NotImplementedException()
  };

  private static readonly IImmutableDictionary<int, char> DirectionMap =
    ImmutableSortedDictionary<int, char>.Empty
      .Add(0, 'E').Add(90, 'N').Add(180, 'W').Add(270, 'S');
}

internal record WaypointShip(int X, int Y, Waypoint Waypoint) : Ship(X, Y)
{
  public WaypointShip Apply(Instruction instruction) => instruction.Type switch
  {
    'F' => this with {
      X = X + Waypoint.X * instruction.Value,
      Y = Y + Waypoint.Y * instruction.Value},
    _ => this with {Waypoint = Waypoint.Apply(instruction)}
  };
}

internal record Waypoint(int X, int Y)
{
  public Waypoint Apply(Instruction instruction) => instruction.Type switch
  {
    'E' => this with {X = X + instruction.Value},
    'N' => this with {Y = Y + instruction.Value},
    'W' => this with {X = X - instruction.Value},
    'S' => this with {Y = Y - instruction.Value},
    'L' => Rotate(instruction.Value),
    'R' => Rotate(360 - instruction.Value),
    _ => throw new NotImplementedException()
  };

  private Waypoint Rotate(int degrees) => degrees switch
  {
    90 => this with {X = -Y, Y = X},
    180 => this with {X = -X, Y = -Y},
    270 => this with {X = Y, Y = -X},
    _ => this
  };
}

internal sealed record Instruction(char Type, int Value);
