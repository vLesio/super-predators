using System.Collections.Generic;
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
        public abstract int CountAgentsInPosition(Vector2Int position);
    }

    public class GrassAgentsAdapter: MapAdapter {
        private readonly Dictionary<Vector2Int, LinkedList<Grass>> _grassAgents;
        
        public GrassAgentsAdapter(Dictionary<Vector2Int, LinkedList<Grass>> grassAgents) {
            _grassAgents = grassAgents;
        }
        
        public override bool IsPositionOccupied(Vector2Int position) {
            return _grassAgents.ContainsKey(position);
        }
        
        public override int CountAgentsInPosition(Vector2Int position) {
            return _grassAgents.TryGetValue(position, out var agents) ? agents.Count : 0;
        }
    }
    
    public class MeatAgentsAdapter: MapAdapter {
        private readonly Dictionary<Vector2Int, LinkedList<Meat>> _meatAgents;
        
        public MeatAgentsAdapter(Dictionary<Vector2Int, LinkedList<Meat>> meatAgents) {
            _meatAgents = meatAgents;
        }

        public override bool IsPositionOccupied(Vector2Int position) {
            return _meatAgents.ContainsKey(position);
        }
        
        public override int CountAgentsInPosition(Vector2Int position) {
            return _meatAgents.TryGetValue(position, out var agents) ? agents.Count : 0;
        }
    }
    
    public class PreyAgentsAdapter: MapAdapter {
        private readonly Dictionary<Vector2Int, LinkedList<Prey>> _preyAgents;
        
        public PreyAgentsAdapter(Dictionary<Vector2Int, LinkedList<Prey>> preyAgents) {
            _preyAgents = preyAgents;
        }
        
        public override bool IsPositionOccupied(Vector2Int position) {
            return _preyAgents.ContainsKey(position);
        }
        
        public override int CountAgentsInPosition(Vector2Int position) {
            return _preyAgents.TryGetValue(position, out var agents) ? agents.Count : 0;
        }
    }
    
    public class PredatorAgentsAdapter: MapAdapter {
        private readonly Dictionary<Vector2Int, LinkedList<Predator>> _predatorAgents;
        
        public PredatorAgentsAdapter(Dictionary<Vector2Int, LinkedList<Predator>> predatorAgents) {
            _predatorAgents = predatorAgents;
        }
        
        public override bool IsPositionOccupied(Vector2Int position) {
            return _predatorAgents.ContainsKey(position);
        }
        
        public override int CountAgentsInPosition(Vector2Int position) {
            return _predatorAgents.TryGetValue(position, out var agents) ? agents.Count : 0;
        }
    }
}