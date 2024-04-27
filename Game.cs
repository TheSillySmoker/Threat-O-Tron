using System.Diagnostics;
using System.Reflection.PortableExecutable;

namespace Threat_o_tron;

class Game{

    public readonly List<IObstacle> obstacles = new List<IObstacle>();

    /// <summary>
    /// Instantiates a new blank game.
    /// </summary>
    public Game(){}

    /// <summary>
    /// Used when the user uses the add command. This will take the arguments to see what it needs to add.
    /// </summary>
    /// <param name="arguments">The arguments that will be anlaysed and parsed to the specific obstacle that needs to be added.</param>
    public void Add(string[] arguments){
        if(arguments.Length > 1){
            string obstacle = arguments[1];
            //ensure that you have a valid obstacle and valid arguements
            switch (obstacle){
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
                Console.WriteLine("Invalid obstacle type.");
                break;  
            }
        }
        else{
            Console.WriteLine("Invalid obstacle type.");
        }      
    }

    /// <summary>
    /// Creates and adds a Guard Obstacle to the current Game. This method also ensures that the arguments are valid. 
    /// </summary>
    /// <param name="arguments">The arguemnts that this method will analyse before parsing them to instantiate a Guard.</param>
    public void AddGuard(string[] arguments)
    {
        if (arguments.Length != 4){
            Console.WriteLine("Incorrect number of arguments.");
            return;
        }

        if (!int.TryParse(arguments[2], out int x) || !int.TryParse(arguments[3], out int y)){
            Console.WriteLine("Coordinates are not valid integers.");
            return;
        }

        Guard guard = new Guard(x, y);
        obstacles.Add(guard);
        Debug.Assert(obstacles.Contains(guard), "The guard failed to add.");
        Console.WriteLine("Successfully added guard obstacle.");
    }

    /// <summary>
    /// Creates and adds a Fence Obstacle to the current Game. This method also ensures that the arguments are valid.
    /// </summary>
    /// <param name="arguments">The arguemnts that this method will analyse before parsing them to instantiate a Fence.</param>
    public void AddFence(string[] arguments){
    if (arguments.Length != 6){
        Console.WriteLine("Incorrect number of arguments.");
        return;
    }

    if (!int.TryParse(arguments[2], out int x) || !int.TryParse(arguments[3], out int y)){
        Console.WriteLine("Coordinates are not valid integers.");
        return;
    }

    if (arguments[4] != "EAST" && arguments[4] != "NORTH"){
        Console.WriteLine("Orientation must be 'east' or 'north'.");
        return;
    }

    if (!int.TryParse(arguments[5], out int length) || length <= 0){
        Console.WriteLine("Length must be a valid integer greater than 0.");
        return;
    }

    Fence fence = new Fence(x, y, arguments[4], length);
    obstacles.Add(fence);
    Debug.Assert(obstacles.Contains(fence), "The fence failed to add.");
    Console.WriteLine("Successfully added fence obstacle.");
}

    /// <summary>
    /// Creates and adds a Sensor Obstacle to the current Game. This method also ensures that the arguments are valid.
    /// </summary>
    /// <param name="arguments">The arguemnts that this method will analyse before parsing them to instantiate a Sensor.</param>
    public void AddSensor(string[] arguments)
    {
        if (arguments.Length != 5){
            Console.WriteLine("Incorrect number of arguments.");
            return;
        }

        if (!int.TryParse(arguments[2], out int x) || !int.TryParse(arguments[3], out int y)){
            Console.WriteLine("Coordinates are not valid integers.");
            return;
        }

        if (!float.TryParse(arguments[4], out float radius) || radius <= 0){
            Console.WriteLine("Range must be a valid positive number.");
            return;
        }

        Sensor sensor = new Sensor(x, y, radius);
        obstacles.Add(sensor);
        Debug.Assert(obstacles.Contains(sensor), "The sensor failed to add.");
        Console.WriteLine("Successfully added sensor obstacle.");
    }

    /// <summary>
    /// Creates and adds a Sensor Obstacle to the current Game. This method also ensures that the arguments are valid.
    /// </summary>
    /// <param name="arguments">The arguemnts that this method will analyse before parsing them to instantiate a Sensor.</param>
    public void AddCamera(string[] arguments)
    {
        if (arguments.Length != 5){
            Console.WriteLine("Incorrect number of arguments.");
            return;
        }

        if (!int.TryParse(arguments[2], out int x) || !int.TryParse(arguments[3], out int y)){
            Console.WriteLine("Coordinates are not valid integers.");
            return;
        }

        string direction = arguments[4].ToUpper(); // Convert direction to uppercase for case-insensitive comparison

        if (direction != "NORTH" && direction != "EAST" && direction != "SOUTH" && direction != "WEST"){
            Console.WriteLine("Direction must be 'north', 'south', 'east' or 'west'.");
            return;
        }

        Camera camera = new Camera(x, y, direction);
        obstacles.Add(camera);
        Debug.Assert(obstacles.Contains(camera), "Camera failed to add to obstacles");
        Console.WriteLine("Successfully added camera obstacle.");
    }

    public void Check(string[] arguments){
        if (arguments.Length != 3){
            Console.WriteLine("Incorrect number of arguments.");
            return;
        }

        if (!int.TryParse(arguments[1], out int agentX) || !int.TryParse(arguments[2], out int agentY)){
            Console.WriteLine("Coordinates are not valid integers.");
            return;
        }

        Check check = new Check(agentX-1, agentY-1, this);
        check.PrintSafeDirections();
        
    }

    /// <summary>
    /// Creates a map with the given arguemnts.
    /// </summary>
    /// <param name="arguments">This will be used to instantiate the map, given it is valid.</param>
    public void MakeMap(string[] arguments)
    {
        if (arguments.Length != 5){
            Console.WriteLine("Incorrect number of arguments.");
            return;
        }

        if (!int.TryParse(arguments[1], out int southWestX) || !int.TryParse(arguments[2], out int southWestY)){
            Console.WriteLine("Coordinates are not valid integers.");
            return;
        }

        if (!int.TryParse(arguments[3], out int width) || width <= 0 || !int.TryParse(arguments[4], out int height) || height <= 0){
            Console.WriteLine("Width and height must be valid positive integers.");
            return;
        }

        Map map = new Map(southWestX, southWestY, width, height, obstacles);
        Console.WriteLine("Here is a map of obstacles in the selected region:");
        map.PrintMap();
    }
}