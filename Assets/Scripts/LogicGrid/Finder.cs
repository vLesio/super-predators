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
        private static Dictionary<Maps, int> _mapsIndices = new Dictionary<Maps, int>();
        private static SearcherEntities _searcherEntities = new SearcherEntities();
        
        public static Vector2Int GetPositionOfNearestTarget(Maps map, SearchResult result) {
            var mapIndex = _mapsIndices[map];
            var targetPosition = result.Targets[mapIndex];
            
            return targetPosition;
        }
        
        static Finder() {
            var grassMapForPreyIndex = _searcherEntities.AddTargetMap(SimulationGrid.GrassAgentsAdapter);
            var meatMapForPreyIndex = _searcherEntities.AddTargetMap(SimulationGrid.ObstacleAgentsAdapter);
            var preyMapForPreyIndex = _searcherEntities.AddTargetMap(SimulationGrid.PreyAgentsAdapter);
            var predatorMapForPreyIndex = _searcherEntities.AddTargetMap(SimulationGrid.PredatorAgentsAdapter);
            
            _mapsIndices.Add(Maps.Grass, grassMapForPreyIndex);
            _mapsIndices.Add(Maps.Meat, meatMapForPreyIndex);
            _mapsIndices.Add(Maps.Prey, preyMapForPreyIndex);
            _mapsIndices.Add(Maps.Predator, predatorMapForPreyIndex);
        }

        public static SearchResult FindClosestTargetsForEveryMap(CellAgent agent) {
            var position = agent.CurrentPosition;
            _searcherEntities.SetSeekerPosition(position);
            
            var bfsSearcher = new BfsSearcher(_searcherEntities);
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
        
        public static Prey FindNearestPreyForAgent(CellAgent agent)
        {
            var oneTimeSearcher = new OneTimeSearcherForAgent(agent, Maps.Prey);
            var nearestPreyPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestPreyPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }
            
            return SimulationGrid.PreyAgents[nearestPreyPosition][0];
        }
        
        public static Predator FindNearestPredatorForAgent(CellAgent agent)
        {
            var oneTimeSearcher = new OneTimeSearcherForAgent(agent, Maps.Predator);
            var nearestPredatorPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestPredatorPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }
            
            return SimulationGrid.PredatorAgents[nearestPredatorPosition][0];
        }
        
        public static Meat FindNearestMeatForAgent(CellAgent agent)
        {
            var oneTimeSearcher = new OneTimeSearcherForAgent(agent, Maps.Meat);
            var nearestMeatPosition = oneTimeSearcher.NearestTargetPosition;
            
            if (nearestMeatPosition == OneTimeSearcherForAgent.InvalidPosition) {
                return null;
            }
            
            return SimulationGrid.ObstacleAgents[nearestMeatPosition][0];
        }
        
        public static Liveable FindNearestMateForAgent(CellAgent agent)
        {
            return null;
        }
        
        public static Liveable FindNearestEnemyForAgent(CellAgent agent)
        {
            return null;
        }
        
        public static List<Liveable> FindAllMatesInCellForAgent(CellAgent agent)
        {
            return null;
        }
        
        public static Grass FindGrassOnAgentPosition(CellAgent agent)
        {
            return null;
        }
        
        public static Meat FindMeatOnAgentPosition(CellAgent agent)
        {
            return null;
        }
    }
}