//this empty line for UTF-8 BOM header

using System;
using System.Collections.Generic;
using AlgorithmsDemo.DTS;
using AlgorithmsDemo.World;
using UnityEngine;

namespace AlgorithmsDemo.Algoritms
{
    public class AStarPathBuilder
    {
        private struct Cell
        {
            public readonly bool isReady;
            public readonly int distFromStart; //accurately calculated
            public readonly int distToEnd; //predicted shortest way

            public int PathLength => distFromStart + distToEnd;

            public Cell(bool isReady, int distFromStart, int distToEnd)
            {
                this.isReady = isReady;
                this.distFromStart = distFromStart;
                this.distToEnd = distToEnd;
            }

            public static Cell Default => new Cell(false, int.MaxValue, int.MaxValue);

            public override string ToString() => $"(isReady={isReady}, distFromStart={distFromStart}, distToEnd={distToEnd})";
        }

        public event Action OnRefresh = () => { };
        public IEnumerable<Vector2Int> LastPath => lastPath;

        private readonly WorldForPathBuilder world;
        private readonly ArrayXY<Cell> cells;

        private readonly List<Vector2Int> lastPath = new List<Vector2Int>();

        private const int oneAxisStep = 2;
        private const int twoAxisStep = 3;
        private const int maxCycles = 1000;

        public AStarPathBuilder(WorldForPathBuilder world)
        {
            this.world = world;
            cells = new ArrayXY<Cell>(world.GetWorldSize(), Cell.Default, pos => Cell.Default);
        }

        public void BuildPath(Vector2Int from, Vector2Int to, List<Vector2Int> toFill)
        {
            cells.ResetToValue(Cell.Default);

            RectAreaInt workArea = new RectAreaInt(from.x, from.y, 0, 0);

            workArea.Expand(from, 1);
            workArea.Expand(to, 1);
            workArea.Clamp(world.GetWorldSize());

            int expectedPathLength = GetShortestPathLength(from, to);
            cells[from] = new Cell(true, 0, expectedPathLength);

            int stuckDefense = 0;
            while (true)
            {
                bool someSolutionFound = false;
                Cell bestSolution = Cell.Default;

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
                            Cell solution = TrySolve(pos, to);
                            if (solution.isReady == true)
                            {
                                if (bestSolution.isReady == false || bestSolution.PathLength > solution.PathLength)
                                {
                                    bestSolution = solution;
                                }

                                if (solution.PathLength == expectedPathLength)
                                {
                                    cells[pos] = solution;
                                    someSolutionFound = true;

                                    //if border reached => expand work area
                                    workArea.Expand(pos, 1);
                                    workArea.Clamp(world.GetWorldSize());
                                }

                                if (solution.PathLength < expectedPathLength)
                                {
                                    throw new Exception($"path found that is shorter than expected during calculation: from {from} to {to}, in cell {pos}");
                                }
                            }
                        }
                    }
                }

                //if destination reached => return path
                if (cells[to].isReady == true)
                {
                    FinalizePath(from, to);

                    OnRefresh();

                    toFill.AddRange(lastPath);

                    return;
                }

                //if success => just repeat
                if (someSolutionFound)
                {
                    continue;
                }
                else
                {
                    if (bestSolution.isReady == true)
                    {
                        //if to path found => increase path length limit and repeat
                        expectedPathLength = bestSolution.PathLength;
                    }
                    else
                    {
                        //if no path found and no solution at all => return error
                        throw new InvalidOperationException($"PATH ERROR: no path from {from} to {to}");
                    }
                }

                if (stuckDefense++ > maxCycles)
                {
                    throw new InvalidOperationException($"PATH ERROR: stuck in cycle more than {maxCycles} times, no path from {from} to {to}");
                }
            }
        }

        private void FinalizePath(Vector2Int from, Vector2Int to)
        {
            lastPath.Clear();

            Vector2Int iterator = to;

            // build path by reverse
            while (iterator.x != from.x || iterator.y != from.y)
            {
                lastPath.Insert(0, iterator);
                Vector2Int newIterator = StepBack(iterator);
                if (newIterator.x == iterator.x && newIterator.y == iterator.y)
                {
                    throw new Exception($"PATH ERROR: path reverse from {to} to {from} stuck in point {newIterator}");
                }

                iterator = newIterator;
            }

            lastPath.Insert(0, from);

            // remove extra points from path
            for (int i = lastPath.Count - 2; i >= 1; i--)
            {
                if (IsLineWalkable(lastPath[i - 1], lastPath[i + 1]) == true)
                {
                    lastPath.RemoveAt(i);
                }
            }
        }

        private Cell TrySolve(Vector2Int solvePosition, Vector2Int goalPosition)
        {
            if (world.IsCellWalkable(solvePosition) == false)
            {
                return Cell.Default;
            }

            int distToEnd = GetShortestPathLength(solvePosition, goalPosition);

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

                    int stepLengthFromNeighbour = isDiagonal ? twoAxisStep : oneAxisStep;

                    Cell suggestion = new Cell(true, cells[neighbourPosition].distFromStart + stepLengthFromNeighbour, distToEnd);

                    if (solution.isReady == false || suggestion.PathLength < solution.PathLength)
                    {
                        solution = suggestion;
                    }
                }
            }

            return solution;
        }

        private int GetShortestPathLength(Vector2Int from, Vector2Int to)
        {
            int dx = Math.Abs(from.x - to.x);
            int dy = Math.Abs(from.y - to.y);
            int dmin = Math.Min(dx, dy);
            int dmax = Math.Max(dx, dy);
            return dmin * twoAxisStep + (dmax - dmin) * oneAxisStep;
        }

        private Vector2Int StepBack(Vector2Int from)
        {
            Vector2Int result = from;

            for (int x = from.x - 1; x <= from.x + 1; x++)
            {
                for (int y = from.y - 1; y <= from.y + 1; y++)
                {
                    Vector2Int suggestion = new Vector2Int(x, y);

                    if (IsOnGrid(suggestion) == false)
                    {
                        // not in area
                        continue;
                    }

                    Cell suggestionCell = cells[suggestion];

                    if (suggestionCell.isReady == true
                        &&
                        suggestionCell.distFromStart < cells[result].distFromStart
                        &&
                        suggestionCell.PathLength <= cells[result].PathLength)
                    {
                        result = suggestion;
                    }
                }
            }

            return result;
        }

        private bool IsLineWalkable(Vector2Int from, Vector2Int to)
        {
            int dx = to.x - from.x;
            int dy = to.y - from.y;
            int dMax = Math.Max(Math.Abs(dx), Math.Abs(dy));

            for (int i = 0; i <= dMax; i++)
            {
                float normalized = (float)i / (float)dMax;

                Vector2Int probe = from + Vector2Int.RoundToInt(new Vector2(dx, dy) * normalized);

                if (world.IsCellWalkable(probe) == false)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsOnGrid(Vector2Int position) => world.GetWorldSize().Belongs(position);
    }
}
