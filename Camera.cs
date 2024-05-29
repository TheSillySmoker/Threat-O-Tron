namespace Threat_o_tron;

class Camera : IObstacle
{
    /// <summary>
    /// GameX and GameY is the Camera's starting location in the Game.
    /// Orientation is the direction the camera will face when drawn on a map.
    /// </summary>
    private readonly int GameX;
    private readonly int GameY;    
    private readonly string Orientation;

    /// <summary>
    /// Instantiates a new Camera.
    /// </summary>
    /// <param name="x">The X Coordinate of the camera in the Game.</param>
    /// <param name="y">The Y Coordinate of the camera in the Game.</param>
    /// <param name="orientation">Direction the camera faces.</param>
    public Camera(int x, int y, string orientation)
    {
        GameX = x;
        GameY = y;
        Orientation = orientation;        
    }   

    /// <summary>
    /// Draws a triangle on the provided Map in the given direction (orientaton).
    /// </summary>
    /// <param name="map">The Map that will be drawn on.</param>
    public void DrawOnMap(Map map)
    {
        // Establish point for plotting the symbol on the Map, this will be the Map's coordinates, not the Game's.
        map.GetMapCoordinates(GameX, GameY, out int mapX, out int mapY);

        switch(Orientation){
            case "NORTH":
                DrawNorthCamera(map, mapX, mapY);
                break;
            case "EAST":
                DrawEastCamera(map, mapX, mapY);
                break;
            case "SOUTH":
                DrawSouthCamera(map, mapX, mapY);
                break;
            case "WEST":
                DrawWestCamera(map, mapX, mapY);
                break;                        
        }
    }

    /// <summary>
    /// Draws a triangle pointing North on the given Map.
    /// </summary>
    /// <param name="map">The Map that will be drawn on.</param>
    /// <param name="mapX">The Camera's starting X Coordinate on the Map.</param>
    /// <param name="mapY">The Camera's starting Y Coordinate on the Map.</param>
    private static void DrawNorthCamera(Map map, int mapX, int mapY){         
        // Count how many spaces there are above the starting coordinate and loop through them.  
        for(int rows = 0; rows < mapY + 1; rows++)
        {
            // In each layer above the start point, plot all the available spaces that are above this for both the right and left side.
            for(int rowsAboveCurrentRow = 0; rowsAboveCurrentRow < mapY + 1 - rows; rowsAboveCurrentRow++)
            {
                // Right side.
                map.CheckAndPlot(mapX + rows, mapY - rowsAboveCurrentRow - rows, 'C');
                // Left side.
                map.CheckAndPlot(mapX - rows, mapY - rowsAboveCurrentRow - rows, 'C');
            }
        }
    }

    /// <summary>
    /// Draws a triangle pointing east on the given Map.
    /// </summary>
    /// <param name="map">The Map that will be drawn on.</param>
    /// <param name="mapX">The Camera's starting X Coordinate on the Map.</param>
    /// <param name="mapY">The Camera's starting Y Coordinate on the Map.</param>
    private static void DrawEastCamera(Map map, int mapX, int mapY)
    {                  
        // Count how many spaces there are east of the starting coordinate and loop through them.   
        for(int columns = 0; columns < map.Width - mapX; columns++)
        {
            // In each column east of the starting point, plot all the available spaces that are east of this both above and below.
            for(int columnsRightOfColumn = 0; columnsRightOfColumn < map.Width - mapX + columns + 1; columnsRightOfColumn++)
            {
                // Above.
                map.CheckAndPlot(mapX + columnsRightOfColumn + columns, mapY - columns, 'C');
                // Below.     
                map.CheckAndPlot(mapX + columnsRightOfColumn + columns, mapY + columns, 'C');
            }
        }
    }

    /// <summary>
    /// Draws a triangle pointing south on the given Map.
    /// </summary>
    /// <param name="map">The Map that will be drawn on.</param>
    /// <param name="mapX">The Camera's starting X Coordinate on the Map.</param>
    /// <param name="mapY">The Camera's starting Y Coordinate on the Map.</param>
    private static void DrawSouthCamera(Map map, int mapX, int mapY)
    {                 
        // Count how many spaces there are below the starting coordinate and loop through them.   
        for(int rows = 0; rows < map.Height - mapY; rows++)
        {
            // In each layer below the start point, plot all the available spaces that are BELOW this for both the right and left side.
            for(int rowsBelowRow = 0; rowsBelowRow < map.Height - mapY + rows + 1; rowsBelowRow++)
            {
                // Right side.
                map.CheckAndPlot(mapX + rows, mapY + rowsBelowRow + rows, 'C');
                // Left side.
                map.CheckAndPlot(mapX - rows, mapY + rowsBelowRow + rows, 'C');
            }
        }
    }

    /// <summary>
    /// Draws a triangle pointing west on the given Map.
    /// </summary>
    /// <param name="map">The Map that will be drawn on.</param>
    /// <param name="mapX">The Camera's starting X Coordinate on the Map.</param>
    /// <param name="mapY">The Camera's starting Y Coordinate on the Map.</param>
    private static void DrawWestCamera(Map map, int mapX, int mapY)
    {      
        // Count how many spaces to the west there are from the starting coordinate and loop through them.   
        for(int columns = 0; columns < mapX + 1; columns++)
        {
            // In each column west of the curent column, plot all the available spaces that are west of this for both above and below.
            for(int columnsLeftOfColumn = 0; columnsLeftOfColumn < mapX + 1 - columns; columnsLeftOfColumn++)
            {
                // Above.
                map.CheckAndPlot(mapX - columnsLeftOfColumn - columns, mapY - columns, 'C');
                // Below.
                map.CheckAndPlot(mapX - columnsLeftOfColumn - columns, mapY + columns, 'C');
            }
        }
    }
}