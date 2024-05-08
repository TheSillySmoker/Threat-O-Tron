using System.Dynamic;
using System.Runtime.CompilerServices;
using Microsoft.Win32.SafeHandles;

namespace Threat_o_tron;

class Path : Map{

    public Path(int agentX, int agentY, int objectiveX, int objectiveY, List<IObstacle> obstacles) 
    : base(GetLowerNumber(agentX, objectiveX), GetLowerNumber(agentY, objectiveY), GetHigherNumber(agentX, objectiveX), GetHigherNumber(agentY, objectiveY), obstacles){
        this.PrintMap();

    }

    private static int GetLowerNumber(int coordinate1, int coordinate2)
    {
        if (coordinate1 < coordinate2)
        {
            return coordinate1;
        }
        else
        {
            return coordinate2;
        }
    }

    private static int GetHigherNumber(int coordinate1, int coordinate2)
    {
        if (coordinate1 > coordinate2)
        {
            return coordinate1;
        }
        else
        {
            return coordinate2;
        }
    }
    
}