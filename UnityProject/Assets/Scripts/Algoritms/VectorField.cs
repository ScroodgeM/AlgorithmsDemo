﻿//this empty line for UTF-8 BOM header
using System;
using System.Collections;
using System.Collections.Generic;
using AlgorithmsDemo.DTS;
using AlgorithmsDemo.World;
using UnityEngine;

namespace AlgorithmsDemo.Algoritms
{
    public class VectorField
    {
        private struct Cell
        {
            public readonly bool isReady;
            public readonly int distanceToTarget;
            public readonly Vector2Int directionToTarget;

            public Cell(bool isReady, int distanceToTarget, Vector2Int directionToTarget)
            {
                this.isReady = isReady;
                this.distanceToTarget = distanceToTarget;
                this.directionToTarget = directionToTarget;
            }

            public static Cell Default => new Cell(false, int.MaxValue, Vector2Int.zero);

            public override string ToString() => $"(isReady={isReady}, distanceToTarget={distanceToTarget} directionToTarget={directionToTarget})";
        }

        private readonly WorldForPathBuilder world;
        private readonly ArrayXY<Cell> cells;

        private const int oneAxisStep = 2;
        private const int twoAxisStep = 3;
        private const int maxCycles = 1000;

        public VectorField(WorldForPathBuilder world)
        {
            this.world = world;
            cells = new ArrayXY<Cell>(world.GetWorldSize(), Cell.Default, pos => Cell.Default);
        }

        public void BuildField(Vector2Int target)
        {
            cells.ResetToValue(Cell.Default);

            RectAreaInt workArea = world.GetWorldSize();

            cells[target] = new Cell(true, 0, Vector2Int.zero);

            int stuckDefense = 0;
            while (true)
            {
                bool someSolutionFound = false;

                int xMin = workArea.xMin;
                int xMax = workArea.xMax;
                int yMin = workArea.yMin;
                int yMax = workArea.yMax;

                for (int x = xMin; x <= xMax; x++)
                {
                    for (int y = yMin; y <= yMax; y++)
                    {
                        Vector2Int pos = new Vector2Int(x, y);
                        if (cells[pos].isReady == false)
                        {
                            cells[pos] = TrySolve(pos);
                            someSolutionFound |= cells[pos].isReady;
                        }
                    }
                }

                if (stuckDefense++ > maxCycles)
                {
                    throw new InvalidOperationException($"PATH ERROR: stuck in cycle more than {maxCycles} times, no path to {target}");
                }

                if (someSolutionFound == false)
                {
                    break;
                }
            }
        }

        public Vector2Int GetDirectionToTarget(Vector2Int position)
        {
            return cells[position].directionToTarget;
        }

        private Cell TrySolve(Vector2Int solvePosition)
        {
            if (world.IsCellWalkable(solvePosition) == false)
            {
                return Cell.Default;
            }

            Cell solution = Cell.Default;

            for (int x = solvePosition.x - 1; x <= solvePosition.x + 1; x++)
            {
                for (int y = solvePosition.y - 1; y <= solvePosition.y + 1; y++)
                {
                    if (x == solvePosition.x && y == solvePosition.y)
                    {
                        // it's me
                        continue;
                    }

                    Vector2Int neighbourPosition = new Vector2Int(x, y);

                    if (IsOnGrid(neighbourPosition) == false)
                    {
                        // not in area
                        continue;
                    }

                    if (cells[neighbourPosition].isReady == false)
                    {
                        // we accept only ready neighbour
                        continue;
                    }

                    bool isDiagonal = (x != solvePosition.x) && (y != solvePosition.y);

                    int stepLengthToNeighbour = isDiagonal ? twoAxisStep : oneAxisStep;

                    Cell suggestion = new Cell(true, stepLengthToNeighbour + cells[neighbourPosition].distanceToTarget, neighbourPosition - solvePosition);

                    if (solution.isReady == false || suggestion.distanceToTarget < solution.distanceToTarget)
                    {
                        solution = suggestion;
                    }
                }
            }

            return solution;
        }

        private bool IsOnGrid(Vector2Int position) => world.GetWorldSize().Belongs(position);
    }
}
