using System.Collections.Generic;
using Agents;
using Agents.LiveableAgents;
using Agents.ResourceAgents;
using Unity.VisualScripting;
using UnityEngine;

namespace LogicGrid
{
    public static class Finder
    {
        public enum Maps {
            Grass,
            Meat,
            Prey,
            Predator
        }
        
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
        
        public static Grass FindNearestGrassForAgent(CellAgent agent)
        {
            return null;
        }
        
        public static Prey FindNearestPreyForAgent(CellAgent agent)
        {
            return null;
        }
        
        public static Predator FindNearestPredatorForAgent(CellAgent agent)
        {
            return null;
        }
        
        public static Meat FindNearestMeatForAgent(CellAgent agent)
        {
            return null;
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