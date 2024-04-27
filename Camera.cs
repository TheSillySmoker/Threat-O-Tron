using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Threat_o_tron;

class Camera : IObstacle
{
    /// <summary>
    /// XOrigin and YOrigin is the Cameras starting location in the game
    /// </summary>
    private int XOrigin{get; set;}
    private int YOrigin{get; set;}    
    private string Orientation{get; set;}
    private int XStart{get;set;}
    private int YStart{get;set;}
    /// <summary>
    /// Instantiates a new Camera.
    /// </summary>
    /// <param name="x">The X Coordinate of the camera in the game.</param>
    /// <param name="y">The Y Coordinate of the camera in the game.</param>
    /// <param name="orientation"></param>
    public Camera(int x, int y, string orientation){
        XOrigin = x;
        YOrigin = y;
        Orientation = orientation;        
    }   

    /// <summary>
    /// Draws a triangle on the provided map in the given direction(orientaton).
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    public void drawOnMap(Map map)
    {
        //Establish startpoint for ploting the symbol on the map; this will be the map's canvas corrdinates, not the games.
        map.FindPointOnMap(XOrigin, YOrigin, out int xStart, out int yStart);
        XStart = xStart;
        YStart = yStart;
        switch(Orientation){
            case "NORTH":
                DrawNorthCamera(map);
            break;
            case "EAST":
                DrawEastCamera(map);
            break;
            case "SOUTH":
                DrawSouthCamera(map);
            break;
            case "WEST":
                DrawWestCamera(map);
            break;                        
        }
    }

    /// <summary>
    /// Draws a triange pointing North on the given map.
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    private void DrawNorthCamera(Map map){
        int x;
        int y;             
        //count how many spaces there are ABOVE the starting corrdinate and loop through them   
        for(int i = 0; i < YStart+1; i++){
            //in each layer above the start point, plot all the available spaces that are above this for both the right and left side.
            for(int ii = 0; ii < YStart+1 - i; ii++){
                    x = XStart+i;
                    y = YStart-ii-i;
                    map.CheckAndPlot(x, y, 'C');
                    x = XStart-i;
                    y = YStart-ii-i;
                    map.CheckAndPlot(x, y, 'C');
            }
        }
    }

    /// <summary>
    /// Draws a triange pointing East on the given map.
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    private void DrawEastCamera(Map map){
        int x;
        int y;                   
        //count how many spaces there are East of the starting corrdinate and loop through them   
        for(int i = 0; i < map.SizeX - XStart; i++){
            //in each layer above the east of the starting point, plot all the available spaces that are east of this for both above and below.
            for(int ii = 0; ii < map.SizeX - XStart + i+1; ii++){
                y = YStart+i;
                x = XStart+ii+i;
                map.CheckAndPlot(x, y, 'C');
                y = YStart-i;
                x = XStart+ii+i;
                map.CheckAndPlot(x, y, 'C');
            }
        }
    }

    /// <summary>
    /// Draws a triange pointing South on the given map.
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    private void DrawSouthCamera(Map map){
        int x;
        int y;                   
        //count how many spaces there are BELOW the starting corrdinate and loop through them   
        for(int i = 0; i < map.SizeY - YStart; i++){
            //in each layer BELOW the start point, plot all the available spaces that are BELOW this for both the right and left side.
            for(int ii = 0; ii < map.SizeY - YStart + i+1; ii++){
                x = XStart+i;
                y = YStart+ii+i;
                map.CheckAndPlot(x, y, 'C');
                x = XStart-i;
                y = YStart+ii+i;
                map.CheckAndPlot(x, y, 'C');
            }
        }
    }

    /// <summary>
    /// Draws a triange pointing West on the given map.
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    private void DrawWestCamera(Map map){
        int x;
        int y;              
        //count how many spaces to the WEST there are from the starting corrdinate and loop through them   
        for(int i = 0; i < XStart+1; i++){
            //in each layer WEST of the start point, plot all the available spaces that are west of this for both above and below.
            for(int ii = 0; ii < XStart+1 - i; ii++){
                    y = YStart+i;
                    x = XStart-ii-i;
                    map.CheckAndPlot(x, y, 'C');
                    y = YStart-i;
                    x = XStart-ii-i;
                    map.CheckAndPlot(x, y, 'C');
            }
        }
    }
 

}