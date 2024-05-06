using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;

namespace Threat_o_tron;

class Check : Map
{
    private readonly int AgentMapX;
    private readonly int AgentMapY;

    /// <summary>
    /// Constructs a check object that can be used for printing out which shows the surrounding unobstructed directions.
    /// </summary>
    /// <param name="southWestX">The X Coodinate for the southwest point of the map that will be instatiated.</param>
    /// <param name="southWestY">The Y Coodinate for the southwest point of the map that will be instatiated.</param>
    /// <param name="obstacles">Existing obstacles in the game.</param>
    public Check(int southWestX, int southWestY, List<IObstacle> obstacles) : base(southWestX, southWestY, 3, 3, obstacles)
    {
        //The agent's position will be in the middle of the map.
        //In a check, the map is 3x3 in size so we need to adjust up and across by 1 to get the centre. 
        FindPointOnMap(southWestX + 1, southWestY + 1, out int agentMapX, out int agentMapY);
        AgentMapX = agentMapX;
        AgentMapY = agentMapY;
    }

    /// <summary>
    /// Gets all the directions the agent can move to safely.
    /// </summary>
    /// <returns>A list of strings (directions).</returns>
    public List<string> GetSafeDirections()
    {
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
        Trace.Assert(!ContainsPoint(x,y), "Your x and y should be in the map you have created");
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
        //check the agents location
        if(!CheckIfSafe(AgentMapX,AgentMapY))
        {
            Console.WriteLine("Agent, your location is compromised. Abort mission.");
            return;
        }
        //get agent's surroundings and print them if there are any.
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