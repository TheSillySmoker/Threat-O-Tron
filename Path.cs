using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Threat_o_tron;

class Path : Map
{
    public List<KeyValuePair<Game.Direction, int>> Directions {get; private set;}
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
    /// Instantiates a new Path object that will attempt to find a safe path from an agent's starting point to an objective. 
    /// </summary>
    /// <param name="agentGameX">The X coordinate for the agent in the game.</param>
    /// <param name="agentGameY">The Y coordinate for the agent in the game.</param>
    /// <param name="objectiveGameY">The X coordinate for the objective in the game.</param>
    /// <param name="objectiveGameX">The Y coordinate for the objective in the game.</param>
    /// <param name="obstacles">The obstacles in the game. The agent will avoid these on its' path.</param>
    public Path(int agentGameX, int agentGameY, int objectiveGameX, int objectiveGameY, List<IObstacle> obstacles) 
    // When calling the Map constructor in base:
    // The southwest point will be the lowest Y (south) and X (west) given by the agent and the objective. Minus 15 on both X and Y to provide padding around the objective or agent. 
    // The size will be the distance between agent's and objective's Xs and Ys. Plus 20 to account for 0 based arrays and padding. 
    : base(
        Math.Min(agentGameX, objectiveGameX) - 15, 
        Math.Min(agentGameY, objectiveGameY) - 15, 
        Math.Abs(objectiveGameX - agentGameX) + 20, 
        Math.Abs(objectiveGameY - agentGameY) + 20, 
        obstacles
    )
    {
        GetMapCoordinates(agentGameX, agentGameY, out int agentMapX, out int agentMapY);
        AgentMapX = agentMapX;
        AgentMapY = agentMapY;

        GetMapCoordinates(objectiveGameX, objectiveGameY, out int objectiveMapX, out int objectiveMapY);
        ObjectiveMapX = objectiveMapX;
        ObjectiveMapY = objectiveMapY;

        Obstacles = obstacles;

        Directions = new List<KeyValuePair<Game.Direction, int>>{};
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
    /// Plots agent and objective on the map and begins the attempt to find the path from agent to objective.
    /// </summary>
    public bool AttemptMission()
    {
        CheckAndPlot(AgentMapX, AgentMapY, 'A');
        CheckAndPlot(ObjectiveMapX, ObjectiveMapY, 'O');

        // Adjust the ordering to start with the smaller distance.
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
        PrintMap(); //todo: remove this

        return AgentMapX == ObjectiveMapX && AgentMapY == ObjectiveMapY;
    }

    private void ObstacleInTheWay(List<string> options)
    {   
        GetGameCoordinates(AgentMapX, AgentMapY, out int agentGameX, out int agentGameY);
        Check check = new Check(agentGameX, agentGameY, Obstacles);
        List<string> safeDirections = check.GetSafeDirections();
            
        if(safeDirections.Contains(options[0]) || safeDirections.Contains(options[1]))
        {
            if (options.Contains("North") || options.Contains("South"))
            {
                MoveOnYAxis(true);
            }
            else if (options.Contains("East") || options.Contains("West"))
            {
                MoveOnXAxis(true);
            }
        }
    }

    ///<summary> 
    ///Determines based on the situation how the agent needs to move on the Y axis.
    ///</summary>
    /// <param name="avoidingObstacle">Whether the agent is needing to attemp to move around an obstacle.</param>
    private void MoveOnYAxis(bool avoidingObstacle)
    {
        //in the case below, the agent has met with the objective.
        if(!avoidingObstacle && AgentMapY == ObjectiveMapY)
        {
            return;
        }

        //when the agent is in-line with the objective on the x axis but not on the y.
        //but there is an obstacle in the way
        if(avoidingObstacle)
        {
            AvoidYAxisObstacle();
        }

        //When the agent is above the obstacle
        if (YKlicksFromOjective > 0 && !avoidingObstacle)
        {
            HeadSouth(YKlicksFromOjective);
        }
        
        //When the agent is below the obstacle
        if (YKlicksFromOjective < 0 && !avoidingObstacle)
        {
            HeadNorth(YKlicksFromOjective);
        }
    }   

    ///<summary> 
    ///Determines based on the situation how the agent needs to move on the X axis.
    ///</summary>
    /// <param name="avoidingObstacle">Whether the agent is needing to attemp to move around an obstacle.</param>
    private void MoveOnXAxis(bool avoidingObstacle)
    {
        //in the case below, the agent has met with the objective.
        if(!avoidingObstacle && AgentMapX == ObjectiveMapX)
        {
            return;
        }

        //when the agent is not in-line with the y.
        //but there is an obstacle in the way
        if(avoidingObstacle)
        {
            AvoidXAxisObstacle();
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
    private void AvoidXAxisObstacle()
    {
        bool clearPathToTheEast = false;
        bool clearPathToTheWest = false;
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

        //when the point exists and it is not blocked to the east
        if (ContainsPoint(AgentMapX + 1, AgentMapY) && Canvas[AgentMapY, AgentMapX + 1] == '.')
        {
            clearPathToTheEast = true;
        }

        //check the position north/south east of the agent
        //AND checks if the point you are checking will actually be on the map.
        while(Canvas[AgentMapY + yChange, AgentMapX + eastChange] != '.' && eastChange + 2 < Width - (Width - (Width - AgentMapX)) - 1)
        { 
            //If it is blocked to the east.
            //OR if it is off the map.
            if(Canvas[AgentMapY, AgentMapX + eastChange] != '.' || eastChange +2 > Width - (Width - (Width - AgentMapX)) - 1)
            {
                clearPathToTheEast = false;
                break;
            }
            clearPathToTheEast = true;
            eastChange++;
        }

        //when the point exists and it is not blocked
        if (ContainsPoint(AgentMapX - 1, AgentMapY) && Canvas[AgentMapY, AgentMapX - 1] == '.')
        {
            clearPathToTheWest = true;
        }
        
        //check the position north/south west of the agent
        //AND checks if the point you are checking will actually be on the map.
        while(Canvas[AgentMapY + yChange, AgentMapX - westChange] != '.' && westChange - 2 < Width -(Width - AgentMapX) - 1 )
        { 
            //If it is blocked to the west.
            //OR if it is off the map.
            if(Canvas[AgentMapY, AgentMapX - westChange] != '.' || westChange +2 > Width -(Width - AgentMapX) - 1)
            {
                clearPathToTheWest = false;
                break;
            }  
            clearPathToTheWest = true;
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
    private void AvoidYAxisObstacle()
    {

        bool clearPathToTheSouth = false;

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

        KeyValuePair<bool, int> northReconnaissance = GetNorthReconnaissance(xChange);

        //Ensure that the point you will be checking exists on the map.
        if(ContainsPoint(AgentMapX + xChange, AgentMapY + southChange))
        {
            //when the point exists and it is not blocked to the south
            if (ContainsPoint(AgentMapX, AgentMapY + 1) && Canvas[AgentMapY + 1, AgentMapX] == '.')
            {
                clearPathToTheSouth = true;
            }
            
            //check the position south east/west of the agent
            while(Canvas[AgentMapY + southChange, AgentMapX + xChange] != '.' && southChange + 2 < Height - (Height - (Height -AgentMapY)) )
            { 
                //If it is blocked to the south.
                //OR if it is off the map.
                if(Canvas[AgentMapY + southChange, AgentMapX] != '.' || southChange + 2 > Height - (Height - (Height -AgentMapY)) )
                {
                    clearPathToTheSouth = false;
                    break;
                }  
                clearPathToTheSouth = true;
                southChange++;
            }
        }
        
        if(northReconnaissance.Key)
        {   
            HeadNorth(northReconnaissance.Value);
        }

        else if (clearPathToTheSouth)
        {
            HeadSouth(southChange);
        } 
    }

 
    private KeyValuePair<bool, int> GetNorthReconnaissance(int xChange)
    {
        bool clearPathToTheNorth = false;
        int northChange = 1;
        //Ensure that the point you will be checking exists on the map.
        if(ContainsPoint(AgentMapX + xChange, AgentMapY - northChange))
        {
            //when the point exists and it is not blocked to the north
            if (ContainsPoint(AgentMapX, AgentMapY - 1) && Canvas[AgentMapY - 1, AgentMapX] == '.')
            {
                clearPathToTheNorth = true;
            }

            //check the position north east/west of the agent
            while(Canvas[AgentMapY - northChange , AgentMapX + xChange] != '.' && northChange < Height - (Height -AgentMapY))
            { 
                //If it is blocked to the north.
                //OR if it is off the map (too far).
                if(Canvas[AgentMapY - northChange, AgentMapX] != '.' || northChange + 2 > Height - (Height -AgentMapY))
                {
                    clearPathToTheNorth = false;
                    break;
                }  
                clearPathToTheNorth = true;
                northChange++;
            }
        }
        return new(clearPathToTheNorth, northChange);
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
                    Directions.Add(new(Game.Direction.North, counter));
                    AgentMapX = ObjectiveMapX;
                    AgentMapY = ObjectiveMapY;
                    return;
                }
                //When there is an obstacle in the way
                else
                {
                    AgentMapY -= y-1;
                    Directions.Add(new(Game.Direction.North, counter - 1));
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
        Directions.Add(new(Game.Direction.North, counter));
        MoveOnXAxis(false);
    }

    /// <summary>
    /// Checks if the path is clear and moves the agent as far east toward the objective as possible
    /// </summary>
    /// <param name="klicks">How far you want to send the agent.</param>
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
                    Directions.Add(new(Game.Direction.East, counter));
                    AgentMapX = ObjectiveMapX;
                    AgentMapY = ObjectiveMapY;
                    return;
                }

                //When there is an obstacle in the way
                else 
                {            
                    AgentMapX += x -1;
                    Directions.Add(new(Game.Direction.East, counter - 1));
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
        Directions.Add(new(Game.Direction.East, counter));
        MoveOnYAxis(false);
    }

    /// <summary>
    /// Checks if the path is clear and moves the agent as far south toward the objective as possible
    /// </summary>
    /// <param name="klicks">How far you want to send the agent.</param>
    private void HeadSouth(int klicks)
    {
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
                    Directions.Add(new(Game.Direction.South, counter));
                    AgentMapX = ObjectiveMapX;
                    AgentMapY = ObjectiveMapY;
                    return;
                }

                //When there is an obstacle in the way
                else
                {
                    AgentMapY += y-1;
                    Directions.Add(new(Game.Direction.South, counter - 1));
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
        Directions.Add(new(Game.Direction.South, counter));
        MoveOnXAxis(false);
    }

    /// <summary>
    /// Checks if the path is clear and moves the agent as far west toward the objective as possible
    /// </summary>
    /// <param name="klicks">How far you want to send the agent.</param>
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
                    Directions.Add(new(Game.Direction.West, counter));
                    AgentMapX = ObjectiveMapX;
                    AgentMapY = ObjectiveMapY;
                    return;
                }

                //When there is an obstacle in the way
                else
                {
                    AgentMapX -= x-1;
                    Directions.Add(new(Game.Direction.West, counter - 1));
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
        Directions.Add(new(Game.Direction.West, counter));
        MoveOnYAxis(false);
    }
}