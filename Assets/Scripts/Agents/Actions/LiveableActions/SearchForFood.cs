using System.Net.Http.Headers;
using Agents.LiveableAgents;
using Agents.ResourceAgents;
using CoinPackage.Debugging;
using LogicGrid;
using Settings;

namespace Agents.Actions.LiveableActions
{
    public class SearchForFood : LiveableAction
    {
        public SearchForFood()
        {
            ActionType = LiveableActionType.SearchForFood;
        }
        public override bool CheckConditions(Liveable agent)
        {
            ResourceAgent nearestFood = Liveable.IsPrey(agent)
                ? Finder.FindNearestGrassForAgent(agent)
                : Finder.FindNearestMeatForAgent(agent);

            if (nearestFood == null)
            {
                return false;
            }
            
            return SimulationGrid.DistanceFromAgentToAgent(agent, nearestFood) <= (Liveable.IsPrey(agent) 
                ? DevSet.I.simulation.distanceVisionPrey 
                : DevSet.I.simulation.distanceVisionPredator);
        }

        public override void Invoke(Liveable agent)
        {
            ResourceAgent nearestFood = Liveable.IsPrey(agent)
                ? Finder.FindNearestGrassForAgent(agent)
                : Finder.FindNearestMeatForAgent(agent);
            
            if (nearestFood == null)
            {
                return;
            }

            // TODO: DONE: If food is in range of speed - move to it, else move by speed
            CDebug.Log($"Prey is trying to reach food at {nearestFood.CurrentPosition}, while being at {agent.CurrentPosition}.");
            Walker.TryToMoveTowardsDirections(agent, nearestFood.CurrentPosition);
        }
    }
}