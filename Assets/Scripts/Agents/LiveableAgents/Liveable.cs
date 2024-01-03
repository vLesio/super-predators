using System.Collections.Generic;
using AgentBehaviour.QuasiCognitiveMap;
using Agents.Actions.LiveableActions;
using LogicGrid;
using Unity.VisualScripting;
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
        SexualNeeds,
        MaxAge
    }
    public abstract class Liveable : SimulationAgent
    {
        public abstract List<LiveableAction> PossibleActions {
            get;
        }

        public FuzzyCognitiveMap CognitiveMap {
            get;
            set;
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

        public double Speed {
            get => Attributes[LiveableAttribute.Speed];
        }

        public Dictionary<LiveableAttribute, double> Attributes { get; } = new Dictionary<LiveableAttribute, double>();
        public LiveableAction CurrentAction;
        
        protected Liveable() {
            InitLiveable();
        }
        
        public abstract void UpdateAttributesDependentOnGrid();
        public abstract void UpdateAttributesDependentOnLocalCell();
        public abstract void UpdateAttributesDependentOnTime();
        
        public void InitLiveable() {
            Attributes.Add(LiveableAttribute.Age, 0);
            Attributes.Add(LiveableAttribute.Energy, 0);
            Attributes.Add(LiveableAttribute.Speed, 0);
            Attributes.Add(LiveableAttribute.FoodClose, 0);
            Attributes.Add(LiveableAttribute.FoodClose, 0);
            Attributes.Add(LiveableAttribute.FoodFar, 0);
            Attributes.Add(LiveableAttribute.MateClose, 0);
            Attributes.Add(LiveableAttribute.MateFar, 0);
            Attributes.Add(LiveableAttribute.EnemyClose, 0);
            Attributes.Add(LiveableAttribute.EnemyFar, 0);
            Attributes.Add(LiveableAttribute.QuantityOfLocalFood, 0);
            Attributes.Add(LiveableAttribute.QuantityOfLocalMates, 0);
            Attributes.Add(LiveableAttribute.SexualNeeds, 0);
        }
        
        public static bool IsPrey(Liveable agent) {
            return agent.GetType() == typeof(Prey);
        }
    }
}