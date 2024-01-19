using AgentBehaviour.FuzzyCognitiveMapUtilities;
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

            if (nearestMate != null && SimulationGrid.DistanceFromAgentToAgent(agent, nearestMate) <= (Liveable.IsPrey(agent)
                    ? DevSet.I.simulation.distanceVisionPrey
                    : DevSet.I.simulation.distanceVisionPredator))
            {
                // TODO: DONE: If the speed is enough to reach the mate - move to it, else move by speed
                Walker.TryToMoveTowardsDirections(agent, nearestMate.CurrentPosition);
                // TODO: DONE: If mate reached - divide sexual needs in cognition map by 3
                if (Finder.FindNearestMateForAgent(agent).CurrentPosition.Equals(agent.CurrentPosition))
                {
                    agent.CognitiveMap.MultiplyNamedInternalConcept(NamedInternalConcept.SexualNeeds, 1f/3f);
                }
            }
            else
            {
                // TODO: DONE: Choose random direction and move by speed like a moron
                Walker.TryToMoveTowardsDirections(agent, SimulationGrid.FindRandomTargetCell(agent));
            }
        }

    }
}