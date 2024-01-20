using AgentBehaviour.FuzzyCognitiveMapUtilities;
using Agents.LiveableAgents;
using LogicGrid;
using Settings;

namespace Agents.Actions.LiveableActions
{
    public class Evasion : LiveableAction
    {
        public Evasion()
        {
            ActionType = LiveableActionType.Evasion;
        }
        public override bool CheckConditions(Liveable agent)
        {
            return true;
        }

        public override void Invoke(Liveable agent)
        {
            var nearestPredator = Finder.FindNearestEnemyForAgent(agent);
            if (nearestPredator != null && SimulationGrid.DistanceFromAgentToAgent(agent, nearestPredator) <= DevSet.I.simulation.distanceVisionPrey && SimulationGrid.DistanceFromAgentToAgent(agent, nearestPredator) > 0)
            {
                Walker.TryToMoveTowardsDirections(agent,
                    SimulationGrid.FindOppositeDirectionToAgent(agent, nearestPredator));
            }
            else
            {
                Walker.TryToMoveTowardsDirections(agent, SimulationGrid.FindRandomTargetCell(agent));
            }
            agent.CognitiveMap.MultiplyNamedInternalConcept(NamedInternalConcept.Fear, 0.5f);
        }
    }
}