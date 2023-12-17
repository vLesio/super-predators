using System;
using System.Collections.Generic;
using Agents.Actions.LiveableActions;
using Agents.LiveableAgents;
using MathNet.Numerics.LinearAlgebra;
using Settings;

namespace AgentBehaviour.QuasiCognitiveMap {
    public class QuasiCognitiveMap {
        private readonly int _sensitiveConceptsCount = Enum.GetNames(typeof(LiveableAttribute)).Length;
        
        private Liveable _liveable;
        
        private Matrix<double> _connectionMatrix;
        private List<LiveableAction> _actions;
        
        private Vector<double> _conceptsActivation;
        
        private int TotalConceptsCount {
            get;
        }
        
        private QuasiCognitiveMap(Liveable liveable, int internalConceptsCount) {
            this._liveable = liveable;

            this._actions = this._liveable.PossibleActions;
            TotalConceptsCount = _sensitiveConceptsCount + this._actions.Count + internalConceptsCount;
            
            this._connectionMatrix = Matrix<double>.Build.Dense(TotalConceptsCount, TotalConceptsCount);
            this._conceptsActivation = Vector<double>.Build.Dense(TotalConceptsCount);
        }
        
        private static Vector<double> _activationFunction(Vector<double> vector)
        {
            var settings = DevSet.I.simulation;

            return vector.Map(x =>
            {
                if (x <= settings.activationS1)
                {
                    return 0.0;
                }

                if (x >= settings.activationS2)
                {
                    return 1.0;
                }

                return (x - settings.activationS1) / (settings.activationS2 - settings.activationS1);
            });
        }

        private void _performFuzzification() {
            
        }

        public void UpdateState() {
            
        }
        

        public static QuasiCognitiveMap Create(Predator predator, int internalConceptsCount) {
            return new QuasiCognitiveMap(predator, internalConceptsCount);
        }
        
        public static QuasiCognitiveMap Create(Prey prey, int internalConceptsCount) {
            return new QuasiCognitiveMap(prey, internalConceptsCount);
        }
    }
}