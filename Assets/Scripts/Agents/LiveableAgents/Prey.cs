﻿using System.Collections.Generic;
using System.Linq;
using AgentBehaviour.FuzzyCognitiveMapUtilities;
using Agents.Actions.LiveableActions;
using CoinPackage.Debugging;
using LogicGrid;
using Settings;

namespace Agents.LiveableAgents
{
    public class Prey : Liveable
    {
        private static readonly Dictionary<MotorConcepts, LiveableAction> PossibleActionsAtr = new Dictionary<MotorConcepts, LiveableAction> {
            {MotorConcepts.SearchForFood, new SearchForFood()},
            {MotorConcepts.Eating, new Eat()},
            {MotorConcepts.Exploration, new Exploration()},
            {MotorConcepts.Breeding, new Breed()},
            {MotorConcepts.Socialization, new Socialization()},
            {MotorConcepts.Resting, new Rest()},
            {MotorConcepts.Evasion, new Evasion()},
            {MotorConcepts.SearchForPreys, new SearchForPreys()}
        };
        
        private static readonly int FarRange = DevSet.I.simulation.distanceVisionPrey;
        private static readonly int NearRange = DevSet.I.simulation.distanceVisionPrey / 2;

        public override Dictionary<MotorConcepts, LiveableAction> PossibleActions => PossibleActionsAtr;
        
        public override double BirthEnergy => DevSet.I.simulation.birthEnergyPrey;
        public override double MaxBirthEnergy => DevSet.I.simulation.birthEnergyPreyMax;
        public override double MaxAge => DevSet.I.simulation.maxAgePrey;
        public override double MaxEnergy => DevSet.I.simulation.maxEnergyPrey;
        
        public override Liveable IdenticalLiveable => new Prey();
        public override double GenomeThreshold => DevSet.I.simulation.TPrey;

        public Prey() {
            CognitiveMap = FuzzyCognitiveMap.Create(this);
        }
        
        public override void UpdateSensitivesDependentOnGrid() {
            var agentsInRangeCounter = new AgentsInRangeCounterAndFinder(
                AgentsInRangeCounterAndFinder.PreySensitiveConceptToMapAdapter, 
                FarRange, NearRange);
            var minDistanceToAgent =
                agentsInRangeCounter.FindDistancesToClosestAgents(this, CurrentPosition);
            
            minDistanceToAgent.Keys.ToList().ForEach(concept => {
                SensitiveConceptsValues[concept] = minDistanceToAgent[concept];
            });
        }
        
        public override void UpdateSensitivesDependentOnLocalCell() {
            var countOfLocalGrass = SimulationGrid.GrassAgentsAdapter.CountAgentsInPosition(CurrentPosition);
            var countOfLocalPreys = SimulationGrid.PreyAgentsAdapter.CountAgentsInPosition(CurrentPosition);

            SensitiveConceptsValues[SensitiveConcepts.QuantityOfLocalFoodLow] = countOfLocalGrass;
            SensitiveConceptsValues[SensitiveConcepts.QuantityOfLocalFoodHigh] = countOfLocalGrass;
            SensitiveConceptsValues[SensitiveConcepts.QuantityOfLocalMateHigh] = countOfLocalPreys - 1;
            SensitiveConceptsValues[SensitiveConcepts.QuantityOfLocalMateLow] = countOfLocalPreys - 1;
            
            Attributes[LiveableAttribute.QuantityOfLocalFood] = countOfLocalGrass;
            Attributes[LiveableAttribute.QuantityOfLocalMates] = countOfLocalPreys;
        }
        
        public override void ChooseAction()
        {
            if(AlreadyChosenAction)
            {
                return;
            }
            foreach (var action in CognitiveMap.GetSortedActions())
            {
                if(action.CheckConditions(this))
                {
                    CurrentAction = action;
                    LogAction(CurrentAction);
                    AlreadyChosenAction = true;
                    break;
                }
                
            }
        }

        public override void Act()
        {
            if (CurrentAction != null)
            {
                CurrentAction.Invoke(this);
                agentsLogger.Log($"{this} has performed {CurrentAction} action.");
                ActedThisTurn = true;
            }
            else {
                agentsLogger.Log($"{this} has {"not performed any action" % Colorize.Yellow}.");
            }
        }
        
        public void LogAction(LiveableAction action) {
            if (Simulation.preyActionsTaken.TryGetValue(action, out var value)) {
                Simulation.preyActionsTaken[action] += 1;
            }
            else {
                Simulation.preyActionsTaken.Add(action, 1);
            }
        }
    }
}