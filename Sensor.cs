namespace Threat_o_tron;

class Sensor : IObstacle
{
    /// <summary>
    /// XOrigin and YOrigin is the location of the Centre of the Sensor
    /// /// </summary>
    private int XOrigin{get; set;}
    private int YOrigin{get; set;}
    private float Radius{get; set;}


    public Sensor(int x, int y, float radius){
        XOrigin = x;
        YOrigin = y;
        Radius = radius;
    }
    public void drawOnMap(Map map)
    {
        map.FindPointOnMap(XOrigin, YOrigin, out int xStart, out int yStart);

        int roundedRadius = RoundFloatToInt(Radius);

        for (int y = yStart - roundedRadius; y < yStart + Radius; y++) {
            for (int x = xStart - roundedRadius; x < xStart + Radius; x++) {
                if (Math.Pow(x - xStart, 2) + Math.Pow(y - yStart, 2) > Math.Pow(roundedRadius - 0.5, 2)) continue;
                    map.CheckAndPlot(x, y, 'S');
                }   
        }
    }
    /// <summary>
    /// Takes a float value and rounds it up if it is above .5.
    /// </summary>
    /// <param name="floatValue">The float value that will be rounded</param>
    /// <returns></returns>
    static int RoundFloatToInt(float floatValue)
    {
        float fractionalPart = floatValue - (float)Math.Floor(floatValue);

        if (fractionalPart >= 0.5f){
            return (int)Math.Ceiling(floatValue);
        }
        else{
            return (int)Math.Floor(floatValue);
        }
    }
}