using System;

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
    /// <param name="inputMessage">The prompt that will be shown to the user.</param>
    /// <returns>User input string or a default "EMPTY INPUT" if the user does not enter anything.</returns>
    public static string Prompt(string inputMessage = "")
    {
        Console.WriteLine(inputMessage);
        // JSS CodeReview: Does this look cleaner? Change it back if you don't like it.
        return Console.ReadLine() ?? " ";
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
            // Make all inputs from the user not case sensitive.
            string[] inputMessageArguments = inputMessage.ToUpper().Split(' ');
            // Filter the first argument/command given by the user.
            try
            {
                switch (inputMessageArguments[0])
                {
                    case "ADD":
                        // JSS CodeReview: See comments in Game class. Move catch clause here.
                        game.Add(inputMessageArguments); 
                        break;
                    case "CHECK":
                        // JSS CodeReview: See comment in Game class. Move catch clause here.
                        game.Check(inputMessageArguments);
                        break;
                    case "MAP":
                        // JSS CodeReview: See comment in Game class. Move catch clause here.
                        game.MakeMap(inputMessageArguments);
                        break;
                    case "PATH":
                        // JSS CodeReview: See comment in Game class. Move catch clause here.
                        game.Path(inputMessageArguments);
                        break;
                    case "HELP":
                        PrintValidCommands();
                        break;
                    case "EXIT":
                        Console.WriteLine("Thank you for using the Threat-o-tron 9000.");
                        exiting = true;
                        break;
                    default:
                        // Instead of getting the uppercase version of the input, this line will get the exact input to give back to the user. 
                        Console.WriteLine($"Invalid option: {inputMessage.Split(' ')[0]}\nType 'help' to see a list of commands.");
                        break;
                }
            }
            catch(ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
        // Repeat if the user does not wish to exit.
        while(!exiting);
    }

    /// <summary>
    /// Prints the valid commands a user can input in the command line interface.
    /// </summary>
    public static void PrintValidCommands()
    {
        Console.WriteLine(
            "Valid commands are:\n" +
            "add guard <x> <y>: registers a guard obstacle\n" +
            "add fence <x> <y> <orientation> <length>: registers a fence obstacle. Orientation must be 'east' or 'north'.\n" +
            "add sensor <x> <y> <radius>: registers a sensor obstacle\n" +
            "add camera <x> <y> <direction>: registers a camera obstacle. Direction must be 'north', 'south', 'east' or 'west'.\n" +
            "check <x> <y>: checks whether a location and its surroundings are safe\n" +
            "map <x> <y> <width> <height>: draws a text-based map of registered obstacles\n" +
            "path <agent x> <agent y> <objective x> <objective y>: finds a path free of obstacles\n" +
            "help: displays this help message\n" +
            "exit: closes this program\n"
        );
    }
}
