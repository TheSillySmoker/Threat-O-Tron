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
    /// A way to write a string and read from CLI with one method
    /// </summary>
    /// <param name="inputMessage">What do you want the user to read before they input text</param>
    /// <returns>User input string or a defualt "EMPTY INPUT" if the user does not enter anything.</returns>
    public static string ReadString(string inputMessage = ""){
        Console.WriteLine(inputMessage);
        string? output = Console.ReadLine();
        if (output != null){
            return output;
        }
        else{
            return "EMPTY INPUT";
        }
    }

    /// <summary>
    /// Starts a game by welcoming the user and asking them to enter a command; filters the first argument given by the user.
    /// </summary>
    public static void StartGame(){
        bool exiting = false;
        Game game = new Game();
        Console.WriteLine("Welcome to the Threat-o-tron 9000 Obstacle Avoidance System.\n");
        printValidCommands();
        //get a valid input
        do{
            string inputMessage = ReadString("Enter Command:");
            //Make all inputs from the user not case sensitive
            string[] inputMessageArguments = inputMessage.ToUpper().Split(' ');
            //Filter the first argument/command given by the user
            switch (inputMessageArguments[0]){
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
                //add path
                Console.WriteLine("This has not been built yet. \nComing soon...");
                break;
                case "EXIT":
                    Console.WriteLine("Thank you for using the Threat-o-tron 9000.");
                    exiting = true;
                    break;
                case "HELP":
                    printValidCommands();
                break;
                default:
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
    public static void printValidCommands(){
        Console.WriteLine("Valid commands are:\nadd guard <x> <y>: registers a guard obstacle\nadd fence <x> <y> <orientation> <length>: registers a fence obstacle. Orientation must be 'east' or 'north'.\nadd sensor <x> <y> <radius>: registers a sensor obstacle\nadd camera <x> <y> <direction>: registers a camera obstacle. Direction must be 'north', 'south', 'east' or 'west'.\ncheck <x> <y>: checks whether a location and its surroundings are safe\nmap <x> <y> <width> <height>: draws a text-based map of registered obstacles\npath <agent x> <agent y> <objective x> <objective y>: finds a path free of obstacles\nhelp: displays this help message\nexit: closes this program\n");
    }
}
