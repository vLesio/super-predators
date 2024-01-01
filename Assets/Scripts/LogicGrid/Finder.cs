using System;
using System.Collections.Generic;
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
            
            return SimulationGrid.GrassAgents[nearestGrassPosition][0];
        }
        
        public static Prey FindNearestPreyForAgent(CellAgent agent) {
            var oneTimeSearcher = new OneTimeSearcherForAgent(agent, Maps.Prey);
            var nearestPreyPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestPreyPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }
            
            return SimulationGrid.PreyAgents[nearestPreyPosition][0];
        }
        
        public static Predator FindNearestPredatorForAgent(CellAgent agent) {
            var oneTimeSearcher = new OneTimeSearcherForAgent(agent, Maps.Predator);
            var nearestPredatorPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestPredatorPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }
            
            return SimulationGrid.PredatorAgents[nearestPredatorPosition][0];
        }
        
        public static Meat FindNearestMeatForAgent(CellAgent agent) {
            var oneTimeSearcher = new OneTimeSearcherForAgent(agent, Maps.Meat);
            var nearestMeatPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestMeatPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }
            
            return SimulationGrid.ObstacleAgents[nearestMeatPosition][0];
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
                var firstPrey = SimulationGrid.PreyAgents[preyPosition][0];
                var secondPrey = SimulationGrid.PreyAgents[preyPosition][1];

                return firstPrey == prey ? secondPrey : firstPrey;
            }
            
            var oneTimeSearcher = new OneTimeSearcherForAgent(prey, Maps.Prey);
            var nearestPreyPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestPreyPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }
            
            return SimulationGrid.PreyAgents[nearestPreyPosition][0];
        }
        
        private static Predator FindNearestMateForPredator(Predator predator) {
            var predatorPosition = predator.CurrentPosition;
            
            if (SimulationGrid.PredatorAgents[predatorPosition].Count > 1) {
                var firstPredator = SimulationGrid.PredatorAgents[predatorPosition][0];
                var secondPredator = SimulationGrid.PredatorAgents[predatorPosition][1];

                return firstPredator == predator ? secondPredator : firstPredator;
            }
            
            var oneTimeSearcher = new OneTimeSearcherForAgent(predator, Maps.Predator);
            var nearestPredatorPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestPredatorPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }
            
            return SimulationGrid.PredatorAgents[nearestPredatorPosition][0];
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
                return SimulationGrid.PredatorAgents[preyPosition][0];
            }
            
            var oneTimeSearcher = new OneTimeSearcherForAgent(prey, Maps.Predator);
            var nearestPredatorPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestPredatorPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }
            
            return SimulationGrid.PredatorAgents[nearestPredatorPosition][0];
        }
        
        private static Prey FindNearestEnemyForPredator(Predator predator) {
            var predatorPosition = predator.CurrentPosition;
            
            if (SimulationGrid.PreyAgents[predatorPosition].Count > 0) {
                return SimulationGrid.PreyAgents[predatorPosition][0];
            }
            
            var oneTimeSearcher = new OneTimeSearcherForAgent(predator, Maps.Prey);
            var nearestPreyPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestPreyPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }
            
            return SimulationGrid.PreyAgents[nearestPreyPosition][0];
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
        
        private static List<Prey> FindAllMatesInCellForAgent(Prey agent) {
            return SimulationGrid.PreyAgents.ContainsKey(agent.CurrentPosition) ?
                SimulationGrid.PreyAgents[agent.CurrentPosition] : new List<Prey>();
        }
        
        private static List<Predator> FindAllMatesInCellForAgent(Predator agent) {
            return SimulationGrid.PredatorAgents.ContainsKey(agent.CurrentPosition) ?
                SimulationGrid.PredatorAgents[agent.CurrentPosition] : new List<Predator>();
        }
        
        public static Grass FindGrassOnAgentPosition(CellAgent agent) {
            return SimulationGrid.GrassAgents.ContainsKey(agent.CurrentPosition) ?
                SimulationGrid.GrassAgents[agent.CurrentPosition][0] : null;
        }
        
        public static Meat FindMeatOnAgentPosition(CellAgent agent) {
            return SimulationGrid.ObstacleAgents.ContainsKey(agent.CurrentPosition) ?
                SimulationGrid.ObstacleAgents[agent.CurrentPosition][0] : null;
        }
    }
}