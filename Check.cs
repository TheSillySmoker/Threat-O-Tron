using System.Reflection.PortableExecutable;
using System.Security.Cryptography;

namespace Threat_o_tron;

class Check : Map{
    private int AgentGameX{get;set;}
    private int AgentGameY{get;set;}
    private int AgentMapX{get;set;}
    private int AgentMapY{get;set;}

    /// <summary>
    /// Constructs a check object that can be used for printing out which shows the surrounding directions not 
    /// </summary>
    /// <param name="southWestX"></param>
    /// <param name="southWestY"></param>
    /// <param name="game"></param>
    public Check(int southWestX, int southWestY, Game game) : base(southWestX, southWestY, 3, 3, game.obstacles)
    {
        AgentGameX = southWestX+1;
        AgentGameY = southWestY+1;
        FindPointOnMap(AgentGameX,AgentGameY, out int agentMapX, out int agentMapY);
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
        if(!ContainsPoint(x,y))
        {
            throw new ArgumentException("Your x and y should be in the map you have created");
        }
        char character = Canvas[y,x];
        if (character == '.')
        {
            return true;
        }
        return false;    
    }
    
    /// <summary>
    /// Prints out all the safe directions the agent can go.
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
        }else
        {
            Console.WriteLine("You cannot safely move in any direction. Abort mission.");
        }
        Console.WriteLine();
    }
}