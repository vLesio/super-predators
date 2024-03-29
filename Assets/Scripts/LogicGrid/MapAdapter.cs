using System;
using System.Collections.Generic;
using Agents;
using Agents.LiveableAgents;
using Agents.ResourceAgents;
using UnityEngine;

namespace LogicGrid {
    public enum Maps {
        Grass,
        Meat,
        Prey,
        Predator
    }
    
    public abstract class MapAdapter {
        public abstract bool IsPositionOccupied(Vector2Int position);
        public abstract bool IsPositionOccupiedByOther(Vector2Int position, Liveable agent);
        public abstract int CountAgentsInPosition(Vector2Int position);
    }

    public class GrassAgentsAdapter: MapAdapter {
        private readonly Dictionary<Vector2Int, Grass> _grassAgents;
        
        public GrassAgentsAdapter(Dictionary<Vector2Int, Grass> grassAgents) {
            _grassAgents = grassAgents;
        }
        
        public override bool IsPositionOccupied(Vector2Int position) {
            return _grassAgents.ContainsKey(position) && _grassAgents[position].IsUseful();
        }
        
        public override bool IsPositionOccupiedByOther(Vector2Int position, Liveable agent) {
            return IsPositionOccupied(position);
        }
        
        public override int CountAgentsInPosition(Vector2Int position) {
            return _grassAgents.TryGetValue(position, out var grass) ? (int) Math.Ceiling(grass.Quantity) : 0;
        }
    }
    
    public class MeatAgentsAdapter: MapAdapter {
        private readonly Dictionary<Vector2Int, Meat> _meatAgents;
        
        public MeatAgentsAdapter(Dictionary<Vector2Int, Meat> meatAgents) {
            _meatAgents = meatAgents;
        }

        public override bool IsPositionOccupied(Vector2Int position) {
            return _meatAgents.ContainsKey(position) && _meatAgents[position].IsUseful();
        }
        
        public override bool IsPositionOccupiedByOther(Vector2Int position, Liveable agent) {
            return IsPositionOccupied(position);
        }
        
        public override int CountAgentsInPosition(Vector2Int position) {
            return _meatAgents.TryGetValue(position, out var meat) ? (int) Math.Ceiling(meat.Quantity) : 0;
        }
    }
    
    public class PreyAgentsAdapter: MapAdapter {
        private readonly Dictionary<Vector2Int, HashSet<Prey>> _preyAgents;
        
        public PreyAgentsAdapter(Dictionary<Vector2Int, HashSet<Prey>> preyAgents) {
            _preyAgents = preyAgents;
        }
        
        public override bool IsPositionOccupied(Vector2Int position) {
            return _preyAgents.ContainsKey(position);
        }
        
        public override bool IsPositionOccupiedByOther(Vector2Int position, Liveable agent) {
            if (!_preyAgents.ContainsKey(position)) {
                return false;
            }

            switch (agent) {
                case Prey prey:
                    return _preyAgents[position].Count > 1 || !_preyAgents[position].Contains(prey);
                default:
                    return true;
            }
        }
        
        public override int CountAgentsInPosition(Vector2Int position) {
            return _preyAgents.TryGetValue(position, out var agents) ? agents.Count : 0;
        }
    }
    
    public class PredatorAgentsAdapter: MapAdapter {
        private readonly Dictionary<Vector2Int, HashSet<Predator>> _predatorAgents;
        
        public PredatorAgentsAdapter(Dictionary<Vector2Int, HashSet<Predator>> predatorAgents) {
            _predatorAgents = predatorAgents;
        }
        
        public override bool IsPositionOccupied(Vector2Int position) {
            return _predatorAgents.ContainsKey(position);
        }
        
        public override bool IsPositionOccupiedByOther(Vector2Int position, Liveable agent) {
            if (!_predatorAgents.ContainsKey(position)) {
                return false;
            }

            switch (agent) {
                case Predator predator:
                    return _predatorAgents[position].Count > 1 || !_predatorAgents[position].Contains(predator);
                default:
                    return true;
            }
        }
        
        public override int CountAgentsInPosition(Vector2Int position) {
            return _predatorAgents.TryGetValue(position, out var agents) ? agents.Count : 0;
        }
    }
}