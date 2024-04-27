namespace Threat_o_tron;
class Fence : IObstacle{
    /// <summary>
    /// FenceStartGameX and FenceStartGameY is the start point of the Fence in the game. Length is how many klicks the Fence will span. Orientation is the direction the fence will span.
    /// </summary>
    private int FenceStartGameX{get;set;}
    private int FenceStartGameY{get;set;}
    private int FenceLength{get;set;}
    private string Orientation{get;set;}
    
    /// <summary>
    /// Creates a new Fence with origin coordinates, orientation and length 
    /// </summary>
    /// <param name="x">The X Coordinate of the Fence in the game.</param>
    /// <param name="y">The Y Coordinate of the Fence in the game.</param>
    /// <param name="orientation">Which way is the Fence spanning.</param>
    /// <param name="length">The length of the Fence in klicks.</param>
    public Fence(int x, int y, string orientation, int fenceLength){
        FenceStartGameX = x;
        FenceStartGameY = y;
        FenceLength = fenceLength;
        Orientation = orientation;
    }
    /// <summary>
    /// Takes an existing map's canvas and plots this obstacle onto it.
    /// </summary>
    /// <param name="map">The map that holds the canvas that will be used to draw on.</param>
    /// <returns>A new char array which can be used for a canvas. It contains this obstacle.</returns>
    public void drawOnMap(Map map){
        //Establish startpoint for ploting the symbol on the map
        map.FindPointOnMap(FenceStartGameX, FenceStartGameY, out int sensorStartMapX, out int sensorStartMapY);

        int x = sensorStartMapX;
        int y = sensorStartMapY;

        if (Orientation == "EAST"){
            for(int i = 0; i < FenceLength; i++){
                //reset x to where the fence starts and add i to get the next point to plot
                x = sensorStartMapX + i;
                map.CheckAndPlot(x, y, 'F');
            }
        }
        if (Orientation == "NORTH"){
            for(int i = 0; i < FenceLength; i++){
                //reset y to where the fence starts and subtract i to get the next point to plot
                y = sensorStartMapY - i;
                map.CheckAndPlot(x, y, 'F');
            }
        }
    }
}