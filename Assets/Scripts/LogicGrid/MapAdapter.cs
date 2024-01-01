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
    }

    public class GrassAgentsAdapter: MapAdapter {
        private readonly Dictionary<Vector2Int, List<Grass>> _grassAgents;
        
        public GrassAgentsAdapter(Dictionary<Vector2Int, List<Grass>> grassAgents) {
            _grassAgents = grassAgents;
        }
        
        public override bool IsPositionOccupied(Vector2Int position) {
            return _grassAgents.ContainsKey(position);
        }
    }
    
    public class MeatAgentsAdapter: MapAdapter {
        private readonly Dictionary<Vector2Int, List<Meat>> _meatAgents;
        
        public MeatAgentsAdapter(Dictionary<Vector2Int, List<Meat>> meatAgents) {
            _meatAgents = meatAgents;
        }
        
        public override bool IsPositionOccupied(Vector2Int position) {
            return _meatAgents.ContainsKey(position);
        }
    }
    
    public class PreyAgentsAdapter: MapAdapter {
        private readonly Dictionary<Vector2Int, List<Prey>> _preyAgents;
        
        public PreyAgentsAdapter(Dictionary<Vector2Int, List<Prey>> preyAgents) {
            _preyAgents = preyAgents;
        }
        
        public override bool IsPositionOccupied(Vector2Int position) {
            return _preyAgents.ContainsKey(position);
        }
    }
    
    public class PredatorAgentsAdapter: MapAdapter {
        private readonly Dictionary<Vector2Int, List<Predator>> _predatorAgents;
        
        public PredatorAgentsAdapter(Dictionary<Vector2Int, List<Predator>> predatorAgents) {
            _predatorAgents = predatorAgents;
        }
        
        public override bool IsPositionOccupied(Vector2Int position) {
            return _predatorAgents.ContainsKey(position);
        }
    }
}