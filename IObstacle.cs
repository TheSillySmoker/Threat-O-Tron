namespace Threat_o_tron;

interface IObstacle
{
    /// <summary>
    /// Discovers the mapX and mapY coordinates, checks if the the obstacle will be on the Map, and plots it on the given Map.
    /// </summary>
    /// <param name="map">The Map that will be drawn on.</param>
    public void DrawOnMap(Map map);
}