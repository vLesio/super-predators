using System;
using System.Collections.Generic;
using System.Linq;
using Agents.LiveableAgents;
using Agents.ResourceAgents;
using Settings;
using UnityEngine;

namespace LogicGrid {
    public static class Simulation {
        public static readonly AllSpeciesOfTypes<Prey> preySpecies = new AllSpeciesOfTypes<Prey>();
        public static readonly AllSpeciesOfTypes<Predator> predatorSpecies = new AllSpeciesOfTypes<Predator>();
        
        private static List<T> GetAllAgentsInIncreasingAgeOrder<T>(Dictionary<Vector2Int, HashSet<T>> agents)
                                                                    where T: Liveable {
            var allAgents = new List<T>();
            
            foreach (var agentList in agents.Values) {
                allAgents.AddRange(agentList);
            }
            
            return allAgents.OrderBy(agent => agent.Age).ToList();
        }
        
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
            var allPreys = GetAllAgentsInIncreasingAgeOrder(SimulationGrid.PreyAgents);
            
            allPreys.ForEach(prey => {
                if (prey.IsDead() || prey.ActedThisTurn) {
                    return;
                }
                
                prey.ChooseAction();
                prey.Act();
                
                if (prey.IsDead()) {
                    return;
                }
                
                // TODO: check if this is correct
                prey.UpdateEnergyAndResetDistanceTravelled();
            });
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
            var allPredators = GetAllAgentsInIncreasingAgeOrder(SimulationGrid.PredatorAgents);

            allPredators.ForEach(predator => {
                if (predator.IsDead() || predator.ActedThisTurn) {
                    return;
                }

                predator.ChooseAction();
                predator.Act();

                if (predator.IsDead()) {
                    return;
                }

                predator.UpdateEnergyAndResetDistanceTravelled();
            });
        }
        
        private static void UpdateDeadLiveables<T>(Dictionary<Vector2Int, HashSet<T>> agents)
                                                   where T: Liveable {
            const int meatCreatedFromDeadAgent = 2;
            
            var emptyPositions = new HashSet<Vector2Int>();
            
            foreach (var agentsSet in agents.Values) {
                var agentsToRemove = agentsSet.Where(agent => agent.IsDead()).ToList();
                
                agentsToRemove.ForEach(agent => {
                    var position = agent.CurrentPosition;

                    if (SimulationGrid.ObstacleAgents.TryGetValue(position, out var meat)) {
                        meat.Quantity += meatCreatedFromDeadAgent;
                    }
                    else {
                        SimulationGrid.SetMeat(position, meatCreatedFromDeadAgent);
                    }

                    agentsSet.Remove(agent);

                    if (agentsSet.Count == 0) {
                        emptyPositions.Add(agent.CurrentPosition);
                    }
                });
            }
            
            foreach (var emptyPosition in emptyPositions) {
                agents.Remove(emptyPosition);
            }
        }
        
        private static void UpdateDeadLiveables() {
            UpdateDeadLiveables(SimulationGrid.PreyAgents);
            UpdateDeadLiveables(SimulationGrid.PredatorAgents);
        }
        
        private static void UpdatePreySpecies() {
            preySpecies.Reset();
            
            var allPreys = GetAllAgentsInIncreasingAgeOrder(SimulationGrid.PreyAgents);

            allPreys.ForEach(prey => {
                preySpecies.AddAgent(prey);
            });
        }
        
        private static void UpdatePredatorSpecies() {
            predatorSpecies.Reset();
            
            var allPredators = GetAllAgentsInIncreasingAgeOrder(SimulationGrid.PredatorAgents);

            allPredators.ForEach(predator => {
                predatorSpecies.AddAgent(predator);
            });
        }
        
        private static void UpdateLocallyResourceAgents<T>(Dictionary<Vector2Int, T> agents)
                                                            where T: ResourceAgent {
            var emptyPositions = new HashSet<Vector2Int>();
            
            foreach (var meat in agents.Values) {
                meat.UpdateQuantity();

                if (meat.IsEmpty()) {
                    emptyPositions.Add(meat.CurrentPosition);
                }
            }
            
            foreach (var emptyPosition in emptyPositions) {
                SimulationGrid.RemoveMeat(emptyPosition);
            }
        }
        
        private static void UpdateGrassNeighbours() {
            var placesToPlantGrass = new HashSet<Vector2Int>();
            
            foreach (var grass in SimulationGrid.GrassAgents.Values) {
                var position = grass.CurrentPosition;
                var neighbours = SimulationGrid.GetNeighbours(position);
                
                neighbours.ForEach(neighbourPosition => {
                    if (SimulationGrid.GrassAgents.ContainsKey(neighbourPosition)) {
                        return;
                    }
                    
                    placesToPlantGrass.Add(neighbourPosition);
                });
            }

            foreach (var position in placesToPlantGrass) {
                if (!SimulationGrid.GrassAgents.ContainsKey(position)) {
                    SimulationGrid.SetGrass(position, DevSet.I.simulation.growGrass);
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
        
        private static void UpdateAgentsAgeAndResetDistanceTravelled<T>(Dictionary<Vector2Int, HashSet<T>> agents)
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
        
        private static void ResetActedThisTurnInAllAgents() {
            foreach (var preyList in SimulationGrid.PreyAgents.Values) {
                foreach (var prey in preyList) {
                    prey.ActedThisTurn = false;
                }
            }
            
            foreach (var predatorList in SimulationGrid.PredatorAgents.Values) {
                foreach (var predator in predatorList) {
                    predator.ActedThisTurn = false;
                }
            }
        }
        
        public static void Update()
        {
            ResetActedThisTurnInAllAgents();
            UpdatePreysStep();
            UpdateDeadLiveables();
            UpdatePredatorsStep();
            UpdateDeadLiveables();
            UpdateSpeciesStep();
            UpdateEnvironmentStep();
            UpdateAgentsAgeAndResetDistanceTravelled();
        }
    }
}