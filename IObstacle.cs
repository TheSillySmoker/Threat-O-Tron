namespace Threat_o_tron;

interface IObstacle
{
    /// <summary>
    /// Gets map coordinates for obstacle, checks if the the obstacle will be on the Map, plots obstacle on the given Map.
    /// </summary>
    /// <param name="map">The Map that will be drawn on.</param>
    public void DrawOnMap(Map map);
}