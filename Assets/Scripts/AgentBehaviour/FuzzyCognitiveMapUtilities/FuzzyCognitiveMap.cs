﻿using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Actions.LiveableActions;
using Agents.LiveableAgents;
using CoinPackage.Debugging;
using MathNet.Numerics.LinearAlgebra;
using Settings;
using Settings.brains;
using Unity.VisualScripting;

namespace AgentBehaviour.FuzzyCognitiveMapUtilities {
    using ConnectionMatrix = Matrix<double>;

    public enum SensitiveConcepts {
        FoeClose = 0,
        FoeFar = 1,
        PreyClose = 2,
        PreyFar = 3,
        FoodClose = 4,
        FoodFar = 5,
        MateClose = 6,
        MateFar = 7,
        EnergyLow = 8,
        EnergyHigh = 9,
        QuantityOfLocalFoodHigh = 10,
        QuantityOfLocalFoodLow = 11,
        QuantityOfLocalMateHigh = 12,
        QuantityOfLocalMateLow = 13
    }
    
    public enum NamedInternalConcept {
        Hunting = 14,
        Fear = 15,
        Hunger = 16,
        SexualNeeds = 17,
        Curiosity = 18,
        Sedentarity = 19,
        Satisfaction = 20,
        Annoyance = 21,
    }

    public enum MotorConcepts {
        Evasion = 22,
        SearchForPreys = 23,
        SearchForFood = 24,
        Socialization = 25,
        Exploration = 26,
        Resting = 27,
        Eating = 28,
        Breeding = 29,
    }
    
    /*
     * Concepts order:
     * sensitive concepts
     * named internal concepts
     * actions
     * unnamed internal concepts
     */
    public class FuzzyCognitiveMap {
        private static readonly double Epsilon = 1.0e-6;

        private readonly Dictionary<SensitiveConcepts, Func<double, double>> _sensitiveConceptToFuzzificationActivationPrey =
            new Dictionary<SensitiveConcepts, Func<double, double>> {
                { SensitiveConcepts.FoeClose, _fuzzificationFunctionNonLocalClose },
                { SensitiveConcepts.FoeFar, _fuzzificationFunctionNonLocalFar },
                { SensitiveConcepts.PreyClose, _fuzzificationFunctionNonLocalClose },
                { SensitiveConcepts.PreyFar, _fuzzificationFunctionNonLocalFar },
                { SensitiveConcepts.FoodClose, _fuzzificationFunctionNonLocalClose },
                { SensitiveConcepts.FoodFar, _fuzzificationFunctionNonLocalFar },
                { SensitiveConcepts.MateClose, _fuzzificationFunctionNonLocalClose },
                { SensitiveConcepts.MateFar, _fuzzificationFunctionNonLocalFar },
                { SensitiveConcepts.EnergyLow, _fuzzificationFunctionEnergyLowPrey },
                { SensitiveConcepts.EnergyHigh, _fuzzificationFunctionEnergyHighPrey },
                { SensitiveConcepts.QuantityOfLocalFoodLow, _fuzzificationFunctionLocalLow },
                { SensitiveConcepts.QuantityOfLocalFoodHigh, _fuzzificationFunctionLocalHigh },
                { SensitiveConcepts.QuantityOfLocalMateHigh, _fuzzificationFunctionLocalMateHigh },
                { SensitiveConcepts.QuantityOfLocalMateLow, _fuzzificationFunctionLocalMateLow }
            };
        
        private readonly Dictionary<SensitiveConcepts, Func<double, double>> _sensitiveConceptToFuzzificationActivationPredator =
            new Dictionary<SensitiveConcepts, Func<double, double>> {
                { SensitiveConcepts.FoeClose, _fuzzificationFunctionNonLocalClose },
                { SensitiveConcepts.FoeFar, _fuzzificationFunctionNonLocalFar },
                { SensitiveConcepts.PreyClose, _fuzzificationFunctionNonLocalClose },
                { SensitiveConcepts.PreyFar, _fuzzificationFunctionNonLocalFar },
                { SensitiveConcepts.FoodClose, _fuzzificationFunctionNonLocalClosePredatorFood },
                { SensitiveConcepts.FoodFar, _fuzzificationFunctionNonLocalFarPredatorFood },
                { SensitiveConcepts.MateClose, _fuzzificationFunctionNonLocalClose },
                { SensitiveConcepts.MateFar, _fuzzificationFunctionNonLocalFar },
                { SensitiveConcepts.EnergyLow, _fuzzificationFunctionEnergyLowPredator },
                { SensitiveConcepts.EnergyHigh, _fuzzificationFunctionEnergyHighPredator },
                { SensitiveConcepts.QuantityOfLocalFoodLow, _fuzzificationFunctionLocalLow },
                { SensitiveConcepts.QuantityOfLocalFoodHigh, _fuzzificationFunctionLocalHigh },
                { SensitiveConcepts.QuantityOfLocalMateHigh, _fuzzificationFunctionLocalMateHigh },
                { SensitiveConcepts.QuantityOfLocalMateLow, _fuzzificationFunctionLocalMateLow }
            };

        private readonly Dictionary<SensitiveConcepts, Func<double, double>> _activeConceptsToFuzzificationActivation;
        
        private readonly int _sensitiveConceptsCount = Enum.GetNames(typeof(SensitiveConcepts)).Length;
        private readonly int _countOfNamedInternalConcepts = Enum.GetNames(typeof(NamedInternalConcept)).Length;
        private readonly int _countOfMotorConcepts = Enum.GetNames(typeof(MotorConcepts)).Length;
        
        private static readonly Random RandomGenerator = new Random();
        
        
        private readonly Liveable _liveable;
        
        private readonly ConnectionMatrix _connectionMatrix;
        private readonly Dictionary<MotorConcepts, LiveableAction> _actions;
        
        private Vector<double> _conceptsActivation;
        
        public int TotalConceptsCount {
            get;
        }
        
        public int CountOfUnnamedInternalConcepts {
            get => this.TotalConceptsCount - this._sensitiveConceptsCount - this._countOfNamedInternalConcepts -
                       this._countOfMotorConcepts;
        }
        
        public int CountOfNonZeroConnections {
            get {
                return this._connectionMatrix.Enumerate().Count(x => Math.Abs(x) > Epsilon);
            }
        }
        
        private FuzzyCognitiveMap(Liveable liveable, int unnamedInternalConceptsCount) {
            this._liveable = liveable;

            this._actions = this._liveable.PossibleActions;
            TotalConceptsCount = _sensitiveConceptsCount + _countOfNamedInternalConcepts +
                                 _countOfMotorConcepts + unnamedInternalConceptsCount;
            
            this._connectionMatrix = ConnectionMatrix.Build.Dense(TotalConceptsCount, TotalConceptsCount);
            this._conceptsActivation = Vector<double>.Build.Dense(TotalConceptsCount);
            
            this._activeConceptsToFuzzificationActivation = liveable switch {
                Predator => _sensitiveConceptToFuzzificationActivationPredator,
                Prey => _sensitiveConceptToFuzzificationActivationPrey,
                _ => throw new ArgumentOutOfRangeException(nameof(liveable))
            };
        }

        private FuzzyCognitiveMap(ConnectionMatrix connectionMatrix, Liveable liveable) {
            this._liveable = liveable;
            
            this._actions = this._liveable.PossibleActions;
            TotalConceptsCount = connectionMatrix.RowCount;
            
            this._connectionMatrix = connectionMatrix;
            this._conceptsActivation = Vector<double>.Build.Dense(TotalConceptsCount);
            
            this._activeConceptsToFuzzificationActivation = liveable switch {
                Predator => _sensitiveConceptToFuzzificationActivationPredator,
                Prey => _sensitiveConceptToFuzzificationActivationPrey,
                _ => throw new ArgumentOutOfRangeException(nameof(liveable))
            };
        }
        
        private static double _generalActivationFunctionAType(double x, double k, double x0) {
            return 1.0 / (1.0 + Math.Exp(-k * (x - x0)));
        }

        private static double _generalActivationFunctionBType(double x, double s1, double s2) {
            if (x <= s1) {
                return 0.0;
            }
            
            if (x >= s2) {
                return 1.0;
            }
            
            return (x - s1) / (s2 - s1);
        }
        
        private static Vector<double> _activationFunction(Vector<double> vector) {
            return vector.Map(x =>
                _generalActivationFunctionAType(x, 1.5, 1.0/2.0)
            );
        }

        private static double _fuzzificationFunctionNonLocalClose(double x) {
            return _generalActivationFunctionBType(20.0 - x, 0.0, 15.0);
        }
        
        private static double _fuzzificationFunctionNonLocalFar(double x) {
            return _generalActivationFunctionBType(x, 5.0, 20.0);
        }
        
        private static double _fuzzificationFunctionNonLocalClosePredatorFood(double x) {
            return _generalActivationFunctionBType(25.0 - x, 0.0, 15.0);
        }
        
        private static double _fuzzificationFunctionNonLocalFarPredatorFood(double x) {
            return _generalActivationFunctionBType(x, 10.0, 25.0);
        }
        
        private static double _fuzzificationFunctionEnergyLowPrey(double x) {
            return _generalActivationFunctionBType(400.0 - x, 0.0, 100.0);
        }
        
        private static double _fuzzificationFunctionEnergyHighPrey(double x) {
            return _generalActivationFunctionBType(x, 300.0, 400.0);
        }
        
        private static double _fuzzificationFunctionEnergyLowPredator(double x) {
            return _generalActivationFunctionBType(700.0 - x, 0.0, 100.0);
        }
        
        private static double _fuzzificationFunctionEnergyHighPredator(double x) {
            return _generalActivationFunctionBType(x, 700.0, 800.0);
        }
        
        private static double _fuzzificationFunctionLocalLow(double x) {
            return _generalActivationFunctionBType(1.0 - x, 0.0, 1.0);
        }
        
        private static double _fuzzificationFunctionLocalHigh(double x) {
            return _generalActivationFunctionBType(x, 0.0, 2.0);
        }
        
        private static double _fuzzificationFunctionLocalMateLow(double x) {
            return _generalActivationFunctionBType(1.0 - x, 0.0, 1.0);
        }
        
        private static double _fuzzificationFunctionLocalMateHigh(double x) {
            return _generalActivationFunctionBType(x, 0.0, 1.0);
        }

        private void _performFuzzification()
        {
            var sensitiveConcepts = _liveable.SensitiveConceptsValues.Keys;

            string sensConceptsStr = $"";
            foreach (var concept in sensitiveConcepts) {
                var fuzzificationFunction = _sensitiveConceptToFuzzificationActivationPrey[concept];
                var fuzzifiedValue = fuzzificationFunction(_liveable.SensitiveConceptsValues[concept]);
                sensConceptsStr +=
                    $"{concept} -> {_liveable.SensitiveConceptsValues[concept]}. Fuzzified: {fuzzifiedValue}\n";
                _conceptsActivation[(int) concept] = fuzzifiedValue;
            }
            // CDebug.Log($"Concepts for {_liveable}\n{sensConceptsStr}");
        }

        private void _calculateNextActivationVector() {
            var multiplication = this._connectionMatrix.Multiply(this._conceptsActivation);
            var afterAdd = multiplication + this._conceptsActivation;
            this._conceptsActivation = _activationFunction(afterAdd);
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
                this._performFuzzification();
                this._calculateNextActivationVector();
                
                
            }
            this._performFuzzification();
        }

        public List<LiveableAction> GetSortedActions() {
            string lol = "";
            for (var i = 0; i < _conceptsActivation.Count; i++) {
                if (i <= 13) {
                    lol += $"{i+1}: {(SensitiveConcepts)i} -> {_conceptsActivation[i]}\n";
                }else if (i <= 21) {
                    lol += $"{i+1}: {(NamedInternalConcept)i} -> {_conceptsActivation[i]}\n";
                }
                else {
                    lol += $"{i+1}: {(MotorConcepts)i} -> {_conceptsActivation[i]}\n";
                }
                
            }
            CDebug.Log($"Activations for {this._liveable}:\n{lol}");

            return this._actions
                .OrderByDescending(x => this._conceptsActivation[(int) x.Key])
                .Select(x => x.Value)
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
            this._conceptsActivation[(int) concept] *= value;
        }

        public static FuzzyCognitiveMap InterbreedBrain(Liveable firstParent, Liveable secondParent, Liveable child) {
            var newBrain = new FuzzyCognitiveMap(child, firstParent.CognitiveMap.CountOfUnnamedInternalConcepts);

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

        public static FuzzyCognitiveMap Create(Predator predator) {
            // return new FuzzyCognitiveMap(predator, internalConceptsCount);
            return new FuzzyCognitiveMap(BrainBase.PredatorMatrix, predator);
        }
        
        public static FuzzyCognitiveMap Create(Prey prey) {
            // return new FuzzyCognitiveMap(prey, internalConceptsCount);
            return new FuzzyCognitiveMap(BrainBase.PreyMatrix, prey);
        }
        
    }
}