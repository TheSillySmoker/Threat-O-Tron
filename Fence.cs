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
    /// Creates a new Fence with origin coordinates, orientation and length 
    /// </summary>
    /// <param name="x">The X Coordinate of the Fence in the game.</param>
    /// <param name="y">The Y Coordinate of the Fence in the game.</param>
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
    /// Takes an existing map's canvas and plots this obstacle onto it.
    /// </summary>
    /// <param name="map">The map that holds the canvas that will be used to draw on.</param>
    /// <returns>A new char array which can be used for a canvas. It contains this obstacle.</returns>
    public void DrawOnMap(Map map)
    {
        //Establish startpoint for ploting the symbol on the map
        map.FindPointOnMap(StartGameX, StartGameY, out int startOnMapX, out int startOnMapY);
        if (Orientation == "EAST")
        {
            for(int x = 0; x < Length; x++)
            {
                //Add x to get the next point and plot
                map.CheckAndPlot(startOnMapX + x, startOnMapY, 'F');
            }
        }
        if (Orientation == "NORTH")
        {
            for(int i = 0; i < Length; i++)
            {
                //Subtract y to get the next point and plot
                map.CheckAndPlot(startOnMapX, startOnMapY - i, 'F');
            }
        }
    }
}