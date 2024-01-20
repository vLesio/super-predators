using System;
using System.Collections.Generic;
using System.Linq;
using AgentBehaviour.FuzzyCognitiveMapUtilities;
using Agents.LiveableAgents;
using Settings;
using UnityEngine;

namespace LogicGrid {
    using SensitiveConceptToMapAdapter = Dictionary<SensitiveConcepts, MapAdapter>;
    using SensitiveConceptToRange = Dictionary<SensitiveConcepts, AttributeRange>;
    
    public enum AttributeRange {
        Close,
        Far
    }
    
    public class AgentsInRangeCounterAndFinder {
        public static readonly SensitiveConceptToMapAdapter PreySensitiveConceptToMapAdapter =
            new SensitiveConceptToMapAdapter {
                {SensitiveConcepts.FoeClose, SimulationGrid.PredatorAgentsAdapter},
                {SensitiveConcepts.FoeFar, SimulationGrid.PredatorAgentsAdapter},
                {SensitiveConcepts.PreyClose, SimulationGrid.PreyAgentsAdapter},
                {SensitiveConcepts.PreyFar, SimulationGrid.PreyAgentsAdapter},
                {SensitiveConcepts.FoodClose, SimulationGrid.GrassAgentsAdapter},
                {SensitiveConcepts.FoodFar, SimulationGrid.GrassAgentsAdapter},
                {SensitiveConcepts.MateClose, SimulationGrid.PreyAgentsAdapter},
                {SensitiveConcepts.MateFar, SimulationGrid.PreyAgentsAdapter}
            };
        public static readonly SensitiveConceptToMapAdapter PredatorSensitiveConceptToMapAdapter =
            new SensitiveConceptToMapAdapter {
                {SensitiveConcepts.FoeClose, SimulationGrid.PreyAgentsAdapter},
                {SensitiveConcepts.FoeFar, SimulationGrid.PreyAgentsAdapter},
                {SensitiveConcepts.PreyClose, SimulationGrid.PreyAgentsAdapter},
                {SensitiveConcepts.PreyFar, SimulationGrid.PreyAgentsAdapter},
                {SensitiveConcepts.FoodClose, SimulationGrid.ObstacleAgentsAdapter},
                {SensitiveConcepts.FoodFar, SimulationGrid.ObstacleAgentsAdapter},
                {SensitiveConcepts.MateClose, SimulationGrid.PredatorAgentsAdapter},
                {SensitiveConcepts.MateFar, SimulationGrid.PredatorAgentsAdapter}
            };
        
        private static readonly SensitiveConceptToRange SensitiveConceptToRange =
            new SensitiveConceptToRange {
                {SensitiveConcepts.FoeClose, AttributeRange.Close},
                {SensitiveConcepts.FoeFar, AttributeRange.Far},
                {SensitiveConcepts.PreyClose, AttributeRange.Close},
                {SensitiveConcepts.PreyFar, AttributeRange.Far},
                {SensitiveConcepts.FoodClose, AttributeRange.Close},
                {SensitiveConcepts.FoodFar, AttributeRange.Far},
                {SensitiveConcepts.MateClose, AttributeRange.Close},
                {SensitiveConcepts.MateFar, AttributeRange.Far}
            };

        private readonly int _farRange;
        private readonly int _nearRange;
        private readonly SensitiveConceptToMapAdapter _attributeToMapAdapter;
        
        public AgentsInRangeCounterAndFinder(SensitiveConceptToMapAdapter attributeToMapAdapter, int farRange, int nearRange) {
            this._attributeToMapAdapter = attributeToMapAdapter;
            this._farRange = farRange;
            this._nearRange = nearRange;
        }

        public Dictionary<SensitiveConcepts, int> CountAgentsInRange(Vector2Int position) {
            var counts = new Dictionary<SensitiveConcepts, int>();
            
            SensitiveConceptToRange.Keys.ToList().ForEach(attribute => {
                counts.Add(attribute, 0);
            });

            for (var dx = -_farRange; dx <= _farRange; dx++) {
                for (var dy = -_farRange; dy <= _farRange; dy++) {
                    var neighbourPosition = new Vector2Int(position.x + dx, position.y + dy);
                    var distanceFromCenter = Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy));
                    
                    SensitiveConceptToRange.Keys.ToList().ForEach(concept => {
                        if (SensitiveConceptToRange[concept] != AttributeRange.Far &&
                            distanceFromCenter > _nearRange) {
                            return;
                        }

                        var mapAdapter = _attributeToMapAdapter[concept];
                        counts[concept] += mapAdapter.CountAgentsInPosition(neighbourPosition);
                    });
                }
            }

            return counts;
        }

        public Dictionary<SensitiveConcepts, int> FindDistancesToClosestAgents(Liveable agentToIgnore, Vector2Int position) {
            var conceptToDistance = new Dictionary<SensitiveConcepts, int>();
            
            var searcherEntities = new SearcherEntities(agentToIgnore, position);
            SensitiveConceptToRange.Keys.ToList().ForEach(concept => {
                var mapAdapter = _attributeToMapAdapter[concept];
                searcherEntities.MapAdapters.Add(mapAdapter);
            });
            
            var bfsSearcher = new BfsSearcher(searcherEntities);
            var results = bfsSearcher.FindClosestTargets();
            
            SensitiveConceptToRange.Keys.ToList().ForEach(concept => {
                var resultPosition = results.Targets[(int)concept];
                var dxAbs = Math.Abs(resultPosition.x - position.x);
                var dyAbs = Math.Abs(resultPosition.y - position.y);
                
                var distance = Math.Min(dxAbs, dyAbs) + Math.Abs(dxAbs - dyAbs);
                
                conceptToDistance.Add(concept, distance);
            });
            
            return conceptToDistance;
        }
    }
}