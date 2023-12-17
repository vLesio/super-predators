using System.Collections.Generic;
using AgentBehaviour.QuasiCognitiveMap;
using Agents.Actions.LiveableActions;
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

        public override List<LiveableAction> PossibleActions => PossibleActionsAtr;
        public override FuzzyCognitiveMap CognitiveMap { get; }
        
        public override double BirthEnergy => DevSet.I.simulation.birthEnergyPredator;
        public override double MaxBirthEnergy => DevSet.I.simulation.birthEnergyPredatorMax;
        public override double MaxAge => DevSet.I.simulation.maxAgePredator;
        public override double MaxEnergy => DevSet.I.simulation.maxEnergyPredator;

        public override Liveable IdenticalLiveable => new Predator();
        
        public Predator() {
            CognitiveMap = FuzzyCognitiveMap.Create(this, DevSet.I.simulation.cogMapComplexity);
        }

        public override void ChooseAction()
        {
            throw new System.NotImplementedException();
        }

        public override void Act()
        {
            throw new System.NotImplementedException();
        }
    }
}