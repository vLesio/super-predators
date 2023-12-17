using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Actions.LiveableActions;
using Agents.LiveableAgents;
using MathNet.Numerics.LinearAlgebra;
using Settings;
using Unity.VisualScripting;

namespace AgentBehaviour.QuasiCognitiveMap {
    public class FuzzyCognitiveMap {
        private readonly int _sensitiveConceptsCount = Enum.GetNames(typeof(LiveableAttribute)).Length;
        
        private readonly Liveable _liveable;
        
        private Matrix<double> _connectionMatrix;
        private readonly List<LiveableAction> _actions;
        
        private Vector<double> _conceptsActivation;
        
        private int TotalConceptsCount {
            get;
        }
        
        private FuzzyCognitiveMap(Liveable liveable, int internalConceptsCount) {
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

        private void _performFuzzification()
        {
            var attributesValues = (LiveableAttribute[]) Enum.GetValues(typeof(LiveableAttribute));

            foreach (var attribute in attributesValues) {
                _conceptsActivation[(int) attribute] = _liveable.attributes[attribute];
            }
        }

        private void _calculateNextActivationVector() {
            this._conceptsActivation = _activationFunction(this._connectionMatrix * this._conceptsActivation);
        }

        public void UpdateState() {
            this._performFuzzification();
            this._calculateNextActivationVector();
        }

        public List<LiveableAction> GetSortedActions()
        {
            return this._actions
                .Select((action, index) => (index, action))
                .ToList()
                .OrderBy(pair => this._conceptsActivation[_sensitiveConceptsCount + pair.index])
                .Select(pair => pair.action)
                .ToList();
        }

        public double CalculateDistance(FuzzyCognitiveMap other) {
            var difference = this._connectionMatrix - other._connectionMatrix;

            return difference
                .Map(Math.Abs)
                .ColumnSums()
                .Sum();
        }

        public FuzzyCognitiveMap InterbreedBrain(FuzzyCognitiveMap other) {
            throw new NotImplementedException();
        }
        

        public static FuzzyCognitiveMap Create(Predator predator, int internalConceptsCount) {
            return new FuzzyCognitiveMap(predator, internalConceptsCount);
        }
        
        public static FuzzyCognitiveMap Create(Prey prey, int internalConceptsCount) {
            return new FuzzyCognitiveMap(prey, internalConceptsCount);
        }
    }
}