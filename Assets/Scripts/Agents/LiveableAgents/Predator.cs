using System.Collections.Generic;
using System.Linq;
using AgentBehaviour.FuzzyCognitiveMapUtilities;
using Agents.Actions.LiveableActions;
using LogicGrid;
using Settings;

namespace Agents.LiveableAgents
{
    public class Predator : Liveable
    {
        private static readonly Dictionary<MotorConcepts, LiveableAction> PossibleActionsAtr = new Dictionary<MotorConcepts, LiveableAction> {
            {MotorConcepts.SearchForFood, new SearchForFood()},
            {MotorConcepts.Eating, new Eat()},
            {MotorConcepts.Exploration, new Exploration()},
            {MotorConcepts.Breeding, new Breed()},
            {MotorConcepts.Socialization, new Socialization()},
            {MotorConcepts.Resting, new Rest()},
            {MotorConcepts.SearchForPreys, new SearchForPreys()}
        };
        
        private static readonly int FarRange = DevSet.I.simulation.distanceVisionPredator;
        private static readonly int NearRange = DevSet.I.simulation.distanceVisionPredator / 2;

        public override Dictionary<MotorConcepts, LiveableAction> PossibleActions => PossibleActionsAtr;
        
        public override double BirthEnergy => DevSet.I.simulation.birthEnergyPredator;
        public override double MaxBirthEnergy => DevSet.I.simulation.birthEnergyPredatorMax;
        public override double MaxAge => DevSet.I.simulation.maxAgePredator;
        public override double MaxEnergy => DevSet.I.simulation.maxEnergyPredator;

        public override Liveable IdenticalLiveable => new Predator();
        public override double GenomeThreshold => DevSet.I.simulation.TPredator;
        
        public Predator() {
            CognitiveMap = FuzzyCognitiveMap.Create(this, DevSet.I.simulation.cogMapComplexity);
        }
        
        public override void UpdateSensitivesDependentOnGrid() {
            var agentsInRangeCounter = new AgentsInRangeCounterAndFinder(AgentsInRangeCounterAndFinder.PredatorSensitiveConceptToMapAdapter, 
                FarRange, NearRange);
            var minDistanceToAgent = agentsInRangeCounter.FindDistancesToClosestAgents(CurrentPosition);
            
            minDistanceToAgent.Keys.ToList().ForEach(concept => {
                SensitiveConceptsValues[concept] = minDistanceToAgent[concept];
            });
        }
        
        public override void UpdateSensitivesDependentOnLocalCell() {
            var countOfLocalMeat = SimulationGrid.ObstacleAgentsAdapter.CountAgentsInPosition(CurrentPosition);
            var countOfLocalPredators = SimulationGrid.PredatorAgentsAdapter.CountAgentsInPosition(CurrentPosition);

            SensitiveConceptsValues[SensitiveConcepts.QuantityOfLocalFoodLow] = countOfLocalMeat;
            SensitiveConceptsValues[SensitiveConcepts.QuantityOfLocalFoodHigh] = countOfLocalMeat;
            SensitiveConceptsValues[SensitiveConcepts.QuantityOfLocalMateHigh] = countOfLocalPredators;
            SensitiveConceptsValues[SensitiveConcepts.QuantityOfLocalMateLow] = countOfLocalPredators;
            
            Attributes[LiveableAttribute.QuantityOfLocalFood] = countOfLocalMeat;
            Attributes[LiveableAttribute.QuantityOfLocalMates] = countOfLocalPredators;
        }

        public override void ChooseAction()
        {
            foreach (var action in CognitiveMap.GetSortedActions())
            {
                if(action.CheckConditions(this))
                {
                    CurrentAction = action;
                    break;
                }
            }
        }

        public override void Act()
        {
            if (CurrentAction != null)
            {
                CurrentAction.Invoke(this);
                ActedThisTurn = true;
            }
        }
    }
}