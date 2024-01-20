using Agents.LiveableAgents;
using Agents.ResourceAgents;
using LogicGrid;
using Settings;

namespace Agents.Actions.LiveableActions
{
    public class SearchForPreys : LiveableAction
    {
        public SearchForPreys()
        {
            ActionType = LiveableActionType.SearchForPreys;
        }
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
            
            Walker.TryToMoveTowardsDirections(agent, nearestPrey.CurrentPosition);
            
            // Murder pray if you are on the same gird as prey
            if (SimulationGrid.DistanceFromAgentToAgent(nearestPrey, agent) <= 0)
            {
                nearestPrey.Murder();
                return;
            }
        }
    }
}