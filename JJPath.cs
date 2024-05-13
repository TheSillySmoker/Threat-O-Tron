using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;

namespace Threat_o_tron;

class JJPath : Map
{
    private bool[,] Visited;
    private bool[,] Solution;

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

        Visited = new bool[Height, Width];
        Solution = new bool[Height, Width];
    }

    public bool IsObjectiveBlocked() {
        return Canvas[ObjectiveMapY, ObjectiveMapX] != '.';
    }

    public bool IsAgentBlocked() {
        return Canvas[AgentMapY, AgentMapX] != '.';
    }

    public bool FindPath() {
        bool result = Move(AgentMapX, AgentMapY);

        // These are plotted afterwards as the Move method sees them as obstacles.
        CheckAndPlot(AgentMapX, AgentMapY, 'A');
        CheckAndPlot(ObjectiveMapX, ObjectiveMapY, 'O');

        return result;
    }

    /// <summary>
    /// Recursive method that finds a path from the agent's coordinates to the objective's coordinates.
    /// JSS CodeReview: This needs to be written better.
    /// </summary>
    /// <param name="currentX"></param>
    /// <param name="currentY"></param>
    /// <param name="directions"></param>
    /// <returns>True, if the objective has successfully been reached without any obstructions.</returns>
    private bool Move(int currentX, int currentY) {
        // Cannot move out of bounds.
        if (currentX < 0 || currentX >= Width || currentY < 0 || currentY >= Height)
        {
            return false;
        }

        if (currentX == ObjectiveMapX && currentY == ObjectiveMapY) {
            Solution[currentY, currentX] = true;
            return true;
        }
        
        if (Canvas[currentY, currentX] != '.' || Visited[currentY, currentX]) 
        {
            return false;
        }

        Visited[currentY, currentX] = true;


        // Try moving north.
        if (Move(currentX, currentY - 1)) 
        {
            Solution[currentY, currentX] = true;
            Canvas[currentY - 1, currentX] = 'N';
            return true;
        }

        // Try moving east.
        if (Move(currentX + 1, currentY))
        {
            Solution[currentY, currentX] = true;
            Canvas[currentY, currentX + 1] = 'E';
            return true;
        }

        // Try moving west.
        if (Move(currentX - 1, currentY)) 
        {
            Solution[currentY, currentX] = true;
            Canvas[currentY, currentX - 1] = 'W';
            return true;
        }

        // Try moving south.
        if (Move(currentX, currentY + 1)) {
            Solution[currentY, currentX] = true;
            Canvas[currentY + 1, currentX] = 'S';
            return true;
        }

        return false;
    }

    private bool IsInBounds(int currentX, int currentY)
    {
        return currentX >= 0 && currentX < Width && currentY >= 0 && currentY < Height;
    }

    public List<KeyValuePair<Game.Direction, int>> GetDirections() 
    {
        List<KeyValuePair<Game.Direction, int>> directions = [];

        int currentX = AgentMapX;
        int currentY = AgentMapY;
        bool objectiveReached;

        do {
            if (IsInBounds(currentX, currentY - 1) && Solution[currentY - 1, currentX])
            {
                KeyValuePair<Game.Direction, int> direction = GetDirection(Game.Direction.North, currentX, currentY);
                directions.Add(direction);
                currentY -= direction.Value;
            }
            else if (IsInBounds(currentX + 1, currentY) && Solution[currentY, currentX + 1])
            {
                KeyValuePair<Game.Direction, int> direction = GetDirection(Game.Direction.East, currentX, currentY);
                directions.Add(direction);
                currentX += direction.Value;
            }
            else if (IsInBounds(currentX - 1, currentY) && Solution[currentY, currentX - 1])
            {
                KeyValuePair<Game.Direction, int> direction = GetDirection(Game.Direction.West, currentX, currentY);
                directions.Add(direction);
                currentX -= direction.Value;
            }
            else if (IsInBounds(currentX, currentY + 1) && Solution[currentY + 1, currentX])
            {
                KeyValuePair<Game.Direction, int> direction = GetDirection(Game.Direction.South, currentX, currentY);
                directions.Add(direction);
                currentY += direction.Value;
            }

            objectiveReached = currentX == ObjectiveMapX && currentY == ObjectiveMapY;
        } while (!objectiveReached);

        return directions;
    }    
    
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

    private KeyValuePair<Game.Direction, int> GetDirection(Game.Direction direction, int currentX, int currentY)
    {
        GetDirectionModifiers(direction, out int xModifier, out int yModifier);

        int length = 0;

        do {
            // Remove the solution as we go so we don't backtrack.
            Solution[currentY, currentX] = false;

            currentX += xModifier;
            currentY += yModifier;
            length++;

            if (!IsInBounds(currentX, currentY))
            {
                break;
            }
        } while (Solution[currentY, currentX]);

        return new(direction, length - 1);
    }
}