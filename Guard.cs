using System.Collections;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

namespace Threat_o_tron;

class Guard : IObstacle{
    /// <summary>
    /// XOrigin and YOrigin is the guard's location in the game
    /// </summary>
    private int XOrigin{get; set;}
    private int YOrigin{get; set;}

    /// <summary>
    /// Instantiates a new Guard.
    /// </summary>
    /// <param name="x">The X Coordinate of the Guard in the game.</param>
    /// <param name="y">The Y Coordinate of the Guard in the game.</param>
    public Guard(int x, int y){
        XOrigin = x;
        YOrigin = y;
    }

    /// <summary>
    /// Plots a 'G' where the guard coordinates are on the provided map
    /// </summary>
    /// <param name="map">The map where the canvas comes from</param>
    /// <returns></returns>
    public void drawOnMap(Map map){
        //where to plot the symbol on the map
        map.FindPointOnMap(XOrigin,YOrigin, out int x, out int y);
        //check to see if the guard will be on the proposed map and plot it if it is.
        map.CheckAndPlot(x, y, 'G');

    }
}