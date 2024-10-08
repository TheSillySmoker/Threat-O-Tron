using System.Collections;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

namespace Threat_o_tron;

class Guard : IObstacle
{
    /// <summary>
    /// GameX and GameY is the Guard's location in the game.
    /// </summary>
    private readonly int GameX;
    private readonly int GameY;

    /// <summary>
    /// Instantiates a new Guard.
    /// </summary>
    /// <param name="x">The X Coordinate of the Guard in the game.</param>
    /// <param name="y">The Y Coordinate of the Guard in the game.</param>
    public Guard(int x, int y){
        GameX = x;
        GameY = y;
    }

    /// <summary>
    /// Plots a 'G' where the Guard's coordinates are on the provided map.
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    public void DrawOnMap(Map map){
        // Establish point for plotting the symbol on the Map, this will be the Map's coordinates, not the Game's.
        map.GetMapCoordinates(GameX, GameY, out int mapX, out int mapY);
        map.CheckAndPlot(mapX, mapY, 'G');
    }
}