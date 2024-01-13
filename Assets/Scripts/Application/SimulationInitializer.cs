using System;
using System.Collections.Generic;
using Agents.LiveableAgents;
using LogicGrid;
using Settings;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Application {
    public class SimulationInitializer {


        public List<(int, int)> GetPreyClusterPoints() {
            var preyClustersCount =
                Math.Ceiling(DevSet.I.simulation.initNbPrey / DevSet.I.simulation.sizeClusterPrey);
            var preyPoints = new List<(int, int)>();

            double step = Math.Sqrt((DevSet.I.simulation.gridSize.x * DevSet.I.simulation.gridSize.y) /
                                    preyClustersCount);
            for (double x = 0; x < DevSet.I.simulation.gridSize.x; x += step) {
                for (double y = 0; y < DevSet.I.simulation.gridSize.y; y += step) {
                    if (preyPoints.Count < preyClustersCount) {
                        preyPoints.Add(((int)x, (int)y));
                    }
                    else {
                        break;
                    }
                }
            }
            return preyPoints;
        }

        public List<(int, int)> GetPredatorClusterPoints() {
            var predatorClustersCount =
                Math.Ceiling(DevSet.I.simulation.initNbPredator / DevSet.I.simulation.sizeClusterPredator);
            var predatorPoints = new List<(int, int)>();
            
            double step = Math.Sqrt((DevSet.I.simulation.gridSize.x  * DevSet.I.simulation.gridSize.y) / predatorClustersCount);
            for (double x = 0; x < DevSet.I.simulation.gridSize.x; x += step) {
                for (double y = 0; y < DevSet.I.simulation.gridSize.y; y += step) {
                    if (predatorPoints.Count < predatorClustersCount) {
                        predatorPoints.Add(((int)x, (int)y));
                    }
                    else {
                        break;
                    }
                }
            }
            return predatorPoints;
        }

        public void SpawnInitialPreyGroups(List<(int, int)> points, float radius) {
            foreach (var valueTuple in points) {
                for (var i = 0; i < DevSet.I.simulation.sizeClusterPrey; i++) {
                    SimulationGrid.SpawnPrey(new Prey(), new Vector2Int(valueTuple.Item1, valueTuple.Item2));
                }
            }
        }
        
        public void SpawnInitialPredatorGroups(List<(int, int)> points, float radius) {
            foreach (var valueTuple in points) {
                for (var i = 0; i < DevSet.I.simulation.sizeClusterPredator; i++) {
                    SimulationGrid.SpawnPredator(new Predator(), new Vector2Int(valueTuple.Item1, valueTuple.Item2));
                }
            }
        }

        public void InitializeGrass() {
            for (var i = 0; i < DevSet.I.simulation.gridSize.x; i++) {
                for (var j = 0; j < DevSet.I.simulation.gridSize.y; j++) {
                    SimulationGrid.SetGrass(new Vector2Int(i, j),
                        Random.Range(-DevSet.I.simulation.maxGrass, DevSet.I.simulation.maxGrass));
                }
            }
        }
    }
}