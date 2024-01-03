using System.Collections.Generic;
using System.Linq;
using Agents.LiveableAgents;
using Settings;
using UnityEngine;

namespace LogicGrid {
    using AttributeToMapAdapter = Dictionary<LiveableAttribute, MapAdapter>;
    using AttributeToRange = Dictionary<LiveableAttribute, AttributeRange>;
    
    public enum AttributeRange {
        Close,
        Far
    }
    
    public class AgentsInRangeCounter {
        public static readonly AttributeToMapAdapter PreyAttributeToMapAdapter =
            new AttributeToMapAdapter {
                {LiveableAttribute.FoodClose, SimulationGrid.GrassAgentsAdapter},
                {LiveableAttribute.FoodFar, SimulationGrid.GrassAgentsAdapter},
                {LiveableAttribute.MateClose, SimulationGrid.PreyAgentsAdapter},
                {LiveableAttribute.MateFar, SimulationGrid.PreyAgentsAdapter},
                {LiveableAttribute.EnemyClose, SimulationGrid.PredatorAgentsAdapter},
                {LiveableAttribute.EnemyFar, SimulationGrid.PredatorAgentsAdapter}
            };
        public static readonly AttributeToMapAdapter PredatorAttributeToMapAdapter =
            new AttributeToMapAdapter {
                {LiveableAttribute.FoodClose, SimulationGrid.ObstacleAgentsAdapter},
                {LiveableAttribute.FoodFar, SimulationGrid.ObstacleAgentsAdapter},
                {LiveableAttribute.MateClose, SimulationGrid.PredatorAgentsAdapter},
                {LiveableAttribute.MateFar, SimulationGrid.PredatorAgentsAdapter},
                {LiveableAttribute.EnemyClose, SimulationGrid.PreyAgentsAdapter},
                {LiveableAttribute.EnemyFar, SimulationGrid.PreyAgentsAdapter}
            };
        
        private static readonly AttributeToRange AttributeToRange =
            new AttributeToRange {
                {LiveableAttribute.FoodClose, AttributeRange.Close},
                {LiveableAttribute.FoodFar, AttributeRange.Far},
                {LiveableAttribute.MateClose, AttributeRange.Close},
                {LiveableAttribute.MateFar, AttributeRange.Far},
                {LiveableAttribute.EnemyClose, AttributeRange.Close},
                {LiveableAttribute.EnemyFar, AttributeRange.Far}
            };

        private readonly int _farRange;
        private readonly int _nearRange;
        private readonly AttributeToMapAdapter _attributeToMapAdapter;
        
        public AgentsInRangeCounter(AttributeToMapAdapter attributeToMapAdapter, int farRange, int nearRange) {
            this._attributeToMapAdapter = attributeToMapAdapter;
            this._farRange = farRange;
            this._nearRange = nearRange;
        }

        public Dictionary<LiveableAttribute, int> CountAgentsInRange(Vector2Int position) {
            var counts = new Dictionary<LiveableAttribute, int>();
            
            AttributeToRange.Keys.ToList().ForEach(attribute => {
                counts.Add(attribute, 0);
            });

            for (var dx = -_farRange; dx <= _farRange; dx++) {
                for (var dy = -_farRange; dy <= _farRange; dy++) {
                    var neighbourPosition = new Vector2Int(position.x + dx, position.y + dy);
                    var distanceFromCenter = Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy));
                    
                    AttributeToRange.Keys.ToList().ForEach(attribute => {
                        if (AttributeToRange[attribute] != AttributeRange.Far &&
                            distanceFromCenter > _nearRange) {
                            return;
                        }

                        var mapAdapter = _attributeToMapAdapter[attribute];
                        counts[attribute] += mapAdapter.CountAgentsInPosition(neighbourPosition);
                    });
                }
            }

            return counts;
        }
    }
}