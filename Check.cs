using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Threat_o_tron;

class Check : Map
{
    private readonly int AgentMapX;
    private readonly int AgentMapY;

    /// <summary>
    /// Constructs a check object that can be used for printing out which shows the surrounding unobstructed directions.
    /// </summary>
    /// <param name="agentGameX">The game X Coodinate of your agent.</param>
    /// <param name="agentGameY">The game Y Coodinate of your agent.</param>
    /// <param name="obstacles">Existing obstacles in the game.</param>
    public Check(int agentGameX, int agentGameY, List<IObstacle> obstacles) : base(agentGameX -1, agentGameY -1, 3, 3, obstacles)
    {
        //The agent's position will be in the middle of the map.
        //In a check, the map is 3x3 in size so we need to adjust up and across by 1 to get the centre. 
        GetMapCoordinates(agentGameX, agentGameY, out int agentMapX, out int agentMapY);
        AgentMapX = agentMapX;
        AgentMapY = agentMapY;
    }

    /// <summary>
    /// Gets all the directions the agent can move to safely.
    /// </summary>
    /// <returns>A list of strings (directions).</returns>
    public List<string> GetSafeDirections()
    {
        // JSS CodeReview: You can do List<string> safeDirections = [].
        List<string> safeDirections = new List<string>();
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
        Debug.Assert(ContainsPoint(x,y), "Your x and y should be in the map you have created");
        // JSS CodeReview: This could be written as "return Canvas[y,x] == '.';".
        if (Canvas[y,x] == '.')
        {
            return true;
        }
        else
        {
            return false;  
        }
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
        // Get agent's surroundings and print them if there are any.
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
        }
        Console.WriteLine();
    }
}