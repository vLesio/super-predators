﻿using System.Collections.Generic;
using Agents.Actions.LiveableActions;

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
        
        public override void Invoke()
        {
            throw new System.NotImplementedException();
        }
    }
}