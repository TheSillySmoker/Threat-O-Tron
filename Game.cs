using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Threat_o_tron;

// JSS CodeReview: Have you considered implementing an enum for north,south,east,west directions? I can help you with that.
class Game
{
    public enum Direction {
        North,
        East,
        West,
        South,
        Unknown
    }

    // JSS CodeReview: It would be more correct to instantiate this list in the Game constructor.
    private readonly List<IObstacle> obstacles = new List<IObstacle>();

    /// <summary>
    /// Instantiates a new blank game.
    /// </summary>
    public Game(){}

    /// <summary>
    /// Used when the user uses the add command. 
    /// Takes the arguments to see what it needs to add.
    /// </summary>
    /// <param name="arguments">The arguments that will be analysed and parsed to the specific obstacle that needs to be added.</param>
    /// JSS CodeReview: It is worthwhile documenting what exceptions are thrown and why.
    ///                 I've added this one. You should add the rest where they are needed.
    /// <exception cref="ArgumentException">Incorrect number of or invalid arguments are provided.</exception>
    public void Add(string[] arguments)
    {
        try
        {
            if(arguments.Length > 1)
            {
                string obstacle = arguments[1];
                // Ensure that the user has a valid obstacle and valid arguments.
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
                // JSS CodeReview: Is this error message more appropriate?
                throw new ArgumentException("Please provide an obstacle type.");
            }  
        }
        // JSS CodeReview: It should be the calling code's responsibility to catch an error and print the message to Console.
        //                 Can you move this catch clause into the Program code?
        catch(ArgumentException exception)
        {   
            Console.WriteLine(exception.Message);
        }
    
    }

    /// <summary>
    /// Creates and adds a Guard Obstacle to the current Game. This method also ensures that the arguments are valid. 
    /// </summary>
    /// <param name="arguments">The arguments that this method will analyse before parsing them to instantiate a Guard.</param>
    /// JSS CodeReview: Add exception documentation here.
    public void AddGuard(string[] arguments)
    {
        try
        {   
            if (arguments.Length != 4)
            {
                // JSS CodeReview: You could also print out the expected command format?
                //                 i.e. "Expected arguments: add guard <x> <y>"
                throw new ArgumentException("Incorrect number of arguments.");
            }

            if (!int.TryParse(arguments[2], out int x) || !int.TryParse(arguments[3], out int y))
            {
                throw new ArgumentException("Coordinates are not valid integers.");
            }

            obstacles.Add(new Guard(x, y));
            Console.WriteLine("Successfully added guard obstacle.");
        }
        // JSS CodeReview: It should be the calling code's responsibility to catch an error and print the message to Console.
        //                 If you add a catch clause to the Program class' add functionality, it will be handled there.
        //                 This way, you can also remove duplicated catch clauses.
        catch(ArgumentException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    /// <summary>
    /// Creates and adds a Fence Obstacle to the current Game. This method also ensures that the arguments are valid.
    /// </summary>
    /// <param name="arguments">The arguments that this method will analyse before parsing them to instantiate a Fence.</param>
    /// JSS CodeReview: Add exception documentation here.
    public void AddFence(string[] arguments)
    {
        try
        {
            if (arguments.Length != 6)
            {
                // JSS CodeReview: You could also print out the expected command format?
                //                 i.e. "Expected arguments: add fence <x> <y> <orientation> <length>"
                throw new ArgumentException("Incorrect number of arguments.");
            }

            if (!int.TryParse(arguments[2], out int x) || !int.TryParse(arguments[3], out int y))
            {
                throw new ArgumentException("Coordinates are not valid integers.");
            }

            if (arguments[4] != "EAST" && arguments[4] != "NORTH")
            {
                // JSS CodeReview: If you wanted to use double quotes instead of single quotes, try \".
                throw new ArgumentException("Orientation must be 'east' or 'north'.");
            }

            if (!int.TryParse(arguments[5], out int length) || length <= 0)
            {
                throw new ArgumentException("Length must be a valid integer greater than 0.");
            }

            Fence fence = new Fence(x, y, arguments[4], length);
            obstacles.Add(fence);
            // JSS CodeReview: Redundant assert.
            Debug.Assert(obstacles.Contains(fence), "The fence failed to add.");
            Console.WriteLine("Successfully added fence obstacle.");
        }
        // JSS CodeReview: It should be the calling code's responsibility to catch an error and print the message to Console.
        //                 If you add a catch clause to the Program class' add functionality, it will be handled there.
        //                 This way, you can also remove duplicated catch clauses.
        catch(ArgumentException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    /// <summary>
    /// Creates and adds a Sensor Obstacle to the current Game. This method also ensures that the arguments are valid.
    /// </summary>
    /// <param name="arguments">The arguments that this method will analyse before parsing them to instantiate a Sensor.</param>
    /// JSS CodeReview: Add exception documentation here.
    public void AddSensor(string[] arguments)
    {
        try
        {
            if (arguments.Length != 5)
            {
                // JSS CodeReview: You could also print out the expected command format?
                //                 i.e. "Expected arguments: add sensor <x> <y> <radius>"
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

            Sensor sensor = new Sensor(x, y, radius);
            obstacles.Add(sensor);
            // JSS CodeReview: Redundant assert.
            Debug.Assert(obstacles.Contains(sensor), "The sensor failed to add.");
            Console.WriteLine("Successfully added sensor obstacle.");
        }
        // JSS CodeReview: It should be the calling code's responsibility to catch an error and print the message to Console.
        //                 If you add a catch clause to the Program class' add functionality, it will be handled there.
        //                 This way, you can also remove duplicated catch clauses.
        catch(ArgumentException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    /// <summary>
    /// Creates and adds a Sensor Obstacle to the current Game. This method also ensures that the arguments are valid.
    /// </summary>
    /// <param name="arguments">The arguments that this method will analyse before parsing them to instantiate a Sensor.</param>
    /// JSS CodeReview: Add exception documentation here.
    public void AddCamera(string[] arguments)
    {
        try
        {
            if (arguments.Length != 5)
            {
                // JSS CodeReview: You could also print out the expected command format?
                //                 i.e. "Expected arguments: add camera <x> <y> <direction>"
                throw new ArgumentException("Incorrect number of arguments.");
            }

            if (!int.TryParse(arguments[2], out int x) || !int.TryParse(arguments[3], out int y))
            {
                throw new ArgumentException("Coordinates are not valid integers.");
            }
            // Convert direction to uppercase for case-insensitive comparison
            string direction = arguments[4].ToUpper(); 

            if (direction != "NORTH" && direction != "EAST" && direction != "SOUTH" && direction != "WEST")
            {
                // JSS CodeReview: Can this be an exception too?
                Console.WriteLine("Direction must be 'north', 'south', 'east' or 'west'.");
                return;
            }

            Camera camera = new Camera(x, y, direction);
            obstacles.Add(camera);
            // JSS CodeReview: Redundant assert.
            Debug.Assert(obstacles.Contains(camera), "Camera failed to add to obstacles");
            Console.WriteLine("Successfully added camera obstacle.");
        }
        // JSS CodeReview: It should be the calling code's responsibility to catch an error and print the message to Console.
        //                 If you add a catch clause to the Program class' add functionality, it will be handled there.
        //                 This way, you can also remove duplicated catch clauses.
        catch(ArgumentException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    // JSS CodeReview: Add documentation.
    public void Check(string[] arguments)
    {
        try
        {
            if (arguments.Length != 3)
            {
                // JSS CodeReview: You could also print out the expected command format?
                //                 i.e. "Expected arguments: check <x> <y>"
                throw new ArgumentException("Incorrect number of arguments.");
            }

            if (!int.TryParse(arguments[1], out int agentX) || !int.TryParse(arguments[2], out int agentY))
            {
                throw new ArgumentException("Coordinates are not valid integers.");
            }
            
            // JSS CodeReview: Is this comment still valid?
            // Creates a new check which creates a new map; subtract 1 from x and y to make the the map's southwest point south west of the agent.
            // JSS CodeReview: You can also do this!
            Check check = new(agentX, agentY, obstacles);
            check.PrintSafeDirections();
        }
        // JSS CodeReview: It should be the calling code's responsibility to catch an error and print the message to Console.
        //                 Can you move this catch clause into the Program code?
        catch(ArgumentException exception)
        {
            Console.WriteLine(exception.Message);
        }    
    }

    // JSS CodeReview: Add documentation.
    public void Path(string[] arguments)
    {
        try
        {
            if (arguments.Length != 5)
            {
                // JSS CodeReview: You could also print out the expected command format?
                //                 i.e. "Expected arguments: path <agent x> <agent y> <objective x> <objective y>"
                throw new ArgumentException("Incorrect number of arguments.");
            }

            if (!int.TryParse(arguments[1], out int agentX) || !int.TryParse(arguments[2], out int agentY))
            {
                throw new ArgumentException("Agent coordinates are not valid integers.");
            }

            if (!int.TryParse(arguments[3], out int objectiveX) || !int.TryParse(arguments[4], out int objectiveY))
            {
                // JSS CodeReview: Do they actually need to be positive?
                throw new ArgumentException("Width and height must be valid positive integers.");
            }

            if(agentX == objectiveX && agentY == objectiveY)
            {
                Console.WriteLine("Agent, you are already at the objective.");
            }  
            else
            {
                JJPath path = new JJPath(agentX, agentY, objectiveX, objectiveY, obstacles);
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
                    if (!path.FindPath())
                    {
                        Console.WriteLine("Agent, there is no safe path to your objective. Abort mission.");
                    }
                    else {
                        path.PrintMap();
                        Console.WriteLine("The following path will take you to the objective:");
                        PrintPathDirections(path.GetDirections());
                    }
                }
                
            }
        }
        // JSS CodeReview: It should be the calling code's responsibility to catch an error and print the message to Console.
        //                 Can you move this catch clause into the Program code?
        catch(ArgumentException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    /// <summary>
    /// Takes a list of directions and distances and writes them to console.
    /// </summary>
    /// <param name="directions">JSS: This needs to be documented.</param>
    // JSS CodeReview: Does this need to be static?
    private static void PrintPathDirections(List<KeyValuePair<Direction, int>> directions)
    {        
        // JSS CodeReview: This wasn't printing the last direction. I've fixed it.
        for(int i = 0; i < directions.Count; i++)
        {
            Console.Write($"Head {directions[i].Key} for ");
            Console.Write($"{Convert.ToString(directions[i].Value)}");
            // JSS CodeReview: How does this look?
            Console.WriteLine($" klick{(directions[i].Value == 1 ? "" : "s")}.");
        }
    }

    /// <summary>
    /// Creates a Map and prints it.
    /// </summary>
    /// <param name="arguments">The arguments that this method will analyse before parsing them to instantiate and print a Map.</param>
    /// JSS CodeReview: Add exception documentation here.
    public void MakeMap(string[] arguments)
    {
        try
        {
            if (arguments.Length != 5)
            {
                // JSS CodeReview: You could also print out the expected command format?
                //                 i.e. "Expected arguments: map <x> <y> <width> <height>"
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

            Map map = new Map(southWestX, southWestY, width, height, obstacles);
            Console.WriteLine("Here is a map of obstacles in the selected region:");
            map.PrintMap();
        }
        // JSS CodeReview: It should be the calling code's responsibility to catch an error and print the message to Console.
        //                 Can you move this catch clause into the Program code?
        catch(ArgumentException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}