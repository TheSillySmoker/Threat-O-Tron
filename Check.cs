using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Threat_o_tron;

class Check 
{
    /// <summary>
    /// AgentMapX and AgentMapY are the agent's coordinates on the map that will be created. 
    /// </summary>
    private readonly int AgentMapX;
    private readonly int AgentMapY;
    
    private readonly Map CheckMap;

    /// <summary>
    /// Constructs a check object that can be used to show the surrounding unobstructed directions.
    /// Creates a 3X3 map 
    /// </summary>
    /// <param name="x">The X Coodinate of your agent in the Game.</param>
    /// <param name="y">The Y Coodinate of your agent in the Game.</param>
    /// <param name="obstacles">Existing obstacles in the game.</param>
    public Check(int x, int y, List<IObstacle> obstacles)
    {
        //The agent's position will be in the middle of the map.
        CheckMap = new Map(x -1, y -1, 3, 3, obstacles);
        CheckMap.GetMapCoordinates(x, y, out int agentMapX, out int agentMapY);
        AgentMapX = agentMapX;
        AgentMapY = agentMapY;
    }

    /// <summary>
    /// Gets all the directions the agent can move to safely.
    /// </summary>
    /// <returns>A list of strings (directions).</returns>
    public List<string> GetSafeDirections()
    {
        List<string> safeDirections = [];
        if(CheckIfSafe(AgentMapX, AgentMapY-1))
        {
            safeDirections.Add("North");
        }
        if(CheckIfSafe(AgentMapX, AgentMapY+1))
        {
            safeDirections.Add("South");
        }
        if(CheckIfSafe(AgentMapX+1, AgentMapY))
        {
            safeDirections.Add("East");
        }
        if(CheckIfSafe(AgentMapX-1, AgentMapY))
        {
            safeDirections.Add("West");
        }
        return safeDirections;
    }

    /// <summary>
    /// Checks to see if a given location is compromised or not.
    /// </summary>
    /// <param name="x">The X coordinate of the map that will be checked.</param>
    /// <param name="y">The Y coordinate of the map that will be checked.</param>
    /// <returns>True or false if the given coordinate is safe.</returns>
    private bool CheckIfSafe(int x, int y)
    {
        Debug.Assert(CheckMap.ContainsPoint(x,y), "Your x and y should be in the map you have created");
        return CheckMap.Canvas[y,x] == '.';
    }
    
    /// <summary>
    /// Prints all the safe directions the agent can go.
    /// Checks if the agent is compromised.
    /// </summary>
    public void PrintSafeDirections()
    {
        // Check the agents location.
        if(!CheckIfSafe(AgentMapX,AgentMapY))
        {
            Console.WriteLine("Agent, your location is compromised. Abort mission.");
            return;
        }
        // Prints safe surroundings if there are any.
        List<string> safeDirections = GetSafeDirections();
        if (safeDirections.Count > 0)
        {
            Console.WriteLine("You can safely take any of the following directions:");
            foreach(string safeDirection in safeDirections)
            {
                Console.WriteLine(safeDirection);
            }
        }
        else
        {
            Console.WriteLine("You cannot safely move in any direction. Abort mission.");
            Console.WriteLine();
        }
    }
}