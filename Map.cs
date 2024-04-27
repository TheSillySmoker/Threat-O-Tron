using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Threat_o_tron;

class Map{

    public char[,] Canvas;
    private int SouthWestX{get; set;}          
    private int SouthWestY{get; set;} 
    public int SizeX{get;private set;}          
    public int SizeY{get;private set;}
    


    /// <summary>
    /// Instantiates a new map based on the size given by the user. Any obstacles in the given obstacles list will be drawn on the map.
    /// </summary>
    /// <param name="southWestX">The game's X coordinate for the most SouthWest point of the map.</param>
    /// <param name="southWestY">The game's Y coordinate for the most SouthWest point of the map.</param>
    /// <param name="sizeX">The width of the map.</param>
    /// <param name="sizeY">The height of the map.</param>
    public Map(int southWestX, int southWestY, int sizeX, int sizeY, List<IObstacle> obstacles){
        SouthWestX = southWestX;
        SouthWestY = southWestY;
        SizeX = sizeX;
        SizeY = sizeY;
        Canvas = new char[SizeY,SizeX];

        PopulateCanvas('.');

        foreach (IObstacle obstacle in obstacles){
            obstacle.drawOnMap(this);
        }
    }

    /// <summary>
    /// Populates the Canvas array property with a specific character.
    /// </summary>
    /// <param name="character">The character that will fill the blank Canvas.</param>
    private void PopulateCanvas(char character){
        for (int i = 0; i < SizeY; i++)
        {
            for (int ii = 0; ii < SizeX; ii++)
            {
                Canvas[i, ii] = character;
            }
        }
    }

    /// <summary>
    /// Prints out the map to the user.
    /// </summary>
    public void PrintMap()
    {
        for (int i = 0; i < SizeY; i++)
        {
            for (int j = 0; j < SizeX; j++)
            {
                Console.Write(Canvas[i, j]);
            }
            Console.WriteLine();
        }
    }
    /// <summary>
    /// Checks to see if the given point exists on this map.
    /// </summary>
    /// <param name="xOrigin">X corrdinate of Obstacle in the game.</param>
    /// <param name="yOrigin">Y coordinate of Obstacle in the game.</param>
    /// <returns>True or false whether the map contains the given point.</returns>
    public bool ContainsPoint(int xOrigin, int yOrigin){
        if(xOrigin <= SizeX-1 && xOrigin >= 0 && yOrigin <= SizeY-1 && yOrigin >= 0){
            return true;
        }
        return false;
    }

    /// <summary>
    /// Finds where the obstacle's starting coordinates for the map, not the game's coordinates!.
    /// </summary>
    /// <param name="xOrigin">X corrdinate of Obstacle in the game.</param>
    /// <param name="yOrigin">Y coordinate of Obstacle in the game.</param>
    /// <param name="mapX">The start X coordinate of the obstacle on the map.</param>
    /// <param name="mapY">The start Y coordinate of the obstacle on the map.</param>
    public void FindPointOnMap(int xOriginOfObstacle, int yOriginOfObstacle, out int mapX, out int mapY){
        mapY = SizeY-1 - (yOriginOfObstacle - SouthWestY);
        mapX = xOriginOfObstacle - SouthWestX ;
    }

    /// <summary>
    /// Checks to see if the point given will be on the map and plots it if it is. This takes canvas Coordinates, not game coordinates.
    /// </summary>
    /// <param name="x">X coordinate on the map's canvas that will be plotted.</param>
    /// <param name="y">Y coordinate on the map's canvas that will be plotted.</param>
    /// <param name="character">The character that will be plotted on the map.</param>
    public void CheckAndPlot(int x, int y, char character){
    if (ContainsPoint(x,y)) Canvas[y,x] = character;
    }
}

