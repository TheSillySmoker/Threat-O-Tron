namespace Threat_o_tron;

interface IObstacle{
    /// <summary>
    /// Takes an existing map's canvas and plots obstacles onto it.
    /// </summary>
    /// <param name="map">The map that will be drawn on.</param>
    public void drawOnMap(Map map);
}