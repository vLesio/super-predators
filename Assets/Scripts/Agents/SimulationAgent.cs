namespace Agents
{
    public abstract class SimulationAgent : CellAgent
    {
        public bool ActedThisTurn { get; set; }
        public abstract void ChooseAction();
        public abstract void Act();

        public override string ToString() {
            return $"Agent: {CurrentPosition}";
        }
    }
}