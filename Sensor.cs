namespace Threat_o_tron;

class Sensor : IObstacle
{
    /// <summary>
    /// GameX and GameY is the location of the Centre of the Sensor
    /// </summary>
    private readonly int GameX;
    private readonly int GameY;
    private readonly float Radius;

    /// <summary>
    /// Instatiates a new Sensor
    /// </summary>
    /// <param name="x">The X Coordinate of the centre of the sensor.</param>
    /// <param name="y">The Y Coordinate of the centre of the sensor.</param>
    /// <param name="radius">The radius of the sensor.</param>
    public Sensor(int x, int y, float radius){
        GameX = x;
        GameY = y;
        Radius = radius;
    }

    /// <summary>
    /// Plots a 'S' where the Sensor exists on the provided map.
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    /// <returns></returns>
    public void DrawOnMap(Map map)
    {
        map.FindPointOnMap(GameX, GameY, out int mapX, out int mapY);
        //Always round up as a sliver of sensor will block out the wone square in the game. 
        int roundedRadius = (int)Math.Ceiling(Radius);
        //start at the top of the circle and end at the bottom
        for (int y = mapY - roundedRadius; y < mapY + roundedRadius; y++) 
        {
            ///For each y (row), go through each column, starting at left
            for (int x = mapX - roundedRadius; x < mapX + Radius; x++)
            {
                //Check to see if the current point is in the circle, plot it if it is. 
                if (Math.Pow(x - mapX, 2) + Math.Pow(y - mapY, 2) < Math.Pow(roundedRadius - 0.5, 2))
                {
                    map.CheckAndPlot(x, y, 'S');
                }
                    
            }   
        }
    }
}