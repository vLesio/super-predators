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
        private static readonly List<LiveableAction> PossibleActionsAtr = new List<LiveableAction>() {
            new SearchForFood(),
            new Eat(),
            new Exploration(),
            new Breed(),
            new Socialization(),
            new Rest(),
            new SearchForPreys()
        };
        
        private static readonly int FarRange = DevSet.I.simulation.distanceVisionPredator;
        private static readonly int NearRange = DevSet.I.simulation.distanceVisionPredator / 2;

        public override List<LiveableAction> PossibleActions => PossibleActionsAtr;
        
        public override double BirthEnergy => DevSet.I.simulation.birthEnergyPredator;
        public override double MaxBirthEnergy => DevSet.I.simulation.birthEnergyPredatorMax;
        public override double MaxAge => DevSet.I.simulation.maxAgePredator;
        public override double MaxEnergy => DevSet.I.simulation.maxEnergyPredator;

        public override Liveable IdenticalLiveable => new Predator();
        public override double GenomeThreshold => DevSet.I.simulation.TPredator;
        
        public Predator() {
            CognitiveMap = FuzzyCognitiveMap.Create(this, DevSet.I.simulation.cogMapComplexity);
        }
        
        public override void UpdateAttributesDependentOnGrid() {
            var agentsInRangeCounter = new AgentsInRangeCounter(AgentsInRangeCounter.PredatorAttributeToMapAdapter, 
                FarRange, NearRange);
            var agentsInRange = agentsInRangeCounter.CountAgentsInRange(CurrentPosition);
            
            agentsInRange.Keys.ToList().ForEach(attribute => {
                Attributes[attribute] = agentsInRange[attribute];
            });
        }
        
        public override void UpdateAttributesDependentOnLocalCell() {
            var countOfLocalMeat = SimulationGrid.ObstacleAgentsAdapter.CountAgentsInPosition(CurrentPosition);
            var countOfLocalPredators = SimulationGrid.PredatorAgentsAdapter.CountAgentsInPosition(CurrentPosition);
            
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