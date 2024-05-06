using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace Threat_o_tron;

static class Program
{
    static void Main(string[] args)
    {
        StartGame();
    }

    /// <summary>
    /// Writes a string/prompt to the CLI, then reads a string afterward.
    /// </summary>
    /// <param name="inputMessage">The promt that will be shown to the user</param>
    /// <returns>User input string or a default "EMPTY INPUT" if the user does not enter anything.</returns>
    public static string Prompt(string inputMessage = "")
    {
        Console.WriteLine(inputMessage);
        string output = Console.ReadLine() ?? " ";
        return output;
    }

    /// <summary>
    /// Starts a game by welcoming the user and asking them to enter a command; filters the first argument given by the user.
    /// </summary>
    public static void StartGame()
    {
        bool exiting = false;
        Game game = new();
        Console.WriteLine("Welcome to the Threat-o-tron 9000 Obstacle Avoidance System.\n");
        PrintValidCommands();
        do
        {
            string inputMessage = Prompt("Enter Command:");
            //Make all inputs from the user not case sensitive
            string[] inputMessageArguments = inputMessage.ToUpper().Split(' ');
            //Filter the first argument/command given by the user
            switch (inputMessageArguments[0])
            {
                case "ADD":
                    game.Add(inputMessageArguments); 
                    break;
                case "CHECK":
                    game.Check(inputMessageArguments);
                    break;
                case "MAP":
                    game.MakeMap(inputMessageArguments);
                    break;
                case "PATH":
                //TODO: add path when built
                    Console.WriteLine("This has not been built yet. \nComing soon...");
                    break;
                case "EXIT":
                    Console.WriteLine("Thank you for using the Threat-o-tron 9000.");
                    exiting = true;
                    break;
                case "HELP":
                    PrintValidCommands();
                    break;
                default:
                    //Instead of getting the uppercase version of the input, this line will get the exact input to give back to the user. 
                    Console.WriteLine($"Invalid option: {inputMessage.Split(' ')[0]}\nType 'help' to see a list of commands.");
                    break;
            }
        }
        //repeat if the user does not wish to exit
        while(!exiting);
    }

    /// <summary>
    /// Writes the valid commands a user can input in the command line interface.
    /// </summary>
    public static void PrintValidCommands()
    {
        Console.WriteLine("Valid commands are:\nadd guard <x> <y>: registers a guard obstacle");
        Console.WriteLine("add fence <x> <y> <orientation> <length>: registers a fence obstacle. Orientation must be 'east' or 'north'.");
        Console.WriteLine("add sensor <x> <y> <radius>: registers a sensor obstacle");
        Console.WriteLine("add camera <x> <y> <direction>: registers a camera obstacle. Direction must be 'north', 'south', 'east' or 'west'.");
        Console.WriteLine("check <x> <y>: checks whether a location and its surroundings are safe");
        Console.WriteLine("map <x> <y> <width> <height>: draws a text-based map of registered obstacles");
        Console.WriteLine("path <agent x> <agent y> <objective x> <objective y>: finds a path free of obstacles");
        Console.WriteLine("help: displays this help message\nexit: closes this program\n");
    }
}
