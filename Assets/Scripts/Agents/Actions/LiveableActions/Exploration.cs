using AgentBehaviour.FuzzyCognitiveMapUtilities;
using Agents.LiveableAgents;
using LogicGrid;

namespace Agents.Actions.LiveableActions
{
    public class Exploration : LiveableAction
    {
        public readonly LiveableActionType Type = LiveableActionType.Exploration;
        public override bool CheckConditions(Liveable agent)
        {
            return true;
        }

        public override void Invoke(Liveable agent)
        {
            // TODO: Choose random direction and move by speed
            Walker.TryToMoveTowardsDirections(agent, SimulationGrid.FindRandomTargetCell(agent));
            // TODO: Divide curiosity in cognition map by 1.5
            agent.CognitiveMap.MultiplyNamedInternalConcept(NamedInternalConcept.Curiosity, 1f/1.5f);
        }
    }
}