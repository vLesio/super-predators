using Agents.LiveableAgents;
using LogicGrid;
using Settings;

namespace Agents.Actions.LiveableActions
{
    public class Evasion : LiveableAction
    {
        public readonly LiveableActionType Type = LiveableActionType.Evasion;
        public override bool CheckConditions(Liveable agent)
        {
            return true;
        }

        public override void Invoke(Liveable agent)
        {
            var nearestPredator = Finder.FindNearestEnemyForAgent(agent);
            var distance = SimulationGrid.DistanceFromAgentToAgent(agent, nearestPredator);
            if (distance <= DevSet.I.simulation.distanceVisionPrey)
            {
                // TODO: Choose opposite direction and move by speed
            }
            else
            {
                // TODO: Choose random direction and move by speed
            }
            
            // TODO: Divide cognitive map for activation fear by 2 
        }
    }
}