using System.Collections.Generic;
using AgentBehaviour.QuasiCognitiveMap;
using Agents.Actions.LiveableActions;
using UnityEngine;

namespace Agents.LiveableAgents
{
    public enum LiveableAttribute
    {
        Age,
        Energy,
        Speed,
        FoodClose,
        FoodFar,
        MateClose,
        MateFar,
        EnemyClose,
        EnemyFar,
        QuantityOfLocalFood,
        QuantityOfLocalMates,
        SexualNeeds
    }
    public abstract class Liveable : SimulationAgent
    {
        public abstract List<LiveableAction> PossibleActions {
            get;
        }

        public abstract FuzzyCognitiveMap CognitiveMap {
            get;
        }

        public abstract double BirthEnergy {
            get;
        }
        
        public abstract double MaxBirthEnergy {
            get;
        }
        
        public abstract double MaxAge {
            get;
        }
        
        public abstract double MaxEnergy {
            get;
        }
        
        public abstract Liveable IdenticalLiveable {
            get;
        }
        
        public Dictionary<LiveableAttribute, double> attributes = new Dictionary<LiveableAttribute, double>();
        public LiveableAction currentAction;
        
        public void InitLiveable()
        {
            attributes.Add(LiveableAttribute.Age, 0);
            attributes.Add(LiveableAttribute.Energy, 0);
            attributes.Add(LiveableAttribute.Speed, 0);
            attributes.Add(LiveableAttribute.FoodClose, 0);
            attributes.Add(LiveableAttribute.FoodClose, 0);
            attributes.Add(LiveableAttribute.FoodFar, 0);
            attributes.Add(LiveableAttribute.MateClose, 0);
            attributes.Add(LiveableAttribute.MateFar, 0);
            attributes.Add(LiveableAttribute.EnemyClose, 0);
            attributes.Add(LiveableAttribute.EnemyFar, 0);
            attributes.Add(LiveableAttribute.QuantityOfLocalFood, 0);
            attributes.Add(LiveableAttribute.QuantityOfLocalMates, 0);
            attributes.Add(LiveableAttribute.SexualNeeds, 0);
        }
    }
}