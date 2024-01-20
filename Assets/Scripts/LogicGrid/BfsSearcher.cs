using System.Collections.Generic;
using System.Linq;
using Agents.LiveableAgents;
using Settings;
using UnityEngine;

namespace LogicGrid {
    public class BfsSearcher {
        private readonly SearcherEntities _entities;

        private HashSet<int> GetAllMapsIndices() {
            var mapIndices = new HashSet<int>();
            
            for (var i = 0; i < _entities.MapAdapters.Count; i++) {
                mapIndices.Add(i);
            }
            
            return mapIndices;
        }
        
        public BfsSearcher(SearcherEntities entities) {
            this._entities = entities;
        }

        public SearchResult FindClosestTargets() {
            var queue = new Queue<Vector2Int>();
            
            var distance = new Dictionary<Vector2Int, int>();
            var mapsIndices = GetAllMapsIndices();
            
            var result = new SearchResult(_entities);
            
            queue.Enqueue(_entities.SeekerPosition);
            distance.Add(_entities.SeekerPosition, 0);

            while (queue.Count > 0) {
                var currentPosition = queue.Dequeue();
                
                if (mapsIndices.Count == 0) {
                    break;
                }
                
                mapsIndices.ToList()
                    .ForEach((mapIndex) => {
                        var mapNotEmptyAtPosition = _entities.MapAdapters[mapIndex].IsPositionOccupiedByOther(currentPosition, _entities.AgentToIgnore);
                        
                        if (!mapNotEmptyAtPosition) {
                            return;
                        }
                        
                        result.SetTarget(mapIndex, currentPosition);
                        mapsIndices.Remove(mapIndex);
                    });
                
                var currentDistance = distance[currentPosition];
                var neighbours = SimulationGrid.GetNeighbours(currentPosition);
                
                neighbours.ForEach((neighbour) => {
                    if (distance.ContainsKey(neighbour)) {
                        return;
                    }
                    
                    distance.Add(neighbour, currentDistance + 1);
                    queue.Enqueue(neighbour);
                });
            }
            
            return result;
        }
    }

    public class MapLimits {
        private Vector2Int _minVector = new Vector2Int(int.MaxValue, int.MaxValue);
        private Vector2Int _maxVector = new Vector2Int(int.MinValue, int.MinValue);

        public Vector2Int Min {
            get => _minVector;
        }
        
        public Vector2Int Max {
            get => _maxVector;
        }
        
        public bool IsInside(Vector2Int position) {
            return position.x >= _minVector.x && position.x <= _maxVector.x && position.y >= _minVector.y && position.y <= _maxVector.y;
        }
        
        public void Update(Vector2Int position) {
            _minVector.x = Mathf.Min(_minVector.x, position.x);
            _minVector.y = Mathf.Min(_minVector.y, position.y);
            _maxVector.x = Mathf.Max(_maxVector.x, position.x);
            _maxVector.y = Mathf.Max(_maxVector.y, position.y);
        }
        
        public void Update(MapLimits other) {
            Update(other._minVector);
            Update(other._maxVector);
        }
    }

    public class SearcherEntities {
        private readonly Liveable _agentToIgnore;
        private Vector2Int _seekerPosition;
        private readonly List<MapAdapter> _mapAdapters = new List<MapAdapter>();
        
        public Liveable AgentToIgnore {
            get => _agentToIgnore;
        }
        
        public List<MapAdapter> MapAdapters {
            get => _mapAdapters;
        }

        public Vector2Int SeekerPosition {
            get => _seekerPosition;
        }

        public SearcherEntities(Liveable agentToIgnore, Vector2Int seekerPosition) {
            this._agentToIgnore = agentToIgnore;
            this._seekerPosition = seekerPosition;
        }
        
        public void SetSeekerPosition(Vector2Int newSeekerPosition) {
            this._seekerPosition = newSeekerPosition;
        }
        
        public int AddTargetMap(MapAdapter targetMap) {
            this._mapAdapters.Add(targetMap);
            
            return this._mapAdapters.Count - 1;
        }
    }
    
    public class SearchResult {
        private readonly List<Vector2Int> _targets;
        private readonly List<bool> _wasTargetFound;
        
        public List<Vector2Int> Targets {
            get => _targets;
        }
        
        public List<bool> WasTargetFound {
            get => _wasTargetFound;
        }
        
        public void SetTarget(int mapIndex, Vector2Int targetPosition) {
            this._targets[mapIndex] = targetPosition;
            this._wasTargetFound[mapIndex] = true;
        }

        public SearchResult(SearcherEntities entities) {
            var countOfTargets = entities.MapAdapters.Count;
            this._targets = new List<Vector2Int>();
            this._wasTargetFound = new List<bool>();

            for (var i = 0; i < countOfTargets; i++) {
                this._targets.Add(new Vector2Int(-1, -1));
                this._wasTargetFound.Add(false);
            }
        }
    }
}