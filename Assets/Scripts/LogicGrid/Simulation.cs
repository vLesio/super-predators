using System;
using System.Collections.Generic;
using System.Linq;
using Agents.LiveableAgents;
using Agents.ResourceAgents;
using UnityEngine;

namespace LogicGrid {
    public static class Simulation {
        private static void UpdatePerceptionsForPreys() {
            foreach (var preyList in SimulationGrid.PreyAgents.Values) {
                foreach (var prey in preyList) {
                    prey.UpdateAttributesDependentOnGrid();
                    prey.UpdateAttributesDependentOnLocalCell();
                }
            }
        }

        private static void UpdateFuzzyCognitiveMapsForPreys() {
            foreach (var preyList in SimulationGrid.PreyAgents.Values) {
                foreach (var prey in preyList) {
                    prey.CognitiveMap.UpdateState(3);
                }
            }
        }

        private static void UpdateActionsAndEnergyForPreys() {
            throw new NotImplementedException();
        }
        
        private static void UpdatePerceptionsForPredators() {
            foreach (var predatorList in SimulationGrid.PredatorAgents.Values) {
                foreach (var predator in predatorList) {
                    predator.UpdateAttributesDependentOnGrid();
                    predator.UpdateAttributesDependentOnLocalCell();
                }
            }
        }
        
        private static void UpdateFuzzyCognitiveMapsForPredators() {
            foreach (var predatorList in SimulationGrid.PredatorAgents.Values) {
                foreach (var predator in predatorList) {
                    predator.CognitiveMap.UpdateState(3);
                }
            }
        }
        
        private static void UpdateActionsAndEnergyForPredators() {
            throw new NotImplementedException();
        }
        
        private static void UpdatePreySpecies() {
            throw new NotImplementedException();
        }
        
        private static void UpdatePredatorSpecies() {
            throw new NotImplementedException();
        }
        
        private static void UpdateLocallyResourceAgents<T>(Dictionary<Vector2Int, LinkedList<T>> agents)
                                                            where T: ResourceAgent {
            var emptyPositions = new HashSet<Vector2Int>();
            
            foreach (var meatList in agents.Values) {
                var meatNode = meatList.First;

                while (meatNode != null) {
                    var nextMeatNode = meatNode.Next;
                    
                    var meat = meatNode.Value;
                    meat.UpdateQuantity();

                    if (meat.IsEmpty()) {
                        meatList.Remove(meatNode);
                        
                        if (meatList.Count == 0) {
                            emptyPositions.Add(meat.CurrentPosition);
                        }
                    }
                    
                    meatNode = nextMeatNode;
                }
            }
            
            foreach (var emptyPosition in emptyPositions) {
                SimulationGrid.ObstacleAgents.Remove(emptyPosition);
            }
        }
        
        private static void UpdateGrassNeighbours() {
            var placesToPlantGrass = new HashSet<Vector2Int>();
            
            foreach (var grassList in SimulationGrid.GrassAgents.Values) {
                foreach (var grass in grassList) {
                    var position = grass.CurrentPosition;
                    var neighbours = SimulationGrid.GetNeighbours(position);
                    
                    neighbours.ForEach(neighbourPosition => {
                        if (SimulationGrid.GrassAgents.ContainsKey(neighbourPosition)) {
                            return;
                        }
                        
                        placesToPlantGrass.Add(neighbourPosition);
                    });
                }
            }

            foreach (var position in placesToPlantGrass) {
                var grass = new Grass() {
                    CurrentPosition = position
                };
                
                if (SimulationGrid.GrassAgents.TryGetValue(position, out var grassList)) {
                    grassList.AddLast(grass);
                } else {
                    SimulationGrid.GrassAgents.Add(position, new LinkedList<Grass>(new[] {grass}));
                }
            }
        }
        
        private static void UpdateGrass() {
            UpdateLocallyResourceAgents(SimulationGrid.GrassAgents);
            UpdateGrassNeighbours();
        }
        
        private static void UpdateMeat() {
            UpdateLocallyResourceAgents(SimulationGrid.ObstacleAgents);
        }
        
        private static void UpdateAgentsAgeAndResetDistanceTravelled<T>(Dictionary<Vector2Int, LinkedList<T>> agents)
                                               where T: Liveable {
            foreach (var agentList in agents.Values) {
                foreach (var agent in agentList) {
                    agent.UpdateEnergyAndResetDistanceTravelled();
                }
            }
        }
        
        private static void UpdateAgentsAgeAndResetDistanceTravelled() {
            UpdateAgentsAgeAndResetDistanceTravelled(SimulationGrid.PreyAgents);
            UpdateAgentsAgeAndResetDistanceTravelled(SimulationGrid.PredatorAgents);
        }


        private static void UpdatePreysStep() {
            UpdatePerceptionsForPreys();
            UpdateFuzzyCognitiveMapsForPreys();
            UpdateActionsAndEnergyForPreys();
        }
        
        private static void UpdatePredatorsStep() {
            UpdatePerceptionsForPredators();
            UpdateFuzzyCognitiveMapsForPredators();
            UpdateActionsAndEnergyForPredators();
        }
        
        private static void UpdateSpeciesStep() {
            UpdatePreySpecies();
            UpdatePredatorSpecies();
        }
        
        private static void UpdateEnvironmentStep() {
            UpdateGrass();
            UpdateMeat();
        }
        
        public static void Update() {
            UpdatePreysStep();
            UpdatePredatorsStep();
            UpdateSpeciesStep();
            UpdateEnvironmentStep();
            UpdateAgentsAgeAndResetDistanceTravelled();
        }
    }
}