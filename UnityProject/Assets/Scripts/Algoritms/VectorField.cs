//this empty line for UTF-8 BOM header

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

        internal RectAreaInt Area => world.GetWorldSize();
        internal Vector2Int GetDestinationTarget() => destinationTarget;

        private readonly WorldForPathBuilder world;
        private readonly ArrayXY<Cell> cells;
        private Vector2Int destinationTarget;

        private const int oneAxisStep = 2;
        private const int twoAxisStep = 3;
        private const int maxCycles = 1000;

        public VectorField(WorldForPathBuilder world)
        {
            this.world = world;
            cells = new ArrayXY<Cell>(Area, Cell.Default, pos => Cell.Default);
        }

        public void BuildField(Vector2Int target, int iterations, out string processInfoMessage)
        {
            destinationTarget = target;

            cells.ResetToValue(Cell.Default);

            RectAreaInt workArea = Area;

            cells[target] = new Cell(true, 0, Vector2Int.zero);

            int stuckDefense = 0;
            int incrementalRange = 0;

            if (--iterations <= 0)
            {
                processInfoMessage = "initialized";
                return;
            }

            while (true)
            {
                incrementalRange++;

                bool someSolutionFound = false;

                int xMin = workArea.xMin;
                int xMax = workArea.xMax;
                int yMin = workArea.yMin;
                int yMax = workArea.yMax;

                for (int x = xMin; x <= xMax; x++)
                {
                    if (Mathf.Abs(x - target.x) > incrementalRange) continue;

                    for (int y = yMin; y <= yMax; y++)
                    {
                        if (Mathf.Abs(y - target.y) > incrementalRange) continue;

                        Vector2Int pos = new Vector2Int(x, y);
                        Cell oldCell = cells[pos];
                        Cell newCell = TrySolve(pos);
                        bool cellSolutionFound = newCell.isReady == true
                                                 &&
                                                 (
                                                     oldCell.isReady == false
                                                     ||
                                                     oldCell.distanceToTarget > newCell.distanceToTarget
                                                 );
                        if (cellSolutionFound == true)
                        {
                            cells[pos] = newCell;
                            someSolutionFound = true;

                            if (--iterations <= 0)
                            {
                                processInfoMessage = "one more cell solved";
                                return;
                            }
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

            processInfoMessage = "job done";
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

        private bool IsOnGrid(Vector2Int position) => Area.Belongs(position);
    }
}
