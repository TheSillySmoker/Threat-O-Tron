namespace Threat_o_tron;
class Fence : IObstacle{
    /// <summary>
    /// StartGameX and StartGameY is the start point of the Fence in the game.
    /// Length is how many klicks the Fence will span. 
    /// Orientation is the direction the fence will span.
    /// </summary>
    private readonly int StartGameX;
    private readonly int StartGameY;
    private readonly int Length;
    private readonly string Orientation;
    
    /// <summary>
    /// Creates a new Fence with origin coordinates, orientation and length.
    /// </summary>
    /// <param name="x">The X coordinate of the Fence in the game.</param>
    /// <param name="y">The Y coordinate of the Fence in the game.</param>
    /// <param name="orientation">Which way the Fence is spanning.</param>
    /// <param name="length">The length of the Fence in klicks.</param>
    public Fence(int x, int y, string orientation, int length)
    {
        StartGameX = x;
        StartGameY = y;
        Length = length;
        Orientation = orientation;
    }
    
    /// <summary>
    /// Plots an 'F' where the Fence exists on the provided Map.
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    public void DrawOnMap(Map map)
    {
        // Establish startpoint for plotting the symbol on the Map; this will be the Map's canvas coordinates, not the Game's.
        map.GetMapCoordinates(StartGameX, StartGameY, out int startOnMapX, out int startOnMapY);

        if (Orientation == "EAST")
        {
            for(int x = 0; x < Length; x++)
            {
                // Add x to get the next point and plot.
                map.CheckAndPlot(startOnMapX + x, startOnMapY, 'F');
            }
        }
        else if (Orientation == "NORTH")
        {
            for(int y = 0; y < Length; y++)
            {
                // Subtract y to get the next point and plot.
                map.CheckAndPlot(startOnMapX, startOnMapY - y, 'F');
            }
        }
    }
}