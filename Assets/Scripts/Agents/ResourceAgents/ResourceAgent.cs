namespace Agents.ResourceAgents
{
    public abstract class ResourceAgent : SimulationAgent
    {
        public float Quantity = 0f;
        
        public static bool IsGrass(ResourceAgent agent)
        {
            return agent.GetType() == typeof(Grass);
        }
    }
}