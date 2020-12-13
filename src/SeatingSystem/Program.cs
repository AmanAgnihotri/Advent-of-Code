// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.IO;
using System.Linq;

var layout = File.ReadLines("Input.txt")
  .Select(row => row
    .Select(tile => tile switch
    {
      '.' => Tile.Floor,
      'L' => Tile.Unoccupied,
      '#' => Tile.Occupied,
      _ => throw new NotImplementedException()
    })
    .ToArray())
  .ToArray();

var rows = layout.Length;
var columns = layout[0].Length;

var directions = new Direction[]
{
  new(-1, -1), new(+0, -1),
  new(+1, -1), new(-1, +0),
  new(+1, +0), new(-1, +1),
  new(+0, +1), new(+1, +1)
};

Console.WriteLine(GetStableOccupantsCount(1, 4));
Console.WriteLine(GetStableOccupantsCount(int.MaxValue, 5));

int GetStableOccupantsCount(int maxVisibility, int maxTolerance)
{
  return GetStableLayout(layout)
    .Sum(row => row.Count(tile => tile is Tile.Occupied));

  Tile[][] GetStableLayout(Tile[][] current)
  {
    var next = GetNextLayout(current);

    return AreEqual(next, current) ? current : GetStableLayout(next);
  }

  Tile[][] GetNextLayout(Tile[][] current)
  {
    return current
      .AsParallel()
      .Select((row, rowIndex) => row
        .Select((tile, tileIndex) =>
        {
          if (tile is Tile.Floor) return tile;

          var occupants = GetVisibleOccupantsCount(rowIndex, tileIndex);

          return tile switch
          {
            Tile.Unoccupied when occupants == 0 => Tile.Occupied,
            Tile.Occupied when occupants >= maxTolerance => Tile.Unoccupied,
            _ => tile
          };
        })
        .ToArray())
      .ToArray();

    int GetVisibleOccupantsCount(int rowIndex, int tileIndex) =>
      directions
        .Select(direction =>
        {
          var (rowDirection, tileDirection) = direction;

          return Enumerable
            .Range(1, maxVisibility)
            .Select(visibility => GetTile(
              rowIndex + rowDirection * visibility,
              tileIndex + tileDirection * visibility))
            .FirstOrDefault(tile => tile is not Tile.Floor);
        })
        .Count(tile => tile is Tile.Occupied);

    Tile GetTile(int row, int column) =>
      row < 0 || row >= rows || column < 0 || column >= columns
        ? Tile.Unknown
        : current[row][column];
  }

  static bool AreEqual(Tile[][] a, Tile[][] b) =>
    Enumerable.Range(0, a.Length).All(row => a[row].SequenceEqual(b[row]));
}

internal sealed record Direction(int Row, int Column);

internal enum Tile : byte { Unknown, Floor, Unoccupied, Occupied }
