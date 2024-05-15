namespace Threat_o_tron;

class MSPath : Map
{
    private int AgentMapX;
    private int AgentMapY;

    private readonly int ObjectiveMapX;   
    private readonly int ObjectiveMapY;

    /// <summary>
    /// An array that represents the Map's canvas and records where the algorithm has been. 
    /// True represents a visited coordinate, false, not visited. This is to prevent backtracking.
    /// </summary>
    private readonly bool [,] Visited;

    /// <summary>
    /// An array that represents the Map's canvas and records the directions to the solution. 
    /// If a coordinate is true, this is the way to the objective.
    /// </summary>
    private readonly bool [,] Solution;

    /// <summary>
    /// Instantiates a new path.
    /// </summary>
    /// <param name="agentGameX">X coordinate of where the agent starts.</param>
    /// <param name="agentGameY">Y coordinate of where the agent starts.</param>
    /// <param name="objectiveGameX">X coordinate of where the objective is.</param>
    /// <param name="objectiveGameY">Y coordinate of where the objective is.</param>
    /// <param name="obstacles">All the obstacles the agent might encounter and will avoid.</param>
    public MSPath(int agentGameX, int agentGameY, int objectiveGameX, int objectiveGameY, List<IObstacle> obstacles) 
    : base(
        Math.Min(agentGameX, objectiveGameX) - 3, 
        Math.Min(agentGameY, objectiveGameY) - 3, 
        Math.Abs(objectiveGameX - agentGameX) + 6, 
        Math.Abs(objectiveGameY - agentGameY) + 6,
        obstacles
    )
    {
        GetMapCoordinates(agentGameX, agentGameY, out AgentMapX, out AgentMapY);
        GetMapCoordinates(objectiveGameX, objectiveGameY, out ObjectiveMapX, out ObjectiveMapY);        
        
        //All points in these arrays will start as false. 
        Visited = new bool[Height,Width];
        Solution = new bool[Height,Width];
    }

}