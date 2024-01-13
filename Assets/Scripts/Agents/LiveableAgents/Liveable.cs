using System;
using System.Collections.Generic;
using AgentBehaviour.FuzzyCognitiveMapUtilities;
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
        private int DistanceTravelledSinceLastUpdate = 0;
        
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
        
        public abstract double GenomeThreshold {
            get;
        }

        public double Speed {
            get => Attributes[LiveableAttribute.Speed];
        }
        
        public double Age {
            get => Attributes[LiveableAttribute.Age];
        }

        public Dictionary<LiveableAttribute, double> Attributes { get; } = new Dictionary<LiveableAttribute, double>();
        public LiveableAction CurrentAction;
        
        private void InitLiveable() {
            Attributes.Add(LiveableAttribute.Age, 1);
            Attributes.Add(LiveableAttribute.Energy, 9999);
            Attributes.Add(LiveableAttribute.Speed, 3);
            Attributes.Add(LiveableAttribute.FoodClose, 5);
            Attributes.Add(LiveableAttribute.FoodFar, 15);
            Attributes.Add(LiveableAttribute.MateClose, 3);
            Attributes.Add(LiveableAttribute.MateFar, 8);
            Attributes.Add(LiveableAttribute.EnemyClose, 5);
            Attributes.Add(LiveableAttribute.EnemyFar, 18);
            Attributes.Add(LiveableAttribute.QuantityOfLocalFood, 0);
            Attributes.Add(LiveableAttribute.QuantityOfLocalMates, 0);
            Attributes.Add(LiveableAttribute.SexualNeeds, 69);
        }
        
        protected Liveable() {
            InitLiveable();
        }
        
        public abstract void UpdateAttributesDependentOnGrid();
        public abstract void UpdateAttributesDependentOnLocalCell();

        public bool IsDead() {
            return Attributes[LiveableAttribute.Energy] <= 0 || Attributes[LiveableAttribute.Age] >= MaxAge;
        }
        
        public void UpdateEnergyAndResetDistanceTravelled() {
            this.Attributes[LiveableAttribute.Energy] -= CognitiveMap.TotalConceptsCount
                                                         + CognitiveMap.CountOfNonZeroConnections * 0.1
                                                         + Math.Pow(DistanceTravelledSinceLastUpdate, 1.4);
            this.DistanceTravelledSinceLastUpdate = 0;
        }

        public void UpdateAttributesDependentOnTime() {
            Attributes[LiveableAttribute.Age] += 1;
        }
        
        public static bool IsPrey(Liveable agent) {
            return agent.GetType() == typeof(Prey);
        }
    }
}