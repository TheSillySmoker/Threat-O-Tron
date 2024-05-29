using System;

namespace Threat_o_tron;

class Sensor : IObstacle
{
    /// <summary>
    /// GameX and GameY is the location of the centre of the Sensor.
    /// Radius will be the radius of the sensor.
    /// </summary>
    private readonly int GameX;
    private readonly int GameY;
    private readonly float Radius;

    /// <summary>
    /// Instatiates a new Sensor.
    /// </summary>
    /// <param name="y">The Y coordinate of the centre of the Sensor in the Game.</param>
    /// <param name="x">The X coordinate of the centre of the Sensor in the Game.</param>
    /// <param name="radius">The radius of the Sensor.</param>
    public Sensor(int x, int y, float radius){
        GameX = x;
        GameY = y;
        Radius = radius;
    }

    /// <summary>
    /// Plots an 'S' where the Sensor exists on the provided Map.
    /// </summary>
    /// <param name="map">The Map that will be drawn on.</param>
    public void DrawOnMap(Map map)
    {
        // Establish point for plotting the symbol on the Map, this will be the Map's coordinates, not the Game's.
        map.GetMapCoordinates(GameX, GameY, out int mapX, out int mapY);

        // Always round up as a sliver of sensor will block out a whole square in the game. 
        int roundedRadius = (int)Math.Ceiling(Radius);

        // Start at the top of the circle and end at the bottom.
        for (int y = mapY - roundedRadius; y < mapY + roundedRadius; y++) 
        {
            /// For each y (row), go through each column, starting at left.
            for (int x = mapX - roundedRadius; x < mapX + Radius; x++)
            {
                // Check to see if the current point is in the circle and plot it, if it is. 
                if (Math.Pow(x - mapX, 2) + Math.Pow(y - mapY, 2) < Math.Pow(roundedRadius - 0.5, 2))
                {
                    map.CheckAndPlot(x, y, 'S');
                }
            }   
        }
    }
}