using System.Collections.Generic;
using System.Linq;
using Settings;
using UnityEngine;

namespace LogicGrid {
    public class BfsSearcher {
        private readonly SearcherEntities _entities;
        
        private static bool IsInside(Vector2Int position) {
            var gridSize = DevSet.I.simulation.gridSize;
            
            return position.x >= 0 && position.x < gridSize.x && position.y >= 0 && position.y < gridSize.y;
        }
        
        private List<Vector2Int> GetNeighbours(Vector2Int position) {
            var neighbours = new List<Vector2Int>();

            for (var i = -1; i <= 1; i++) {
                for (var j = -1; j <= 1; j++) {
                    var neighbour = new Vector2Int(position.x + i, position.y + j);
                    if (neighbour != position && IsInside(neighbour)) {
                        neighbours.Add(neighbour);
                    }
                }
            }
            
            return neighbours;
        }

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
                        var mapNotEmptyAtPosition = _entities.MapAdapters[mapIndex].IsPositionOccupied(currentPosition);
                        
                        if (!mapNotEmptyAtPosition) {
                            return;
                        }
                        
                        result.SetTarget(mapIndex, currentPosition);
                        mapsIndices.Remove(mapIndex);
                    });
                
                var currentDistance = distance[currentPosition];
                var neighbours = GetNeighbours(currentPosition);
                
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
        private Vector2Int _seekerPosition;
        private readonly List<MapAdapter> _mapAdapters = new List<MapAdapter>();
        
        public List<MapAdapter> MapAdapters {
            get => _mapAdapters;
        }

        public Vector2Int SeekerPosition {
            get => _seekerPosition;
        }

        public SearcherEntities(Vector2Int seekerPosition = default) {
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
        
        public List<Vector2Int> Targets {
            get => _targets;
        }
        
        public void SetTarget(int mapIndex, Vector2Int targetPosition) {
            this._targets[mapIndex] = targetPosition;
        }

        public SearchResult(SearcherEntities entities) {
            var countOfTargets = entities.MapAdapters.Count;
            this._targets = new List<Vector2Int>(countOfTargets);
        }
    }
}