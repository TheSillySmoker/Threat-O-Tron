using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Threat_o_tron;

class Camera : IObstacle
{
    /// <summary>
    /// CameraGameX and CameraGameY is the Cameras starting location in the game
    /// </summary>
    private int CameraGameX{get; set;}
    private int CameraGameY{get; set;}    
    private string Orientation{get; set;}
    private int XStartOnMap{get;set;}
    private int YStartOnMap{get;set;}
    /// <summary>
    /// Instantiates a new Camera.
    /// </summary>
    /// <param name="x">The X Coordinate of the camera in the game.</param>
    /// <param name="y">The Y Coordinate of the camera in the game.</param>
    /// <param name="orientation"></param>
    public Camera(int x, int y, string orientation){
        CameraGameX = x;
        CameraGameY = y;
        Orientation = orientation;        
    }   

    /// <summary>
    /// Draws a triangle on the provided map in the given direction(orientaton).
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    public void drawOnMap(Map map)
    {
        //Establish startpoint for ploting the symbol on the map; this will be the map's canvas corrdinates, not the games.
        map.FindPointOnMap(CameraGameX, CameraGameY, out int xStartOnMap, out int yStartOnMap);
        XStartOnMap = xStartOnMap;
        YStartOnMap = yStartOnMap;
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
        for(int i = 0; i < YStartOnMap+1; i++){
            //in each layer above the start point, plot all the available spaces that are above this for both the right and left side.
            for(int ii = 0; ii < YStartOnMap+1 - i; ii++){
                    x = XStartOnMap+i;
                    y = YStartOnMap-ii-i;
                    map.CheckAndPlot(x, y, 'C');
                    x = XStartOnMap-i;
                    y = YStartOnMap-ii-i;
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
        for(int i = 0; i < map.SizeX - XStartOnMap; i++){
            //in each layer above the east of the starting point, plot all the available spaces that are east of this for both above and below.
            for(int ii = 0; ii < map.SizeX - XStartOnMap + i+1; ii++){
                y = YStartOnMap+i;
                x = XStartOnMap+ii+i;
                map.CheckAndPlot(x, y, 'C');
                y = YStartOnMap-i;
                x = XStartOnMap+ii+i;
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
        for(int i = 0; i < map.SizeY - YStartOnMap; i++){
            //in each layer BELOW the start point, plot all the available spaces that are BELOW this for both the right and left side.
            for(int ii = 0; ii < map.SizeY - YStartOnMap + i+1; ii++){
                x = XStartOnMap+i;
                y = YStartOnMap+ii+i;
                map.CheckAndPlot(x, y, 'C');
                x = XStartOnMap-i;
                y = YStartOnMap+ii+i;
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
        for(int i = 0; i < XStartOnMap+1; i++){
            //in each layer WEST of the start point, plot all the available spaces that are west of this for both above and below.
            for(int ii = 0; ii < XStartOnMap+1 - i; ii++){
                    y = YStartOnMap+i;
                    x = XStartOnMap-ii-i;
                    map.CheckAndPlot(x, y, 'C');
                    y = YStartOnMap-i;
                    x = XStartOnMap-ii-i;
                    map.CheckAndPlot(x, y, 'C');
            }
        }
    }
 

}