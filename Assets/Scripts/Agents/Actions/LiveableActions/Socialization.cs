using Agents.LiveableAgents;
using LogicGrid;
using Settings;

namespace Agents.Actions.LiveableActions
{
    public class Socialization : LiveableAction
    {
        public readonly LiveableActionType Type = LiveableActionType.Socialization;

        public override bool CheckConditions(Liveable agent)
        {
            return true;
        }

        public override void Invoke(Liveable agent)
        {
            Liveable nearestMate = Finder.FindNearestMateForAgent(agent);

            if (SimulationGrid.DistanceFromAgentToAgent(agent, nearestMate) <= (Liveable.IsPrey(agent)
                    ? DevSet.I.simulation.distanceVisionPrey
                    : DevSet.I.simulation.distanceVisionPredator))
            {
                // TODO: If the speed is enough to reach the mate - move to it, else move by speed
                
                // TODO: If mate reached - divide sexual needs in cognition map by 3
            }
            else
            {
                // TODO: Choose random direction and move by speed like a moron
            }
        }

    }
}