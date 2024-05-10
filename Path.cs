using System.ComponentModel;
using System.Dynamic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Win32.SafeHandles;

namespace Threat_o_tron;

class Path : Map{

    public List<string> Directions {get; private set;}
    private int AgentMapX{get; set;}
    private int AgentMapY{get; set;}

    private readonly int ObjectiveMapX;
    private readonly int ObjectiveMapY;
    private readonly List<IObstacle> Obstacles;

    /// <summary>
    /// Instantiates a new path object that will attempt to find a safe path from an agent's starting point to an objective. 
    /// </summary>
    /// <param name="agentGameX">The X coordinate for the agent in the game.</param>
    /// <param name="agentGameY">The Y coordinate for the agent in the game.</param>
    /// <param name="objectiveGameY">The X coordinate for the objective in the game.</param>
    /// <param name="objectiveGameX">The Y coordinate for the objective in the game.</param>
    /// <param name="obstacles">The obstacles in the game. The agent will avoid these on its' path.</param>
    public Path(int agentGameX, int agentGameY, int objectiveGameX, int objectiveGameY, List<IObstacle> obstacles) 
    //When calling the Map constructor in base:
    //The southwest point will be the lowest Y (south) and X (west) given by the agent and the objective. Minus 1 on both X and Y to provide padding around the objective or agent. 
    //The size will be the distance between agent's and objective's Xs and Ys. Plus 3 to account for 0 based arrays and padding. 
    : base(GetLowerNumber(agentGameX, objectiveGameX)-1, GetLowerNumber(agentGameY, objectiveGameY)-1, Math.Abs(objectiveGameX - agentGameX)+3, Math.Abs(objectiveGameY - agentGameY)+3, obstacles)
    {

        GetMapCoordinates(agentGameX, agentGameY, out int agentMapX, out int agentMapY);
        AgentMapX = agentMapX;
        AgentMapY = agentMapY;
        GetMapCoordinates(objectiveGameX, objectiveGameY, out int objectiveMapX, out int objectiveMapY);
        ObjectiveMapX = objectiveMapX;
        ObjectiveMapY = objectiveMapY;

        Obstacles = obstacles;

        Directions = [];
    }
    /// <summary>
    /// Takes two ints and returns the smaller number.
    /// </summary>
    /// <param name="coordinate1">One of the Ints you want to compare.</param>
    /// <param name="coordinate2">One of the Ints you want to compare.</param>
    /// <returns>The smaller number of the two ints</returns>
    private static int GetLowerNumber(int coordinate1, int coordinate2)
    {
        if (coordinate1 < coordinate2)
        {
            return coordinate1;
        }
        else
        {
            return coordinate2;
        }
    }

    /// <summary>
    /// Checks to see if the objective will be compromised.
    /// </summary>
    /// <returns>Boolean value weather the objective is on a compromised location.</returns>
    public bool ObjectiveIsBlocked()
    {
        if (Canvas[ObjectiveMapY, ObjectiveMapX] != '.')
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AttemptMission()
    {
        CheckAndPlot(AgentMapX, AgentMapY, 'A');
        CheckAndPlot(ObjectiveMapX, ObjectiveMapY, 'O');
        MoveOnXAxis();  
        MoveOnYAxis();      
    }

    private void ObstacleInTheWay(int currentX, int currentY){
        //TODO: delete line below
        Check check = new Check(currentX, currentY, Obstacles);
        if(check.GetSafeDirections().Contains("North"))
        {
            // do something
        }
    }

    private void MoveOnYAxis()
    {
        int yKlicksFromAgent;
        yKlicksFromAgent = ObjectiveMapY - AgentMapY;

        if (yKlicksFromAgent > 0)
        {
            HeadSouth(yKlicksFromAgent);
        }

        if (yKlicksFromAgent < 0)
        {
            HeadNorth(yKlicksFromAgent);
        }
    } 
    private void MoveOnXAxis()
    {
        int xKlicksFromAgent;
        xKlicksFromAgent = ObjectiveMapX - AgentMapX;
        if (xKlicksFromAgent > 0)
        {
            HeadEast(xKlicksFromAgent);
        }

        if (xKlicksFromAgent < 0)
        {
            HeadWest(xKlicksFromAgent);
        }
    }

    private void HeadNorth(int klicks)
    {
        int counter = 0;

        for (int y = 1; y < Math.Abs(klicks)+1; y++)
        {
            counter ++;
            //check to see if there is an obstacle in the way or if agent has reached objective
            char nextX = Canvas[AgentMapY-y, AgentMapX];
            
            if(nextX != '.')
            {
                if (nextX == 'O')
                {
                    Directions.Add($"Head north for {counter} klicks.");
                    //TODO: objective complete
                    return;
                }
                else
                {
                    AgentMapY -= y;
                    Directions.Add($"Head north for {counter-1} klicks");
                    ObstacleInTheWay(AgentMapX, AgentMapY);
                    return;
                }                    
            }
            //No obstacle in the way
            else
            {
                CheckAndPlot(AgentMapX, AgentMapY-y, 'N');
            }
        }
        AgentMapY -= counter;
        Directions.Add($"Head north for {counter} klicks.");
    }

    private void HeadEast(int klicks)
    {
        int counter = 0;

        for (int x = 1; x < klicks+1; x++)
        {
            counter ++;
            //check to see if there is an obstacle in the way or if agent has reached objective
            char nextX = Canvas[AgentMapY, AgentMapX+x];
            
            if(nextX != '.')
            {
                if (nextX == 'O')
                {
                    Directions.Add($"Head east for {counter} klicks.");
                    //TODO: objective complete
                    return;
                }
                else
                {
                    AgentMapX += x;
                    Directions.Add($"Head east for {counter-1} klicks");
                    ObstacleInTheWay(AgentMapX, AgentMapY);
                    return;
                }                    
            }
            //No obstacle in the way
            else
            {
                CheckAndPlot(AgentMapX+x,AgentMapY, 'E');
            }
        }
        AgentMapX += counter;
        Directions.Add($"Head east for {counter} klicks.");
    }

    private void HeadSouth(int klicks)
    {
        int counter = 0;

        for (int y = 1; y < klicks+1; y++)
        {
            counter ++;
            //check to see if there is an obstacle in the way or if agent has reached objective
            char nextX = Canvas[AgentMapY+y, AgentMapX];
            
            if(nextX != '.')
            {
                if (nextX == 'O')
                {
                    Directions.Add($"Head north for {counter} klicks.");
                    //TODO: objective complete
                    return;
                }
                else
                {
                    AgentMapY += y;
                    Directions.Add($"Head north for {counter-1} klicks");
                    ObstacleInTheWay(AgentMapX, AgentMapY);
                    return;
                }                    
            }
            //No obstacle in the way
            else
            {
                CheckAndPlot(AgentMapX, AgentMapY+y, 'S');
            }
        }
        AgentMapY += counter;
        Directions.Add($"Head north for {counter} klicks.");
    }

    private void HeadWest(int klicks)
    {
        int counter = 0;

        for (int x = 1; x < Math.Abs(klicks)+1; x++)
        {
            counter ++;
            //check to see if there is an obstacle in the way or if agent has reached objective
            char nextX = Canvas[AgentMapY, AgentMapX-x];            
            if(nextX != '.')
            {
                if (nextX == 'O')
                {
                    Directions.Add($"Head west for {counter} klicks.");
                    //TODO: objective complete
                    return;
                }
                else
                {
                    AgentMapX -= x;
                    Directions.Add($"Head west for {counter-1} klicks");
                    ObstacleInTheWay(AgentMapX, AgentMapY);
                    return;
                }                    
            }
            //No obstacle in the way
            else
            {
                CheckAndPlot(AgentMapX-x,AgentMapY, 'W');
            }
        }
        AgentMapX -= counter;
        Directions.Add($"Head west for {counter} klicks.");
    }
}