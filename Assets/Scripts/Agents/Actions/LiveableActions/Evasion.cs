﻿using AgentBehaviour.FuzzyCognitiveMapUtilities;
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
                Walker.TryToMoveTowardsDirections(agent,
                    SimulationGrid.FindOppositeDirectionToAgent(agent, nearestPredator));
                // TODO: DONE: Choose opposite direction and move by speed
            }
            else
            {
                Walker.TryToMoveTowardsDirections(agent, SimulationGrid.FindRandomDirection());
                // TODO: DONE: Choose random direction and move by speed
            }
            agent.CognitiveMap.MultiplyNamedInternalConcept(NamedInternalConcept.Fear, 0.5f);
            // TODO: DONE: Divide cognitive map for activation fear by 2 
        }
    }
}