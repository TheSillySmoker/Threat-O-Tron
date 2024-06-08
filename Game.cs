using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Threat_o_tron;
class Game
{
    /// <summary>
    /// Represents a direction that can be taken on a Map.
    /// </summary>
    public enum Direction {
        North,
        East,
        West,
        South
    }

    /// <summary>
    /// The items that will be drawn on the map and what the agent will avoid in a path.
    /// </summary>
    private readonly List<IObstacle> Obstacles;

    /// <summary>
    /// Instantiates a new blank game.
    /// </summary>
    public Game()
    {
        Obstacles = new List<IObstacle>();
    }

    /// <summary>
    /// When the user uses the add command. 
    /// Takes the arguments to see what it needs to add.
    /// </summary>
    /// <param name="arguments">The arguments that will be analysed and parsed to the specific obstacle that needs to be added.</param>
    /// <exception cref="ArgumentException">Incorrect number of or invalid arguments are provided.</exception>
    public void Add(string[] arguments)
    {
        if(arguments.Length > 1)
        {
            string obstacle = arguments[1];
            // Ensure that the user has a valid obstacle argument.
            switch (obstacle)
            {
                case "GUARD":
                    AddGuard(arguments);
                    break;
                case "FENCE":
                    AddFence(arguments);
                    break;
                case "SENSOR":
                    AddSensor(arguments);
                    break;
                case "CAMERA":
                    AddCamera(arguments);
                    break;
                default:
                    throw new ArgumentException("Invalid obstacle type.");
            }
        }
        else
        {
            throw new ArgumentException("Invalid obstacle type.");
        }  
    }

    /// <summary>
    /// Creates and adds a Guard Obstacle to the current Game. This method also ensures that the arguments are valid. 
    /// </summary>
    /// <param name="arguments">The arguments that this method will analyse before parsing them to instantiate a Guard.</param>
    /// <exception cref="ArgumentException">Incorrect number of or invalid arguments are provided.</exception>
    public void AddGuard(string[] arguments)
    {
        if (arguments.Length != 4)
        {
            throw new ArgumentException("Incorrect number of arguments.");
        }

        if (!int.TryParse(arguments[2], out int x) || !int.TryParse(arguments[3], out int y))
        {
            throw new ArgumentException("Coordinates are not valid integers.");
        }

        Obstacles.Add(new Guard(x, y));
        Console.WriteLine("Successfully added guard obstacle.");
    }

    /// <summary>
    /// Creates and adds a Fence Obstacle to the current Game. Ensures that the arguments are valid.
    /// </summary>
    /// <param name="arguments">The arguments that this method will analyse before parsing them to instantiate a Fence.</param>
    /// <exception cref="ArgumentException">Incorrect number of or invalid arguments are provided.</exception>
    public void AddFence(string[] arguments)
    {
        if (arguments.Length != 6)
        {
            throw new ArgumentException("Incorrect number of arguments.");
        }

        if (!int.TryParse(arguments[2], out int x) || !int.TryParse(arguments[3], out int y))
        {
            throw new ArgumentException("Coordinates are not valid integers.");
        }

        if (arguments[4] != "EAST" && arguments[4] != "NORTH")
        {
            throw new ArgumentException("Orientation must be 'east' or 'north'.");
        }

        if (!int.TryParse(arguments[5], out int length) || length <= 0)
        {
            throw new ArgumentException("Length must be a valid integer greater than 0.");
        }

        Obstacles.Add(new Fence(x, y, arguments[4], length));
        Console.WriteLine("Successfully added fence obstacle.");
    }

    /// <summary>
    /// Creates and adds a Sensor Obstacle to the current Game. Ensures that the arguments are valid.
    /// </summary>
    /// <param name="arguments">The arguments that this method will analyse before parsing them to instantiate a Sensor.</param>
    /// <exception cref="ArgumentException">Incorrect number of or invalid arguments are provided.</exception>
    public void AddSensor(string[] arguments)
    {
        if (arguments.Length != 5)
        {
            throw new ArgumentException("Incorrect number of arguments.");
        }

        if (!int.TryParse(arguments[2], out int x) || !int.TryParse(arguments[3], out int y))
        {
            throw new ArgumentException("Coordinates are not valid integers.");
        }

        if (!float.TryParse(arguments[4], out float radius) || radius <= 0)
        {
            throw new ArgumentException("Range must be a valid positive number.");
        }

        Obstacles.Add(new Sensor(x, y, radius));
        Console.WriteLine("Successfully added sensor obstacle.");
    }

    /// <summary>
    /// Creates and adds a Camera Obstacle to the current Game. Ensures that the arguments are valid.
    /// </summary>
    /// <param name="arguments">The arguments that this method will analyse before parsing them to instantiate a Camera.</param>
    /// <exception cref="ArgumentException">Incorrect number of or invalid arguments are provided.</exception>
    public void AddCamera(string[] arguments)
    {
        if (arguments.Length != 5)
        {
            throw new ArgumentException("Incorrect number of arguments.");
        }

        if (!int.TryParse(arguments[2], out int x) || !int.TryParse(arguments[3], out int y))
        {
            throw new ArgumentException("Coordinates are not valid integers.");
        }

        string direction = arguments[4]; 

        if (direction != "NORTH" && direction != "EAST" && direction != "SOUTH" && direction != "WEST")
        {
            throw new ArgumentException("Direction must be 'north', 'south', 'east' or 'west'.");
        }

        Obstacles.Add(new Camera(x, y, direction));
        Console.WriteLine("Successfully added camera obstacle.");
    }

    /// <summary>
    /// Checks North East South and West of a point given by the user for obstacles in the game.
    /// </summary>
    /// <param name="arguments">The point that this will be checking around. Given by user.</param>
    /// <exception cref="ArgumentException">Incorrect number of or invalid arguments are provided.</exception>
    public void Check(string[] arguments)
    {
        if (arguments.Length != 3)
        {
            throw new ArgumentException("Incorrect number of arguments.");
        }

        if (!int.TryParse(arguments[1], out int agentX) || !int.TryParse(arguments[2], out int agentY))
        {
            throw new ArgumentException("Coordinates are not valid integers.");
        }

        Check check = new Check(agentX, agentY, Obstacles);
        check.PrintSafeDirections();
    }

    /// <summary>
    /// Creates a Map and prints it.
    /// </summary>
    /// <param name="arguments">The arguments will analysed before parsing them to instantiate and print a Map.</param>
    /// <exception cref="ArgumentException">Incorrect number of or invalid arguments are provided.</exception>
    public void MakeMap(string[] arguments)
    {
        if (arguments.Length != 5)
        {
            throw new ArgumentException("Incorrect number of arguments.");
        }

        if (!int.TryParse(arguments[1], out int southWestX) || !int.TryParse(arguments[2], out int southWestY))
        {
            throw new ArgumentException("Coordinates are not valid integers.");
        }

        if (!int.TryParse(arguments[3], out int width) || width <= 0 || !int.TryParse(arguments[4], out int height) || height <= 0)
        {
            throw new ArgumentException("Width and height must be valid positive integers.");
        }

        Map map = new Map(southWestX, southWestY, width, height, Obstacles);
        Console.WriteLine("Here is a map of Obstacles in the selected region:");
        map.PrintMap();
    }

    /// <summary>
    /// Takes an agent start coordinate and objective coordinate and attempts to find a clear path to that while avoiding obastcles.
    /// </summary>
    /// <param name="arguments">The start coordinates and objective coordinates. Given by the user.</param>
    /// <exception cref="ArgumentException">Incorrect number of or invalid arguments are provided.</exception>
    public void Path(string[] arguments)
    {
        if (arguments.Length != 5)
        {
            throw new ArgumentException("Incorrect number of arguments.");
        }

        if (!int.TryParse(arguments[1], out int agentX) || !int.TryParse(arguments[2], out int agentY))
        {
            throw new ArgumentException("Agent coordinates are not valid integers.");
        }

        if (!int.TryParse(arguments[3], out int objectiveX) || !int.TryParse(arguments[4], out int objectiveY))
        {
            throw new ArgumentException("Width and height must be valid positive integers.");
        }

        if(agentX == objectiveX && agentY == objectiveY)
        {
            Console.WriteLine("Agent, you are already at the objective.");
        }     
        else
        {
            Path path = new Path(agentX, agentY, objectiveX, objectiveY, Obstacles);
            if(path.IsObjectiveBlocked())
            {
                Console.WriteLine("The objective is blocked by an obstacle and cannot be reached.");
            }
            else if(path.IsAgentBlocked())
            {
                Console.WriteLine("Agent, your location is compromised. Abort mission.");
            }
            else
            {
                if (!path.AttemptMission())
                {
                    Console.WriteLine("Agent, there is no safe path to your objective. Abort mission.");
                }
                else {
                    path.PrintMap();
                    Console.WriteLine("The following path will take you to the objective:");
                    PrintPathDirections(path.Directions);
                }
            }
            
        }
    }

    /// <summary>
    /// Takes a list of directions and distances and writes them to console.
    /// </summary>
    /// <param name="directions">JSS: This needs to be documented.</param>
    private static void PrintPathDirections(List<KeyValuePair<Direction, int>> directions)
    {        
        for(int i = 0; i < directions.Count; i++)
        {
            if(directions[i].Value < 0)
            {
                Console.Write($"Head {directions[i].Key} for ");
                Console.Write($"{Convert.ToString(directions[i].Value)}");
                Console.WriteLine($" klick{(directions[i].Value == 1 ? "" : "s")}.");
            }
        }
    }
}