using System.Diagnostics;
using System.Reflection.PortableExecutable;

namespace Threat_o_tron;

public class InvalidNumberOfArgumentsException : Exception
{
    public InvalidNumberOfArgumentsException(string Message) : base (Message)
    {

    }
}

class Game
{
    private readonly List<IObstacle> obstacles = new List<IObstacle>();

    /// <summary>
    /// Instantiates a new blank game.
    /// </summary>
    public Game(){}

    /// <summary>
    /// Used when the user uses the add command. 
    /// Takes the arguments to see what it needs to add.
    /// </summary>
    /// <param name="arguments">The arguments that will be anlaysed and parsed to the specific obstacle that needs to be added.</param>
    public void Add(string[] arguments)
    {
        try
        {
            if(arguments.Length > 1)
            {
                string obstacle = arguments[1];
                //ensure that you have a valid obstacle and valid arguments
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
        catch(ArgumentException exception)
        {   
            Console.WriteLine(exception.Message);
        }
    
    }

    /// <summary>
    /// Creates and adds a Guard Obstacle to the current Game. This method also ensures that the arguments are valid. 
    /// </summary>
    /// <param name="arguments">The arguments that this method will analyse before parsing them to instantiate a Guard.</param>
    public void AddGuard(string[] arguments)
    {
        try
        {   
            if (arguments.Length != 4)
            {
                throw new ArgumentException("Incorrect number of arguments.");
            }

            if (!int.TryParse(arguments[2], out int x) || !int.TryParse(arguments[3], out int y))
            {
                throw new ArgumentException("Coordinates are not valid integers.");
            }
            
            obstacles.Add(new Guard(x, y));
            Console.WriteLine("Successfully added guard obstacle.");
        }
        catch(ArgumentException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    /// <summary>
    /// Creates and adds a Fence Obstacle to the current Game. This method also ensures that the arguments are valid.
    /// </summary>
    /// <param name="arguments">The arguments that this method will analyse before parsing them to instantiate a Fence.</param>
    public void AddFence(string[] arguments)
    {
        try
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

            Fence fence = new Fence(x, y, arguments[4], length);
            obstacles.Add(fence);
            Debug.Assert(obstacles.Contains(fence), "The fence failed to add.");
            Console.WriteLine("Successfully added fence obstacle.");
        }
        catch(ArgumentException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    /// <summary>
    /// Creates and adds a Sensor Obstacle to the current Game. This method also ensures that the arguments are valid.
    /// </summary>
    /// <param name="arguments">The arguments that this method will analyse before parsing them to instantiate a Sensor.</param>
    public void AddSensor(string[] arguments)
    {
        try
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

            Sensor sensor = new Sensor(x, y, radius);
            obstacles.Add(sensor);
            Debug.Assert(obstacles.Contains(sensor), "The sensor failed to add.");
            Console.WriteLine("Successfully added sensor obstacle.");
        }
        catch(ArgumentException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    /// <summary>
    /// Creates and adds a Sensor Obstacle to the current Game. This method also ensures that the arguments are valid.
    /// </summary>
    /// <param name="arguments">The arguments that this method will analyse before parsing them to instantiate a Sensor.</param>
    public void AddCamera(string[] arguments)
    {
        try
        {
            if (arguments.Length != 5)
            {
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
                Console.WriteLine("Direction must be 'north', 'south', 'east' or 'west'.");
                return;
            }

            Camera camera = new Camera(x, y, direction);
            obstacles.Add(camera);
            Debug.Assert(obstacles.Contains(camera), "Camera failed to add to obstacles");
            Console.WriteLine("Successfully added camera obstacle.");
        }
        catch(ArgumentException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    public void Check(string[] arguments)
    {
        try
        {
            if (arguments.Length != 3)
            {
                throw new ArgumentException("Incorrect number of arguments.");
            }

            if (!int.TryParse(arguments[1], out int agentX) || !int.TryParse(arguments[2], out int agentY))
            {
                throw new ArgumentException("Coordinates are not valid integers.");
            }
            
            //creates a new check which creates a new map; subtract 1 from x and y to make the the map's southwest point south west of the agent.
            Check check = new Check(agentX-1, agentY-1, obstacles);
            check.PrintSafeDirections();
        }
        catch(ArgumentException exception)
        {
            Console.WriteLine(exception.Message);
        }    
    }

    /// <summary>
    /// Creates a map with the given arguments.
    /// </summary>
    /// <param name="arguments">This will be used to instantiate the map, given it is valid.</param>
    public void MakeMap(string[] arguments)
    {
        try
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

            Map map = new Map(southWestX, southWestY, width, height, obstacles);
            Console.WriteLine("Here is a map of obstacles in the selected region:");
            map.PrintMap();
        }
        catch(ArgumentException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}