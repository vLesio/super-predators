﻿using System.Collections.Generic;
using System.Linq;
using AgentBehaviour.QuasiCognitiveMap;
using Agents.Actions.LiveableActions;
using LogicGrid;
using Settings;

namespace Agents.LiveableAgents
{
    public class Prey : Liveable
    {
        private static readonly List<LiveableAction> PossibleActionsAtr = new List<LiveableAction>() {
            new SearchForFood(),
            new Eat(),
            new Exploration(),
            new Breed(),
            new Socialization(),
            new Rest(),
            new Evasion(),
            new SearchForPreys()
        };
        
        private static readonly int FarRange = DevSet.I.simulation.distanceVisionPrey;
        private static readonly int NearRange = DevSet.I.simulation.distanceVisionPrey / 2;

        public override List<LiveableAction> PossibleActions => PossibleActionsAtr;
        
        public override double BirthEnergy => DevSet.I.simulation.birthEnergyPrey;
        public override double MaxBirthEnergy => DevSet.I.simulation.birthEnergyPreyMax;
        public override double MaxAge => DevSet.I.simulation.maxAgePrey;
        public override double MaxEnergy => DevSet.I.simulation.maxEnergyPrey;
        
        public override Liveable IdenticalLiveable => new Prey();

        public Prey() {
            CognitiveMap = FuzzyCognitiveMap.Create(this, DevSet.I.simulation.cogMapComplexity);
        }
        
        public override void UpdateAttributesDependentOnGrid() {
            var agentsInRangeCounter = new AgentsInRangeCounter(AgentsInRangeCounter.PreyAttributeToMapAdapter, 
                FarRange, NearRange);
            var agentsInRange = agentsInRangeCounter.CountAgentsInRange(CurrentPosition);
            
            agentsInRange.Keys.ToList().ForEach(attribute => {
                Attributes[attribute] = agentsInRange[attribute];
            });
        }
        
        public override void UpdateAttributesDependentOnLocalCell() {
            throw new System.NotImplementedException();
        }

        public override void UpdateAttributesDependentOnTime() {
            throw new System.NotImplementedException();
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