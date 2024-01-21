using System;
using System.IO;
using AgentBehaviour.FuzzyCognitiveMapUtilities;
using AgentBehaviour.GenomeUtilities;
using Agents.LiveableAgents;
using CoinPackage.Debugging;
using JetBrains.Annotations;
using LogicGrid;
using MathNet.Numerics;
using Settings;

namespace Agents.Actions.LiveableActions
{
    public class Breed : LiveableAction
    {
        public Breed()
        {
            ActionType = LiveableActionType.Breed;
        }
        public override bool CheckConditions(Liveable agent)
        {

            if (!CheckIfLivableHasEnergy(agent))
            {
                return false;
            }

            var breedMate = CheckIfThereIsCapableMateInRange(agent);

            return breedMate != null;
        }

        public override void Invoke(Liveable agent)
        {
            var breedMate = CheckIfThereIsCapableMateInRange(agent);
            if (breedMate == null)
            {
                return;
            }

            var child = BreedingProcessor.Interbreed(agent, breedMate);

            switch (child) {
                case Prey prey:
                    SimulationGrid.SpawnPrey(prey, prey.CurrentPosition);
                    break;
                case Predator predator:
                    SimulationGrid.SpawnPredator(predator, predator.CurrentPosition);
                    break;
                default:
                    throw new InvalidDataException("Child is neither prey nor predator!");
            }
            
            agent.CognitiveMap.MultiplyNamedInternalConcept(NamedInternalConcept.SexualNeeds, 0f);
            breedMate.CognitiveMap.MultiplyNamedInternalConcept(NamedInternalConcept.SexualNeeds, 0f);
            CDebug.LogWarning("Breeded succesfull! New Live is coming to town!");
            if (breedMate.GetHashCode() == agent.GetHashCode())
            {
                CDebug.LogError("Breeded with myself!");
            }
            breedMate.ActedThisTurn = true;
        }

        private bool CheckIfLivableHasEnergy(Liveable agent)
        {
            return (agent.Attributes[LiveableAttribute.Energy] > 0.125f * (agent.GetType() == typeof(Prey)
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

            if (mate.Age < (Liveable.IsPrey(mate) ? DevSet.I.simulation.ageInterbreedPrey : DevSet.I.simulation.ageInterbreedPredator))
            {
                return null;
            }

            foreach (var potentialBreeder in Finder.FindAllMatesInCellForAgent(agent))
            {
                if (potentialBreeder == null)
                {
                    continue;
                }
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
                
                if (!agent.SomeoneWantToBreedWithMe)
                {
                    if(potentialBreeder.CurrentAction?.ActionType != LiveableActionType.Breed)
                    {
                        if (!CheckIfAgentsWantToBreedWithMe(agent, potentialBreeder))
                        {
                            continue;
                        }
                    }
                }

                return potentialBreeder;
            }

            return null;
        }

        private bool CheckIfAgentsWantToBreedWithMe(Liveable hornyAgent, Liveable potentialBreeder)
        {
            potentialBreeder.SomeoneWantToBreedWithMe = true;
            potentialBreeder.ChooseAction();
            potentialBreeder.SomeoneWantToBreedWithMe = false;
            return potentialBreeder.CurrentAction.ActionType == LiveableActionType.Breed;
        }
    }
}