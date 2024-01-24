using System;
using System.Collections.Generic;
using System.Linq;
using Agents.LiveableAgents;
using LogicGrid;
using Settings;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Application {
    public class SimulationInitializer {
        private List<int> Sample(int min, int max, int count) {
            var numbers = new List<int>();
            
            for (var i = min; i < max; i++) {
                numbers.Add(i);
            }
            
            var result = new List<int>();

            for (var i = 0; i < count && i < numbers.Count; i++) {
                var index = Random.Range(i, numbers.Count);
                (numbers[i], numbers[index]) = (numbers[index], numbers[i]);
                
                var number = numbers[i];
                result.Add(number);
            }
            
            return result;
        }

        private List<(int, int)> SamplePoints(Vector2Int minPoint, Vector2Int maxPoint, int count) {
            var deltaX = maxPoint.x - minPoint.x;
            var deltaY = maxPoint.y - minPoint.y;
            
            var area = deltaX * deltaY;
            var indices = Sample(0, area, count);

            var points = indices
                .Select((index) =>
                    (minPoint.x + index % deltaX, minPoint.y + index / deltaX)
                ).ToList();
            
            return points;
        }

        public List<(int, int)> GetPreyClusterPoints() {
            var preyClustersCount =
                Math.Ceiling(DevSet.I.simulation.initNbPrey / DevSet.I.simulation.sizeClusterPrey);
            var preyPoints = SamplePoints(Vector2Int.zero,
                DevSet.I.simulation.gridSize, (int)preyClustersCount);

            return preyPoints;
        }

        public List<(int, int)> GetPredatorClusterPoints() {
            var predatorClustersCount =
                Math.Ceiling(DevSet.I.simulation.initNbPredator / DevSet.I.simulation.sizeClusterPredator);
            var predatorPoints = SamplePoints(Vector2Int.zero,
                DevSet.I.simulation.gridSize, (int)predatorClustersCount);
            
            return predatorPoints;
        }

        public void SpawnInitialPreyGroups(List<(int, int)> points, float radius) {
            foreach (var valueTuple in points) {
                SpawnOneInitialPreyGroup(valueTuple, radius);
            }
        }
        
        public void SpawnInitialPredatorGroups(List<(int, int)> points, float radius) {
            foreach (var valueTuple in points) {
                SpawnOneInitialPredatorGroup(valueTuple, radius);
            }
        }
        
        private (Vector2Int, Vector2Int) GetClampedPoints((int, int) point, float radius) {
            var minPoint = new Vector2Int(point.Item1 - (int)radius, point.Item2 - (int)radius);
            var maxPoint = new Vector2Int(point.Item1 + (int)radius, point.Item2 + (int)radius);
            
            var minPointClamped = new Vector2Int(
                Mathf.Clamp(minPoint.x, 0, DevSet.I.simulation.gridSize.x),
                Mathf.Clamp(minPoint.y, 0, DevSet.I.simulation.gridSize.y)
            );
            var maxPointClamped = new Vector2Int(
                Mathf.Clamp(maxPoint.x, 0, DevSet.I.simulation.gridSize.x),
                Mathf.Clamp(maxPoint.y, 0, DevSet.I.simulation.gridSize.y)
            );

            return (minPointClamped, maxPointClamped);
        }

        private void SpawnOneInitialPreyGroup((int, int) point, float radius) {
            var (minPointClamped, maxPointClamped) = GetClampedPoints(point, radius);

            for (var i = 0; i < DevSet.I.simulation.sizeClusterPrey; i++) {
                var randomPosition = new Vector2Int(
                    Random.Range(minPointClamped.x, maxPointClamped.x),
                    Random.Range(minPointClamped.y, maxPointClamped.y)
                );
                
                SimulationGrid.SpawnPrey(new Prey(), randomPosition);
            }
        }
        
        private void SpawnOneInitialPredatorGroup((int, int) point, float radius) {
            var (minPointClamped, maxPointClamped) = GetClampedPoints(point, radius);

            for (var i = 0; i < DevSet.I.simulation.sizeClusterPredator; i++) {
                var randomPosition = new Vector2Int(
                    Random.Range(minPointClamped.x, maxPointClamped.x),
                    Random.Range(minPointClamped.y, maxPointClamped.y)
                );
                
                SimulationGrid.SpawnPredator(new Predator(), randomPosition);
            }
        }

        public void InitializeGrass() {
            for (var i = 0; i < DevSet.I.simulation.gridSize.x; i++) {
                for (var j = 0; j < DevSet.I.simulation.gridSize.y; j++) {
                    if (Random.Range(0f, 1f) <= DevSet.I.simulation.probaGrass) {
                        SimulationGrid.SetGrass(new Vector2Int(i, j),
                            Random.Range(1f, DevSet.I.simulation.maxGrass));
                    }
                }
            }
        }
    }
}