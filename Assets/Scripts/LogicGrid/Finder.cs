using System;
using System.Collections.Generic;
using System.Linq;
using AgentBehaviour.GenomeUtilities;
using Agents;
using Agents.LiveableAgents;
using Agents.ResourceAgents;
using Unity.VisualScripting;
using UnityEngine;

namespace LogicGrid
{
    public class OneTimeSearcherForAgent {
        public static readonly Vector2Int InvalidPosition = new Vector2Int(-1, -1);
        
        private readonly SearchResult _result;

        public Vector2Int NearestTargetPosition {
            get {
                if (_result == null || _result.Targets.Count == 0) {
                    return new Vector2Int(-1, -1);
                }
                
                return _result.Targets[0];
            }
        }
        
        public OneTimeSearcherForAgent(CellAgent agent, Maps map) {
            var position = agent.CurrentPosition;
            var searcherEntities = new SearcherEntities();
            searcherEntities.SetSeekerPosition(position);

            var mapAdapter = SimulationGrid.GetMapAdapterForMap(map);
            searcherEntities.AddTargetMap(mapAdapter);
            
            var bfsSearcher = new BfsSearcher(searcherEntities);
            _result = bfsSearcher.FindClosestTargets();
        }
    }
    
    public static class Finder {
        private const double MaxProbabilityDelta = 1.0e-7;
        
        private static readonly Dictionary<Maps, int> MapsIndices = new Dictionary<Maps, int>();
        private static readonly SearcherEntities SearcherEntities = new SearcherEntities();
        
        public static Vector2Int GetPositionOfNearestTarget(Maps map, SearchResult result) {
            var mapIndex = MapsIndices[map];
            var targetPosition = result.Targets[mapIndex];
            
            return targetPosition;
        }
        
        static Finder() {
            var grassMapForPreyIndex = SearcherEntities.AddTargetMap(SimulationGrid.GrassAgentsAdapter);
            var meatMapForPreyIndex = SearcherEntities.AddTargetMap(SimulationGrid.ObstacleAgentsAdapter);
            var preyMapForPreyIndex = SearcherEntities.AddTargetMap(SimulationGrid.PreyAgentsAdapter);
            var predatorMapForPreyIndex = SearcherEntities.AddTargetMap(SimulationGrid.PredatorAgentsAdapter);
            
            MapsIndices.Add(Maps.Grass, grassMapForPreyIndex);
            MapsIndices.Add(Maps.Meat, meatMapForPreyIndex);
            MapsIndices.Add(Maps.Prey, preyMapForPreyIndex);
            MapsIndices.Add(Maps.Predator, predatorMapForPreyIndex);
        }

        public static SearchResult FindClosestTargetsForEveryMap(CellAgent agent) {
            var position = agent.CurrentPosition;
            SearcherEntities.SetSeekerPosition(position);
            
            var bfsSearcher = new BfsSearcher(SearcherEntities);
            return bfsSearcher.FindClosestTargets();
        }
        
        public static Grass FindNearestGrassForAgent(CellAgent agent) {
            var oneTimeSearcher = new OneTimeSearcherForAgent(agent, Maps.Grass);
            var nearestGrassPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestGrassPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }

            return SimulationGrid.GrassAgents[nearestGrassPosition].First();
        }
        
        public static Prey FindNearestPreyForAgent(CellAgent agent) {
            var oneTimeSearcher = new OneTimeSearcherForAgent(agent, Maps.Prey);
            var nearestPreyPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestPreyPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }

            return SimulationGrid.PreyAgents[nearestPreyPosition].First();
        }
        
        public static Predator FindNearestPredatorForAgent(CellAgent agent) {
            var oneTimeSearcher = new OneTimeSearcherForAgent(agent, Maps.Predator);
            var nearestPredatorPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestPredatorPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }

            return SimulationGrid.PredatorAgents[nearestPredatorPosition].First();
        }
        
        public static Meat FindNearestMeatForAgent(CellAgent agent) {
            var oneTimeSearcher = new OneTimeSearcherForAgent(agent, Maps.Meat);
            var nearestMeatPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestMeatPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }

            return SimulationGrid.ObstacleAgents[nearestMeatPosition].First();
        }
        
        public static Liveable FindNearestMateForAgent(CellAgent agent) {
            switch (agent) {
                case Prey prey:
                    return FindNearestMateForPrey(prey);
                case Predator predator:
                    return FindNearestMateForPredator(predator);
                default:
                    throw new ArgumentException("Invalid agent type");
            }
        }
        
        private static Prey FindNearestMateForPrey(Prey prey) {
            var preyPosition = prey.CurrentPosition;
            
            if (SimulationGrid.PreyAgents[preyPosition].Count > 1) {
                var filteredPreys = SimulationGrid.PreyAgents[preyPosition]
                    .Where(otherPrey => otherPrey != prey)
                    .ToList();

                var maxBreedingProbability = filteredPreys.Max(otherPrey =>
                    BreedingProcessor.CalculateBreedingProbability(
                        prey.CognitiveMap, otherPrey.CognitiveMap));
                
                return filteredPreys.First(otherPrey =>
                    Math.Abs(
                        BreedingProcessor.CalculateBreedingProbability(
                            prey.CognitiveMap, otherPrey.CognitiveMap) -
                        maxBreedingProbability) <= MaxProbabilityDelta);
            }
            
            var oneTimeSearcher = new OneTimeSearcherForAgent(prey, Maps.Prey);
            var nearestPreyPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestPreyPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }

            return SimulationGrid.PreyAgents[nearestPreyPosition].First();
        }
        
        private static Predator FindNearestMateForPredator(Predator predator) {
            var predatorPosition = predator.CurrentPosition;
            
            if (SimulationGrid.PredatorAgents[predatorPosition].Count > 1) {
                var filteredPredators = SimulationGrid.PredatorAgents[predatorPosition]
                    .Where(otherPredator => otherPredator != predator)
                    .ToList();

                var maxBreedingProbability = filteredPredators.Max(otherPredator =>
                    BreedingProcessor.CalculateBreedingProbability(
                        predator.CognitiveMap, otherPredator.CognitiveMap));
                
                return filteredPredators.First(otherPredator =>
                    Math.Abs(
                        BreedingProcessor.CalculateBreedingProbability(
                            predator.CognitiveMap, otherPredator.CognitiveMap) -
                        maxBreedingProbability) <= MaxProbabilityDelta);
            }
            
            var oneTimeSearcher = new OneTimeSearcherForAgent(predator, Maps.Predator);
            var nearestPredatorPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestPredatorPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }

            return SimulationGrid.PredatorAgents[nearestPredatorPosition].First();
        }
        
        public static Liveable FindNearestEnemyForAgent(CellAgent agent) {
            switch (agent) {
                case Prey prey:
                    return FindNearestEnemyForPrey(prey);
                case Predator predator:
                    return FindNearestEnemyForPredator(predator);
                default:
                    throw new ArgumentException("Invalid agent type");
            }
        }
        
        private static Predator FindNearestEnemyForPrey(Prey prey) {
            var preyPosition = prey.CurrentPosition;
            
            if (SimulationGrid.PredatorAgents[preyPosition].Count > 0) {
                return SimulationGrid.PredatorAgents[preyPosition].First();
            }
            
            var oneTimeSearcher = new OneTimeSearcherForAgent(prey, Maps.Predator);
            var nearestPredatorPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestPredatorPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }

            return SimulationGrid.PredatorAgents[nearestPredatorPosition].First();
        }
        
        private static Prey FindNearestEnemyForPredator(Predator predator) {
            var predatorPosition = predator.CurrentPosition;
            
            if (SimulationGrid.PreyAgents[predatorPosition].Count > 0) {
                return SimulationGrid.PreyAgents[predatorPosition].First();
            }
            
            var oneTimeSearcher = new OneTimeSearcherForAgent(predator, Maps.Prey);
            var nearestPreyPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestPreyPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }

            return SimulationGrid.PreyAgents[nearestPreyPosition].First();
        }
        
        public static List<Liveable> FindAllMatesInCellForAgent(CellAgent agent) {
            // TODO: This should be done in a better way
            
            switch (agent) {
                case Prey prey:
                    return FindAllMatesInCellForAgent(prey).ConvertTo<List<Liveable>>();
                case Predator predator:
                    return FindAllMatesInCellForAgent(predator).ConvertTo<List<Liveable>>();
                default:
                    throw new ArgumentException("Invalid agent type");
            }
        }
        
        private static LinkedList<Prey> FindAllMatesInCellForAgent(Prey agent) {
            return SimulationGrid.PreyAgents.TryGetValue(agent.CurrentPosition, out var preys)
                ? preys : new LinkedList<Prey>();
        }
        
        private static LinkedList<Predator> FindAllMatesInCellForAgent(Predator agent) {
            return SimulationGrid.PredatorAgents.TryGetValue(agent.CurrentPosition, out var predators)
                ? predators : new LinkedList<Predator>();
        }
        
        public static Grass FindGrassOnAgentPosition(CellAgent agent) {
            return SimulationGrid.GrassAgents.TryGetValue(agent.CurrentPosition, out var grasses)
                ? grasses.First() : null;
        }
        
        public static Meat FindMeatOnAgentPosition(CellAgent agent) {
            return SimulationGrid.ObstacleAgents.TryGetValue(agent.CurrentPosition, out var meats)
                ? meats.First() : null;
        }
    }
}