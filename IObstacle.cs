namespace Threat_o_tron;

interface IObstacle
{
    /// <summary>
    /// Discovers the mapX and mapY coordinates, checks if the the obstacle will be on the map, and plots it on the given map.
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    public void DrawOnMap(Map map);
}