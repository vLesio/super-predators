namespace Agents.ResourceAgents
{
    public abstract class ResourceAgent : SimulationAgent
    {
        public float Quantity = 0f;
        
        public static bool IsGrass(ResourceAgent agent)
        {
            return agent.GetType() == typeof(Grass);
        }

        public bool IsEmpty() {
            return Quantity <= 0;
        }
        
        public bool IsUseful() {
            return Quantity >= 1;
        }
        
        public abstract bool UpdateQuantity();
    }
}