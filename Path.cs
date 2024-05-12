using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Win32.SafeHandles;

namespace Threat_o_tron;

class Path : Map
{

    public List<Object> Directions {get; private set;}
    private int AgentMapX{get; set;}
    private int AgentMapY{get; set;}

    private int XKlicksFromOjective 
    {
        get
        {
        return ObjectiveMapX - AgentMapX;
        }
    }

    private int YKlicksFromOjective
    {
        get
        {
            return ObjectiveMapY - AgentMapY;
        }
    }


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
    : base(GetLowerNumber(agentGameX, objectiveGameX)-10, GetLowerNumber(agentGameY, objectiveGameY)-10,  Math.Abs(objectiveGameX - agentGameX)+20, Math.Abs(objectiveGameY - agentGameY)+20, obstacles)
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
    /// Checks to see if the objective will be compromised before attempting to generate a path.
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

    /// <summary>
    /// Plots agent and objective on the map and begins the attempt to find the path from agent to objective.
    /// </summary>
    public void AttemptMission()
    {
        CheckAndPlot(AgentMapX, AgentMapY, 'A');
        CheckAndPlot(ObjectiveMapX, ObjectiveMapY, 'O');
        //adjust the ordering to start with the smaller distance
        if (XKlicksFromOjective > YKlicksFromOjective)
        {
            MoveOnYAxis(false);  
            MoveOnXAxis(false);
        }
        else
        {
            MoveOnXAxis(false);
            MoveOnYAxis(false);    
        }  

    }

    private void ObstacleInTheWay(List<string> options)
    {   
        Console.WriteLine("OBS");
        GetGameCoordinates(AgentMapX, AgentMapY, out int agentGameX, out int agentGameY); //TODO: fix this
        Check check = new Check(agentGameX, agentGameY, Obstacles);
        check.PrintSafeDirections();
        List<string> safeDirections = check.GetSafeDirections();
        foreach(string direction in safeDirections)
        {
            Console.WriteLine($"Directions {direction}");
        }
            
        if(safeDirections.Contains(options[0]) || safeDirections.Contains(options[1]))
        {
            if (options[0] == "North" || options[1] == "South")
            {
                MoveOnYAxis(true);
            }
            else if (options[0] == "East" || options[1] == "West")
            {
                MoveOnXAxis(true);
            }
        }
    }

    private void MoveOnYAxis(bool avoidingObstacle)
    {
        if(!avoidingObstacle && AgentMapY == ObjectiveMapY)
        {
            return;
        }

        //when the agent is in-line with the objective on the x axis but not on the y.
        //but there is an obstacle in the way
        if(XKlicksFromOjective != 0 && avoidingObstacle)
        {
            MoveAroundObstacleOnYAxis();
        }

        if (YKlicksFromOjective > 0 && !avoidingObstacle)
        {
            HeadSouth(YKlicksFromOjective);
        }

        if (YKlicksFromOjective < 0 && !avoidingObstacle)
        {
            HeadNorth(YKlicksFromOjective);
        }
    }   

    private void MoveOnXAxis(bool avoidingObstacle)
    {
        //in the case below, the agent has met with the objective, otherwise it is trying to get around an obstacle.
        if(!avoidingObstacle && AgentMapX == ObjectiveMapX)
        {
            return;
        }

        //when the agent is not in-line with the y.
        //but there is an obstacle in the way
        if(YKlicksFromOjective != 0 && avoidingObstacle)
        {
            MoveAroundObstacleOnXAxis();
        }
 
        //when the agent is to the right of the objective
        if (XKlicksFromOjective > 0 && !avoidingObstacle)
        {
            HeadEast(XKlicksFromOjective);
        }

        //when the agent is to the left of the objective
        if (XKlicksFromOjective < 0 && !avoidingObstacle)
        {
            HeadWest(XKlicksFromOjective);
        }
    }

    /// <summary>
    /// When the agent needs to move on the y axis but there is an obstacle in the way.
    /// In this case, this method will be determined whether to go west or east and excecute that.
    /// </summary>
    private void MoveAroundObstacleOnXAxis()
    {
        bool clearPathToTheEast = true;
        bool clearPathToTheWest = true;
        int eastChange = 1;
        int westChange = 1;
        int yChange;

        //When the agent needs to head north.
        if(YKlicksFromOjective < 0)
        {
            yChange = -1;
        }

        //When agent needs to head south.
        else 
        {
            yChange = 1;
        }

        //check the position north/south east of the agent
        //AND checks if the point you are checking will actually be on the map.
        while(Canvas[AgentMapY + yChange, AgentMapX + eastChange] != '.' && eastChange + 2 < Width - (Width - (Width - AgentMapX)) - 1)
        { 
            //If it is not clear to move to the east.
            //OR if you are checking somewhere that is off the map.
            if(Canvas[AgentMapY, AgentMapX + eastChange] != '.' || eastChange +2 < Width - (Width - (Width - AgentMapX)) - 1)
            {
                clearPathToTheEast = false;
                break;
            }
            eastChange++;
        }
        
        //check the position north/south west of the agent
        //AND checks if the point you are checking will actually be on the map.
        while(Canvas[AgentMapY + yChange, AgentMapX - westChange] != '.' && westChange - 2 < Width -(Width - AgentMapX) - 1 )
        { 
            Console.WriteLine(westChange);
            //If it is clear to move to the west.
            if(Canvas[AgentMapY, AgentMapX - westChange] != '.' || westChange -2 < Width -(Width - AgentMapX) - 1)
            {
                clearPathToTheWest = false;
                break;
            }  
            westChange++;
        }

        if(clearPathToTheEast)
        {   
           HeadEast(eastChange);
        }

        else if (clearPathToTheWest)
        {
           HeadWest(westChange);
        }
    }

    /// <summary>
    /// When the agent needs to move on the X axis but there is an obstacle in the way.
    /// In this case, this method will be determined whether to go north or south and excecute that.
    /// </summary>
    private void MoveAroundObstacleOnYAxis()
    {
        Console.WriteLine("MOving around obs");
        bool clearPathToTheNorth = true;
        bool clearPathToTheSouth = true;
        int northChange = 1;
        int southChange = 1;
        int xChange;

        //When the agent needs to head East.
        if(XKlicksFromOjective < 0)
        {
            xChange = -1;
        }
        //When agent needs to head West.
        else 
        {
            xChange = 1;
        }
        Console.WriteLine($"AgentmapX: {AgentMapX}, AgentMapY: {AgentMapY}\nHeight: {Height}");
        //check the position north east/west of the agent
        while(Canvas[AgentMapY - northChange , AgentMapX + xChange] != '.')
        { 
            //If it is clear to move to the north.
            if(Canvas[AgentMapY - northChange, AgentMapX] != '.')
            {
                clearPathToTheNorth = false;
                break;
            }  
            northChange++;
            
            //check if the next position will be on the map     
            if(northChange  > Height - (Height -AgentMapY))
            {
                clearPathToTheNorth = false;
                break;
            }
        }
        //check the position south east/west of the agent
        while(Canvas[AgentMapY + southChange, AgentMapX + xChange] != '.' )
        { 
            //If it is clear to move to the south.
            if(Canvas[AgentMapY + southChange, AgentMapX] != '.' )
            {
                clearPathToTheSouth = false;
                break;
            }  
            southChange++;
            
            //check if the next position will be on the map
            if(southChange + 1 > Height - (Height - (Height -AgentMapY)))
            {
                clearPathToTheSouth = false;
                break;
            }
        }
        
        if(clearPathToTheNorth)
        {   
           HeadNorth(northChange);
        }

        else if (clearPathToTheSouth)
        {
            Console.WriteLine(southChange);
            HeadSouth(southChange);
        }
        

    }


    /// <summary>
    /// Checks if the path is clear and moves the agent as far north toward the objective as possible
    /// </summary>
    /// <param name="klicks">How far you want to send the agent.</param>
    private void HeadNorth(int klicks)
    {
        int counter = 0;

        for (int y = 1; y < Math.Abs(klicks)+1; y++)
        {
            counter ++;
            //check to see if there is an obstacle in the way or if agent has reached objective
            char nextY = Canvas[AgentMapY-y, AgentMapX];
            
            if(nextY != '.')
            {
                if (nextY == 'O')
                {
                    Directions.Add("north");
                    Directions.Add(counter);
                    AgentMapX = ObjectiveMapX;
                    AgentMapY = ObjectiveMapY;
                    return;
                }
                //When there is an obstacle in the way
                else
                {
                    AgentMapY -= y-1;
                    Directions.Add("north");
                    Directions.Add(counter-1);
                    ObstacleInTheWay(["East","West"]);
                    return;
                }                    
            }

            //No obstacle in the way
            else
            {
                CheckAndPlot(AgentMapX, AgentMapY-y, 'N');
            }
        }
        //Move the agent, note direction and how far agent has moved. 
        AgentMapY -= counter;
        Directions.Add("north");
        Directions.Add(counter);
        MoveOnXAxis(false);
    }

    private void HeadEast(int klicks)
    {
        int counter = 0;

        for (int x = 1; x < klicks + 1; x++)
        {
            counter ++;
            //check to see if there is an obstacle in the way or if agent has reached objective
            char nextX = Canvas[AgentMapY, AgentMapX+x];
            
            if(nextX != '.')
            {
                if (nextX == 'O')
                {
                    Directions.Add("east");
                    Directions.Add(counter);
                    AgentMapX = ObjectiveMapX;
                    AgentMapY = ObjectiveMapY;
                    return;
                }

                //When there is an obstacle in the way
                else 
                {
                    
                    AgentMapX += x -1;
                    Console.WriteLine($"x:{AgentMapX}, y: {AgentMapY} ");
                    Directions.Add("east");
                    Directions.Add(counter-1);
                    ObstacleInTheWay(["North","South"]);                   
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
        Directions.Add("east");
        Directions.Add(counter);
        MoveOnYAxis(false);
    }

    private void HeadSouth(int klicks)
    {
        Console.WriteLine($"south klicks: {klicks}");
        int counter = 0;

        for (int y = 1; y < klicks + 1; y++)
        {
            counter ++;
            //check to see if there is an obstacle in the way or if agent has reached objective
            char nextY = Canvas[AgentMapY+y, AgentMapX];
            
            if(nextY != '.')
            {
                if (nextY == 'O')
                {
                    Directions.Add("south");
                    Directions.Add(counter);
                    AgentMapX = ObjectiveMapX;
                    AgentMapY = ObjectiveMapY;
                    return;
                }

                //When there is an obstacle in the way
                else
                {
                    AgentMapY += y-1;
                    Directions.Add("south");
                    Directions.Add(counter-1);
                    ObstacleInTheWay(["East","West"]);
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
        Directions.Add("south");
        Directions.Add(counter);
        MoveOnXAxis(false);
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
                    Directions.Add("west");
                    Directions.Add(counter);
                    AgentMapX = ObjectiveMapX;
                    AgentMapY = ObjectiveMapY;
                    return;
                }

                //When there is an obstacle in the way
                else
                {
                    AgentMapX -= x-1;
                    Directions.Add("west");
                    Directions.Add(counter-1);
                    ObstacleInTheWay(["North","South"]);
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
        Directions.Add("west");
        Directions.Add(counter);
        MoveOnYAxis(false);
    }
}