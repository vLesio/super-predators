﻿using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Actions.LiveableActions;
using Agents.LiveableAgents;
using MathNet.Numerics.LinearAlgebra;
using Settings;
using Unity.VisualScripting;

namespace AgentBehaviour.FuzzyCognitiveMapUtilities {
    using ConnectionMatrix = Matrix<double>;
    
    public enum NamedInternalConcept {
        Hunting,
        Fear,
        Hunger,
        SexualNeeds,
        Curiosity,
        Sedentarity,
        Satisfaction,
        Annoyance
    }
    
    /*
     * Concepts order:
     * sensitive concepts
     * actions
     * named internal concepts
     * unnamed internal concepts
     */
    public class FuzzyCognitiveMap {
        private static readonly double Epsilon = 1.0e-6;
        
        private readonly int _countOfNamedInternalConcepts = Enum.GetNames(typeof(NamedInternalConcept)).Length;
        
        private static readonly Random RandomGenerator = new Random();
        
        private readonly int _sensitiveConceptsCount = Enum.GetNames(typeof(LiveableAttribute)).Length;
        
        private readonly Liveable _liveable;
        
        private readonly ConnectionMatrix _connectionMatrix;
        private readonly List<LiveableAction> _actions;
        
        private Vector<double> _conceptsActivation;
        
        public int TotalConceptsCount {
            get;
        }
        
        public int CountOfNonZeroConnections {
            get {
                return this._connectionMatrix.Enumerate().Count(x => Math.Abs(x) < Epsilon);
            }
        }
        
        private FuzzyCognitiveMap(Liveable liveable, int unnamedInternalConceptsCount) {
            this._liveable = liveable;

            this._actions = this._liveable.PossibleActions;
            TotalConceptsCount = _sensitiveConceptsCount + this._actions.Count +
                                 _countOfNamedInternalConcepts + unnamedInternalConceptsCount;
            
            this._connectionMatrix = ConnectionMatrix.Build.Dense(TotalConceptsCount, TotalConceptsCount);
            this._conceptsActivation = Vector<double>.Build.Dense(TotalConceptsCount);
        }

        private FuzzyCognitiveMap(ConnectionMatrix connectionMatrix, Liveable liveable) {
            this._liveable = liveable;
            
            this._actions = this._liveable.PossibleActions;
            TotalConceptsCount = connectionMatrix.RowCount;
            
            this._connectionMatrix = connectionMatrix;
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
                _conceptsActivation[(int) attribute] = _liveable.Attributes[attribute];
            }
        }

        private void _calculateNextActivationVector() {
            this._conceptsActivation = _activationFunction(
                this._connectionMatrix * this._conceptsActivation + this._conceptsActivation
                );
        }
        
        public static FuzzyCognitiveMap operator*(FuzzyCognitiveMap cognitiveMap, double value) {
            return new FuzzyCognitiveMap(cognitiveMap._connectionMatrix * value, cognitiveMap._liveable);
        }
        
        public static FuzzyCognitiveMap operator/(FuzzyCognitiveMap cognitiveMap, double value) {
            return new FuzzyCognitiveMap(cognitiveMap._connectionMatrix / value, cognitiveMap._liveable);
        }
        
        public static FuzzyCognitiveMap operator+(FuzzyCognitiveMap firstCognitiveMap,
                                                    FuzzyCognitiveMap secondCognitiveMap) {
            return new FuzzyCognitiveMap(
                firstCognitiveMap._connectionMatrix + secondCognitiveMap._connectionMatrix,
                firstCognitiveMap._liveable);
        }
        
        public static FuzzyCognitiveMap operator-(FuzzyCognitiveMap firstCognitiveMap,
                                                    FuzzyCognitiveMap secondCognitiveMap) {
            return new FuzzyCognitiveMap(
                firstCognitiveMap._connectionMatrix - secondCognitiveMap._connectionMatrix,
                firstCognitiveMap._liveable);
        }

        public void UpdateState(int iterationsCount) {
            this._performFuzzification();
            
            for (var i = 0; i < iterationsCount; i++) {
                this._calculateNextActivationVector();
            }
        }

        public List<LiveableAction> GetSortedActions()
        {
            return this._actions
                .Select((action, index) => (index, action))
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

        public void MultiplyNamedInternalConcept(NamedInternalConcept concept, double value) {
            var index = _sensitiveConceptsCount + _actions.Count + (int) concept;
            this._conceptsActivation[index] *= value;
        }

        public static FuzzyCognitiveMap InterbreedBrain(Liveable firstParent, Liveable secondParent) {
            var newBrain = new FuzzyCognitiveMap(firstParent, DevSet.I.simulation.cogMapComplexity);

            var firstParentBrain = firstParent.CognitiveMap;
            var secondParentBrain = secondParent.CognitiveMap;
            
            var conceptsCount = newBrain.TotalConceptsCount;
            
            for (var sourceConceptIndex = 0; sourceConceptIndex < conceptsCount; sourceConceptIndex++) {
                var shouldSelectFromFirstParent = RandomGenerator.NextDouble() <= 0.5;
                
                var selectedParent = shouldSelectFromFirstParent ? firstParentBrain : secondParentBrain;
                
                for (var targetConceptIndex = 0; targetConceptIndex < conceptsCount; targetConceptIndex++) {
                    var connectionStrength = selectedParent._connectionMatrix[sourceConceptIndex, targetConceptIndex];
                    var randomValue = RandomGenerator.NextDouble();
                    
                    if (connectionStrength == 0.0) {
                        var childConnectionStrength = (double) connectionStrength;
                        
                        if (randomValue < DevSet.I.simulation.probaMut) {
                            var r = (RandomGenerator.NextDouble() * 2.0 - 1.0) * DevSet.I.simulation.Mut;

                            childConnectionStrength += r;

                            if (Math.Abs(childConnectionStrength) < DevSet.I.simulation.minEdge) {
                                childConnectionStrength = 0.0;
                            }
                        }
                        
                        newBrain._connectionMatrix[sourceConceptIndex, targetConceptIndex] = childConnectionStrength;
                    } else {
                        var childConnectionStrength = (double) connectionStrength;

                        if (randomValue < DevSet.I.simulation.SmallProbaMut) {
                            var r = (RandomGenerator.NextDouble() * 2.0 - 1.0) * DevSet.I.simulation.highMut;
                            
                            childConnectionStrength = r;
                            
                            if (Math.Abs(childConnectionStrength) < DevSet.I.simulation.minEdge) {
                                childConnectionStrength = 0.0;
                            }
                        }
                        
                        newBrain._connectionMatrix[sourceConceptIndex, targetConceptIndex] = childConnectionStrength;
                    }
                }
            }
            
            return newBrain;
        }

        public static FuzzyCognitiveMap Create(Predator predator, int internalConceptsCount) {
            return new FuzzyCognitiveMap(predator, internalConceptsCount);
        }
        
        public static FuzzyCognitiveMap Create(Prey prey, int internalConceptsCount) {
            return new FuzzyCognitiveMap(prey, internalConceptsCount);
        }
    }
}