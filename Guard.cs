using System.Collections;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

namespace Threat_o_tron;

class Guard : IObstacle
{
    /// <summary>
    /// GameX and GameY is the guard's location in the game
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
    /// Plots a 'G' where the guard coordinates are on the provided map.
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    /// <returns></returns>
    public void DrawOnMap(Map map){
        //where to plot the symbol on the map
        map.FindPointOnMap(GameX, GameY, out int MapX, out int MapY);
        //check to see if the guard will be on the proposed map and plot it if it is.
        map.CheckAndPlot(MapX, MapY, 'G');

    }
}