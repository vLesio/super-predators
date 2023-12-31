﻿using System.Collections.Generic;
using AgentBehaviour.QuasiCognitiveMap;
using Agents.Actions.LiveableActions;
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

        public override List<LiveableAction> PossibleActions => PossibleActionsAtr;
        public override FuzzyCognitiveMap CognitiveMap { get; set; }
        
        public override double BirthEnergy => DevSet.I.simulation.birthEnergyPrey;
        public override double MaxBirthEnergy => DevSet.I.simulation.birthEnergyPreyMax;
        public override double MaxAge => DevSet.I.simulation.maxAgePrey;
        public override double MaxEnergy => DevSet.I.simulation.maxEnergyPrey;
        
        public override Liveable IdenticalLiveable => new Prey();

        public Prey() {
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