namespace Threat_o_tron;
class Fence : IObstacle{
    /// <summary>
    /// GameX and GameY is the start point of the Fence in the game.
    /// Length is how many klicks the Fence will span. 
    /// Orientation is the direction the fence will span.
    /// </summary>
    private readonly int GameX;
    private readonly int GameY;
    private readonly int Length;
    private readonly string Orientation;
    
    /// <summary>
    /// Creates a new Fence with origin coordinates, orientation and length.
    /// </summary>
    /// <param name="x">The X coordinate of the start of this Fence in the game.</param>
    /// <param name="y">The Y coordinate of the start of this Fence in the game.</param>
    /// <param name="orientation">Which way the Fence is spanning.</param>
    /// <param name="length">The length of the Fence in klicks.</param>
    public Fence(int x, int y, string orientation, int length)
    {
        GameX = x;
        GameY = y;
        Length = length;
        Orientation = orientation;
    }
    
    /// <summary>
    /// Plots an 'F' where the Fence exists on the provided Map.
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    public void DrawOnMap(Map map)
    {
        // Establish point for plotting the symbol on the Map, this will be the Map's coordinates, not the Game's.
        map.GetMapCoordinates(GameX, GameY, out int mapX, out int mapY);

        if (Orientation == "EAST")
        {
            for(int x = 0; x < Length; x++)
            {
                // Add x to get the next point and plot.
                map.CheckAndPlot(mapX + x, mapY, 'F');
            }
        }
        else if (Orientation == "NORTH")
        {
            for(int y = 0; y < Length; y++)
            {
                // Subtract y to get the next point and plot.
                map.CheckAndPlot(mapX, mapY - y, 'F');
            }
        }
    }
}