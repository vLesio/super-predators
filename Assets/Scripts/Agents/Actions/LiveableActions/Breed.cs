using AgentBehaviour.GenomeUtilities;
using Agents.LiveableAgents;
using JetBrains.Annotations;
using LogicGrid;
using Settings;

namespace Agents.Actions.LiveableActions
{
    public class Breed : LiveableAction
    {
        public readonly LiveableActionType Type = LiveableActionType.Breed;
        public override bool CheckConditions(Liveable agent)
        {

            if (!CheckIfLivableHasEnergy(agent))
            {
                return false;
            }

            var breedMate = CheckIfThereIsCapableMateInRange(agent);
            if(breedMate == null)
            {
                return false;
            }

            return true;
        }

        public override void Invoke(Liveable agent)
        {
            throw new System.NotImplementedException();
        }

        private bool CheckIfLivableHasEnergy(Liveable agent)
        {
            return (agent.attributes[LiveableAttribute.Energy] > 0.125f * (agent.GetType() == typeof(Prey)
                    ? DevSet.I.simulation.maxEnergyPrey
                    : DevSet.I.simulation.maxEnergyPredator));
        }
        [CanBeNull]
        private Liveable CheckIfThereIsCapableMateInRange(Liveable agent)
        {
            var mate =  Finder.FindNearestMateForAgent(agent);
            if(mate == null)
            {
                return null;
            }

            if (mate.CurrentPosition != agent.CurrentPosition)
            {
                return null;
            }

            foreach (var potentialBreeder in Finder.FindAllMatesInCellForAgent(agent))
            {
                if (!CheckIfLivableHasEnergy(potentialBreeder))
                {
                    continue;
                }
                
                if (!BreedingProcessor.WasBreedingSuccessful(agent.CognitiveMap, potentialBreeder.CognitiveMap))
                {
                    continue;
                }

                if (potentialBreeder.ActedThisTurn)
                {
                    continue;
                }
                
                if (potentialBreeder.currentAction.ActionType != LiveableActionType.Breed)
                {
                    continue;
                }

                return potentialBreeder;
            }

            return null;
        }
    }
}