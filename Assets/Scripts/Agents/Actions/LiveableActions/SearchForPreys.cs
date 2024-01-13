using Agents.LiveableAgents;
using Agents.ResourceAgents;
using LogicGrid;
using Settings;

namespace Agents.Actions.LiveableActions
{
    public class SearchForPreys : LiveableAction
    {
        public readonly LiveableActionType Type = LiveableActionType.SearchForPreys;
        public override bool CheckConditions(Liveable agent)
        {
            Prey nearestPrey = Finder.FindNearestPreyForAgent(agent);
            
            if(nearestPrey == null)
            {
                return false;
            }
            
            return SimulationGrid.DistanceFromAgentToAgent(agent, nearestPrey) <= DevSet.I.simulation.distanceVisionPredator;

        }

        public override void Invoke(Liveable agent)
        {
            Prey nearestPrey = Finder.FindNearestPreyForAgent(agent);

            if (nearestPrey == null)
            {
                return;
            }

            // TODO: DONE: If prey is in range of speed - move to it, else move by speed
            Walker.TryToMoveTowardsDirections(agent, nearestPrey.CurrentPosition);
        }
    }
}