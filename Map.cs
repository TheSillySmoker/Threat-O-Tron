using System;
using System.Collections.Generic;

namespace Threat_o_tron;

class Map
{
    protected char[,] Canvas;
    private readonly int SouthWestX;
    private readonly int SouthWestY;
    public int Width{get;}          
    public int Height{get;}

    /// <summary>
    /// Instantiates a new map based on the size given by the user. Any obstacles in the given obstacles list will be drawn on the map.
    /// </summary>
    /// <param name="southWestX">The game's X coordinate for the most SouthWest point of the map.</param>
    /// <param name="southWestY">The game's Y coordinate for the most SouthWest point of the map.</param>
    /// <param name="width">The width of the map.</param>
    /// <param name="height">The height of the map.</param>
    public Map(int southWestX, int southWestY, int width, int height, List<IObstacle> obstacles)
    {
        SouthWestX = southWestX;
        SouthWestY = southWestY;
        Width = width;
        Height = height;
        Canvas = new char[Height,Width];

        // Populate all spots in the canvas with '.'.
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Canvas[y, x] = '.';
            }
        }

        // Draw each obstacle that exists in the game on this map.
        foreach (IObstacle obstacle in obstacles)
        {
            obstacle.DrawOnMap(this);
        }
    }

    /// <summary>
    /// Prints out the map to the user.
    /// </summary>
    public void PrintMap()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Console.Write(Canvas[y, x]);
            }
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Checks to see if the given point exists on this map.
    /// </summary>
    /// <param name="mapX">X coordinate of Obstacle in the game.</param>
    /// <param name="mapY">Y coordinate of Obstacle in the game.</param>
    /// <returns>True if the map contains the given point.</returns>
    protected bool ContainsPoint(int mapX, int mapY)
    {
        return mapX <= Width - 1 && mapX >= 0 && mapY <= Height - 1 && mapY >= 0;
    }

    /// <summary>
    /// Finds where the object's starting coordinates for the map, not the game's coordinates!.
    /// </summary>
    /// <param name="gameX">X coordinate of the object in the game.</param>
    /// <param name="gameY">Y coordinate of the object in the game.</param>
    /// <param name="mapX">The start X coordinate of the object on the map.</param>
    /// <param name="mapY">The start Y coordinate of the object on the map.</param>
    public void GetMapCoordinates(int gameX, int gameY, out int mapX, out int mapY)
    {
        mapY = Height - 1 - (gameY - SouthWestY);
        mapX = gameX - SouthWestX;
    }

    public void GetGameCoordinates(int mapX, int mapY, out int gameX, out int gameY)
    {
        gameX = Height - 1 - mapX + SouthWestY;
        gameY = mapY + SouthWestY;
    }

    /// <summary>
    /// Checks to see if the given point will be on the map and plots it, if it is. This takes canvas coordinates, not Game coordinates.
    /// </summary>
    /// <param name="mapX">X coordinate on the Map's canvas that will be plotted.</param>
    /// <param name="mapY">Y coordinate on the Map's canvas that will be plotted.</param>
    /// <param name="character">The character that will be plotted on the map.</param>
    public void CheckAndPlot(int mapX, int mapY, char character)
    {
        if (ContainsPoint(mapX, mapY))
        {
            Canvas[mapY,mapX] = character;  
        } 
    }
}

