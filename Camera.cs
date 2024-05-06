using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Threat_o_tron;

class Camera : IObstacle
{
    /// <summary>
    /// GameX and GameY is the Cameras starting location in the game
    /// </summary>
    private readonly int GameX;
    private readonly int GameY;    
    private readonly string Orientation;

    /// <summary>
    /// Instantiates a new Camera.
    /// </summary>
    /// <param name="x">The X Coordinate of the camera in the game.</param>
    /// <param name="y">The Y Coordinate of the camera in the game.</param>
    /// <param name="orientation">Which way the camera spans.</param>
    public Camera(int x, int y, string orientation)
    {
        GameX = x;
        GameY = y;
        Orientation = orientation;        
    }   

    /// <summary>
    /// Draws a triangle on the provided map in the given direction(orientaton).
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    public void DrawOnMap(Map map)
    {
        //Establish startpoint for ploting the symbol on the map; this will be the map's canvas coordinates, not the games.
        map.FindPointOnMap(GameX, GameY, out int mapX, out int mapY);
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
    /// Draws a triangle pointing North on the given map.
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    private static void DrawNorthCamera(Map map, int mapX, int mapY){         
        //count how many spaces there are ABOVE the starting coordinate and loop through them   
        for(int rows = 0; rows < mapY+1; rows++)
        {
            //in each layer above the start point, plot all the available spaces that are above this for both the right and left side.
            for(int rowsAboveCurrentRow = 0; rowsAboveCurrentRow < mapY+1 - rows; rowsAboveCurrentRow++)
            {
                //Right side
                map.CheckAndPlot(mapX + rows, mapY - rowsAboveCurrentRow - rows, 'C');
                //Left side
                map.CheckAndPlot(mapX - rows, mapY - rowsAboveCurrentRow - rows, 'C');
            }
        }
    }

    /// <summary>
    /// Draws a triangle pointing East on the given map.
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    private static void DrawEastCamera(Map map, int mapX, int mapY)
    {                  
        //count how many spaces there are East of the starting coordinate and loop through them   
        for(int columns = 0; columns < map.SizeX - mapX; columns++)
        {
            //in each column east of the starting point, plot all the available spaces that are east of this both above and below.
            for(int columnsRightOfColumn = 0; columnsRightOfColumn < map.SizeX - mapX + columns+1; columnsRightOfColumn++)
            {
                //Above
                map.CheckAndPlot(mapX + columnsRightOfColumn + columns, mapY - columns, 'C');
                //Below      
                map.CheckAndPlot(mapX + columnsRightOfColumn + columns, mapY + columns, 'C');
            }
        }
    }

    /// <summary>
    /// Draws a triangle pointing South on the given map.
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    private static void DrawSouthCamera(Map map, int mapX, int mapY)
    {                 
        //count how many spaces there are BELOW the starting coordinate and loop through them   
        for(int rows = 0; rows < map.SizeY - mapY; rows++)
        {
            //in each layer BELOW the start point, plot all the available spaces that are BELOW this for both the right and left side.
            for(int rowsBelowRow = 0; rowsBelowRow < map.SizeY - mapY + rows+1; rowsBelowRow++)
            {
                //Right side
                map.CheckAndPlot(mapX + rows, mapY + rowsBelowRow + rows, 'C');
                //Left side
                map.CheckAndPlot(mapX - rows, mapY + rowsBelowRow + rows, 'C');
            }
        }
    }

    /// <summary>
    /// Draws a triangle pointing West on the given map.
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    private static void DrawWestCamera(Map map, int mapX, int mapY)
    {      
        //count how many spaces to the WEST there are from the starting coordinate and loop through them   
        for(int columns = 0; columns < mapX+1; columns++)
        {
            //in each column WEST of the curent column, plot all the available spaces that are west of this for both above and below.
            for(int columnsLeftOfColumn = 0; columnsLeftOfColumn < mapX+1 - columns; columnsLeftOfColumn++)
            {
                    //Above
                    map.CheckAndPlot(mapX - columnsLeftOfColumn - columns, mapY - columns, 'C');
                    //Below
                    map.CheckAndPlot(mapX - columnsLeftOfColumn - columns, mapY + columns, 'C');
            }
        }
    }
}