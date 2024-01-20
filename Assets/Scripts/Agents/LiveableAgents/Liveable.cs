using System;
using System.Collections.Generic;
using AgentBehaviour.FuzzyCognitiveMapUtilities;
using Agents.Actions.LiveableActions;
using CoinPackage.Debugging;
using LogicGrid;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

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
        public static CLogger AgentsLogger = Loggers.LoggersList[Loggers.LoggerType.AGENTS];
        
        private static readonly Dictionary<SensitiveConcepts, double> SensitiveConceptsValuesAtr = new Dictionary<SensitiveConcepts, double> {
            {SensitiveConcepts.FoeClose, 0},
            {SensitiveConcepts.FoeFar, 0},
            {SensitiveConcepts.PreyClose, 0},
            {SensitiveConcepts.PreyFar, 0},
            {SensitiveConcepts.FoodClose, 0},
            {SensitiveConcepts.FoodFar, 0},
            {SensitiveConcepts.MateClose, 0},
            {SensitiveConcepts.MateFar, 0},
            {SensitiveConcepts.EnergyLow, 0},
            {SensitiveConcepts.EnergyHigh, 0},
            {SensitiveConcepts.QuantityOfLocalFoodLow, 0},
            {SensitiveConcepts.QuantityOfLocalFoodHigh, 0},
            {SensitiveConcepts.QuantityOfLocalMateHigh, 0},
            {SensitiveConcepts.QuantityOfLocalMateLow, 0}
        };

        private int DistanceTravelledSinceLastUpdate = 0;
        
        public Dictionary<SensitiveConcepts, double> SensitiveConceptsValues {
            get => SensitiveConceptsValuesAtr;
        }
        
        public abstract Dictionary<MotorConcepts, LiveableAction> PossibleActions {
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
        }
        
        protected Liveable() {
            InitLiveable();
        }
        
        public abstract void UpdateSensitivesDependentOnGrid();
        public abstract void UpdateSensitivesDependentOnLocalCell();

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

        public override string ToString() {
            string type = GetType() == typeof(Prey) ? "Prey" : "Predator";
            return $"[{type}:{CurrentPosition}]" % Colorize.Green;
        }
    }
}