using System.Reflection.PortableExecutable;
using System.Security.Cryptography;

namespace Threat_o_tron;

class Check : Map{
    private int AgentX{get;set;}
    private int AgentY{get;set;}
    private int AgentMapX{get;set;}
    private int AgentMapY{get;set;}

    /// <summary>
    /// Constructs a check object that can be used for printing out which shows the surrounding directions not 
    /// </summary>
    /// <param name="southWestX"></param>
    /// <param name="southWestY"></param>
    /// <param name="game"></param>
    public Check(int southWestX, int southWestY, Game game) : base(southWestX, southWestY, 3, 3, game.obstacles){
        AgentX = southWestX+1;
        AgentY = southWestY+1;
        FindPointOnMap(AgentX,AgentY, out int agentMapX, out int agentMapY);
        AgentMapX = agentMapX;
        AgentMapY = agentMapY;
    }

    public List<string> GetSafeDirections(){
        List<string> safeDirections = new List<string>();
        if(CheckSafe(AgentMapX, AgentMapY-1)){
            safeDirections.Add("North");
        }
        if(CheckSafe(AgentMapX, AgentMapY+1)){
            safeDirections.Add("South");
        }
        if(CheckSafe(AgentMapX+1, AgentMapY)){
            safeDirections.Add("East");
        }
        if(CheckSafe(AgentMapX-1, AgentMapY)){
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
    private bool CheckSafe(int x, int y){
        if(!ContainsPoint(x,y)){
            throw new ArgumentException("Your x and y should be in the map you have created");
        }
        char character = Canvas[y,x];
        if (character == '.'){
            return true;
        }
        return false;    
    }

    public void PrintSafeDirections(){
        //check the agents location
        if(!CheckSafe(AgentMapX,AgentMapY)){
        Console.WriteLine("Agent, your location is compromised. Abort mission.");
        return;
        }
        //get agent's surroundings and print them if there are any.
        List<string> safeDirections = GetSafeDirections();
        if (safeDirections.Count > 0){
            Console.WriteLine("You can safely take any of the following directions:");
            foreach(string safeDirection in safeDirections){
                Console.WriteLine(safeDirection);
            }
        }else{
            Console.WriteLine("You cannot safely move in any direction. Abort mission.");
        }
        Console.WriteLine();
    }
}