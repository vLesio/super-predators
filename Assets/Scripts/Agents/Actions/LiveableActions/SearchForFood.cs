using Agents.LiveableAgents;
using Agents.ResourceAgents;
using LogicGrid;
using Settings;

namespace Agents.Actions.LiveableActions
{
    public class SearchForFood : LiveableAction
    {
        public readonly LiveableActionType Type = LiveableActionType.SearchForFood;
        public override bool CheckConditions(Liveable agent)
        {
            ResourceAgent nearestFood = Liveable.IsPrey(agent)
                ? Finder.FindNearestGrassForAgent(agent)
                : Finder.FindNearestMeatForAgent(agent);
            
            return SimulationGrid.DistanceFromAgentToAgent(agent, nearestFood) <= (Liveable.IsPrey(agent) 
                ? DevSet.I.simulation.distanceVisionPrey 
                : DevSet.I.simulation.distanceVisionPredator);
        }

        public override void Invoke(Liveable agent)
        {
            ResourceAgent nearestFood = Liveable.IsPrey(agent)
                ? Finder.FindNearestGrassForAgent(agent)
                : Finder.FindNearestMeatForAgent(agent);

            // TODO: If food is in range of speed - move to it, else move by speed
        }
    }
}