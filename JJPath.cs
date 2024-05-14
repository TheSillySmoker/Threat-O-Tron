using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;

namespace Threat_o_tron;

class JJPath : Map
{
    /// <summary>
    /// Represents the Map canvas, but instead keeps track of which coordinates the pathfinding algorithm has already visited.
    /// If a coordinate is marked as `true`, this means the pathfinding algorithm has been here and does not need to search here again.
    /// This prevents backtracking.
    /// </summary>
    private readonly bool[,] Visited;
    /// <summary>
    /// Represents the Map canvas, but instead keeps track of the solution that the pathfinding algorithm has decided.
    /// If a coordinate is marked as `true`, this means it is part of the path from the agent's starting coordinates to the objective's coordinates.
    /// </summary>
    private readonly bool[,] Solution;

    private int AgentMapX;
    private int AgentMapY;
    private readonly int ObjectiveMapX;
    private readonly int ObjectiveMapY;    
    
    public JJPath(int agentGameX, int agentGameY, int objectiveGameX, int objectiveGameY, List<IObstacle> obstacles) 
    : base(
        Math.Min(agentGameX, objectiveGameX) - 3, 
        Math.Min(agentGameY, objectiveGameY) - 3, 
        Math.Abs(objectiveGameX - agentGameX) + 6, 
        Math.Abs(objectiveGameY - agentGameY) + 6,
        obstacles
    )
    {
        GetMapCoordinates(agentGameX, agentGameY, out AgentMapX, out AgentMapY);
        GetMapCoordinates(objectiveGameX, objectiveGameY, out ObjectiveMapX, out ObjectiveMapY);

        // Initialise the Visited and Solution grids. This will mark each coordinate as `false` in both.
        Visited = new bool[Height, Width];
        Solution = new bool[Height, Width];
    }

    /// <summary>
    /// Checks if the objective is blocked by an Obstacle.
    /// </summary>
    /// <returns>True, if the objective's coordinates are on an Obstacle.</returns>
    public bool IsObjectiveBlocked() {
        return Canvas[ObjectiveMapY, ObjectiveMapX] != '.';
    }

    /// <summary>
    /// Checks if the agent is on an Obstacle.
    /// </summary>
    /// <returns>True, if the agent's coordinates are on an Obstacle.</returns>
    public bool IsAgentBlocked() {
        return Canvas[AgentMapY, AgentMapX] != '.';
    }

    /// <summary>
    /// Runs the pathfinding algorithm which will populate the Canvas with directions as well as produce a Solution.
    /// </summary>
    /// <returns>True, if the pathfinding algorithm was successfully able to reach the objective.</returns>
    /// <see cref="Move"/>
    /// <see cref="Solution"/>
    public bool FindPath() {
        bool result = Move(AgentMapX, AgentMapY);

        // These are plotted afterwards as the Move method sees them as obstacles.
        CheckAndPlot(AgentMapX, AgentMapY, 'A');
        CheckAndPlot(ObjectiveMapX, ObjectiveMapY, 'O');

        return result;
    }

    /// <summary>
    /// Checks if the given coordinate is in bounds of the Map.
    /// </summary>
    /// <param name="currentX">The current X coordinate.</param>
    /// <param name="currentY">The current Y coordinate.</param>
    /// <returns>True, if the given coordinate is in bounds.</returns>
    private bool IsInBounds(int currentX, int currentY)
    {
        return currentX >= 0 && currentX < Width && currentY >= 0 && currentY < Height;
    }

    /// <summary>
    /// Calculates the minimum number of klicks required to reach the objective, ignoring any Obstacles.
    /// </summary>
    /// <param name="currentX">The current X coordinate.</param>
    /// <param name="currentY">The current Y coordinate.</param>
    /// <returns>Minimum number of klicks required to reach the objective from the given coordinate.</returns>
    private int GetMinimumKlicksFromObjective(int currentX, int currentY)
    {
        return Math.Abs(ObjectiveMapX - currentX) + Math.Abs(ObjectiveMapY - currentY);
    }

    /// <summary>
    /// Calculates the priority of directions for the pathfinding algorithm to take.
    /// This makes the pathfinding algorithm take directions that bring it closer to the objective first.
    /// </summary>
    /// <param name="currentX">The current X coordinate.</param>
    /// <param name="currentY">The current Y coordinate.</param>
    /// <returns>A list of directions and their minimum amount of klicks required to reach the objective after moving in that direction.</returns>
    private List<KeyValuePair<Game.Direction, int>> GetDirectionPriority(int currentX, int currentY)
    {
        List<KeyValuePair<Game.Direction, int>> result = [];

        result.Add(new(Game.Direction.North, GetMinimumKlicksFromObjective(currentX, currentY - 1)));
        result.Add(new(Game.Direction.East, GetMinimumKlicksFromObjective(currentX + 1, currentY)));
        result.Add(new(Game.Direction.West, GetMinimumKlicksFromObjective(currentX - 1, currentY)));
        result.Add(new(Game.Direction.South, GetMinimumKlicksFromObjective(currentX, currentY + 1)));

        // Sorts the list by minimum number of klicks required to reach the objective after moving in that direction.
        result.Sort((a, b) => a.Value.CompareTo(b.Value));
        return result;
    }

    /// <summary>
    /// Recursive method (and the core of the pathfinding algorithm) that finds a path from the provided coordinates to the objective's coordinates. 
    /// This implements the recursive maze solving algorithm. It will continually call into itself trying every possible direction, with only one
    /// set of nested method calls eventually returning `true`. Once this method returns `true`, all calls will return to their previous caller and mark 
    /// out the solution, as well as plotting directions on the Canvas. All other sets of nested method calls will eventually return `false` due to
    /// encountering an Obstacle, moving out of bounds, re-visiting a coordinate, or otherwise having no moves to make.
    /// 
    /// <see href="https://en.wikipedia.org/wiki/Maze-solving_algorithm#Recursive_algorithm">Recursize maze solving algorithm.</see>
    /// 
    /// This algorithm is an improvement of the standard recursive maze solving algorithm due to the fact that it prioritises making moves that bring
    /// it closer to the objective first, otherwise it would make moves in the order of north, east, south, west.
    /// </summary>
    /// <param name="currentX">The current X coordinate prior to the pathfinding algorithm making its next move.</param>
    /// <param name="currentY">The current Y coordinate prior to the pathfinding algorithm making its next move.</param>
    /// <returns>True, if the objective has successfully been reached without any obstructions or going out of bounds.</returns>
    private bool Move(int currentX, int currentY) {
        // Cannot move out of bounds.
        if (!IsInBounds(currentX, currentY))
        {
            return false;
        }

        // We have reached the objective, return true to begin falling back and filling out the canvas.
        if (currentX == ObjectiveMapX && currentY == ObjectiveMapY) {
            // Edge case, mark the solution on the objective too for GetDirections().
            Solution[currentY, currentX] = true;
            return true;
        }
        
        // Encountered an obstacle or re-visiting a coordinate.
        if (Canvas[currentY, currentX] != '.' || Visited[currentY, currentX]) 
        {
            return false;
        }

        // Mark this coordinate as visited so the pathfinding algorithm won't attempt to move here again.
        Visited[currentY, currentX] = true;

        // Try and move in each of the four directions, taking priority into account.
        foreach(KeyValuePair<Game.Direction, int> directions in GetDirectionPriority(currentX, currentY))
        {
            switch(directions.Key)
            {
                case Game.Direction.North:
                    // Recursive call into Move in this direction. This comment applies to all switch cases.
                    if (Move(currentX, currentY - 1)) 
                    {
                        // The objective was reached after moving in this direction, so mark the solution and direction on the Canvas then return to the previous caller.
                        // This comment also applies to all the switch cases.
                        Solution[currentY, currentX] = true;
                        Canvas[currentY - 1, currentX] = 'N';
                        return true;
                    }
                    break;
                case Game.Direction.East:
                    if (Move(currentX + 1, currentY))
                    {
                        Solution[currentY, currentX] = true;
                        Canvas[currentY, currentX + 1] = 'E';
                        return true;
                    }
                    break;
                case Game.Direction.West:
                    if (Move(currentX - 1, currentY)) 
                    {
                        Solution[currentY, currentX] = true;
                        Canvas[currentY, currentX - 1] = 'W';
                        return true;
                    }
                    break;
                case Game.Direction.South:
                    if (Move(currentX, currentY + 1)) {
                        Solution[currentY, currentX] = true;
                        Canvas[currentY + 1, currentX] = 'S';
                        return true;
                    }
                    break;
            }
        }

        // No valid direction was found, so this move is also invalid. Fall back to the previous caller.
        return false;
    }

    /// <summary>
    /// After successfully running FindPath, the Solution grid is used to calculate the directions to move to reach the objective from
    /// the agent's starting coordinates. It will follow the path of `true` values in the Solution grid all the way to the objective.
    /// </summary>
    /// <returns>A list of directions and corresponding klicks to move, in the order that they should be taken from the agent's starting coordinates.</returns>
    public List<KeyValuePair<Game.Direction, int>> GetDirections() 
    {
        List<KeyValuePair<Game.Direction, int>> directions = [];

        int currentX = AgentMapX;
        int currentY = AgentMapY;
        bool objectiveReached;

        do {
            // Attempt to move north while in bounds and following the Solution.
            if (IsInBounds(currentX, currentY - 1) && Solution[currentY - 1, currentX])
            {
                // Calculate the amount we should move in this direction. This applies to all directions.
                KeyValuePair<Game.Direction, int> direction = GetDirection(Game.Direction.North, currentX, currentY);
                directions.Add(direction);
                currentY -= direction.Value;
            }
            // Attempt to move east while in bounds and following the Solution.
            else if (IsInBounds(currentX + 1, currentY) && Solution[currentY, currentX + 1])
            {
                KeyValuePair<Game.Direction, int> direction = GetDirection(Game.Direction.East, currentX, currentY);
                directions.Add(direction);
                currentX += direction.Value;
            }
            // Attempt to move west while in bounds and following the Solution.
            else if (IsInBounds(currentX - 1, currentY) && Solution[currentY, currentX - 1])
            {
                KeyValuePair<Game.Direction, int> direction = GetDirection(Game.Direction.West, currentX, currentY);
                directions.Add(direction);
                currentX -= direction.Value;
            }
            // Attempt to move south while in bounds and following the Solution.
            else if (IsInBounds(currentX, currentY + 1) && Solution[currentY + 1, currentX])
            {
                KeyValuePair<Game.Direction, int> direction = GetDirection(Game.Direction.South, currentX, currentY);
                directions.Add(direction);
                currentY += direction.Value;
            }

            // Objective if reached if the current coordinate matches the objective's coordinates.
            objectiveReached = currentX == ObjectiveMapX && currentY == ObjectiveMapY;
        } while (!objectiveReached);

        return directions;
    }    
    
    /// <summary>
    /// Calculates the x and y modifiers that a provided direction will make.
    /// For example, the Y modifier of the North direction is -1 and the X modifier is 0.
    /// </summary>
    /// <param name="direction">The provided direction.</param>
    /// <param name="xModifier">The X modifier.</param>
    /// <param name="yModifier">The Y modifier.</param>
    private static void GetDirectionModifiers(Game.Direction direction, out int xModifier, out int yModifier) {
        switch (direction) {
            case Game.Direction.North:
                xModifier = 0;
                yModifier = -1;
                break;
            case Game.Direction.East:
                xModifier = 1;
                yModifier = 0;
                break;
            case Game.Direction.West:
                xModifier = -1;
                yModifier = 0;
                break;
            case Game.Direction.South:
                xModifier = 0;
                yModifier = 1;
                break;
            default:
                throw new NotImplementedException("Unsupported direction.");
        }
    }

    /// <summary>
    /// Calculates the amount of klicks for a direction to move. We want to keep moving in the direction provided
    /// until we go out of bounds or are no longer following the solution.
    /// </summary>
    /// <param name="direction">The direction to move.</param>
    /// <param name="currentX">The current X coordinate.</param>
    /// <param name="currentY">The current Y coordinate.</param>
    /// <returns>The direction to move in along with the corresponding klicks to move.</returns>
    private KeyValuePair<Game.Direction, int> GetDirection(Game.Direction direction, int currentX, int currentY)
    {
        // Get the direction modifiers for the provided direction.
        GetDirectionModifiers(direction, out int xModifier, out int yModifier);

        // Start off at 0 klicks.
        int length = 0;

        do {
            // Remove the solution as we go so we don't backtrack.
            Solution[currentY, currentX] = false;

            currentX += xModifier;
            currentY += yModifier;
            length++;

            // We are no longer in bounds, so break out of the loop.
            if (!IsInBounds(currentX, currentY))
            {
                break;
            }
        } while (Solution[currentY, currentX]);

        // We always overshoot the length by 1, so subtract it before providing the direction.
        return new(direction, length - 1);
    }
}