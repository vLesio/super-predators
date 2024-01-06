using System.Collections.Generic;
using System.Linq;
using AgentBehaviour.QuasiCognitiveMap;
using Agents.LiveableAgents;
using Settings;
using UnityEngine;

namespace LogicGrid {
    public class AllSpeciesOfTypes<T> where T: Liveable {
        private readonly LinkedList<Species<T>> _agentsSpecies = new LinkedList<Species<T>>();
        private readonly Dictionary<T, Species<T>> _agentsSpeciesMap = new Dictionary<T, Species<T>>();

        public Species<T> FindClosestSpecies(T agent) {
            return _agentsSpecies
                .OrderBy(species => species.Distance(agent))
                .First();
        }
        
        public Species<T> GetAgentsCurrentSpecies(T agent) {
            return _agentsSpeciesMap.TryGetValue(agent, out var species) ? species : null;
        }

        public Species<T> AddAgent(T agent) {
            var closestSpecies = FindClosestSpecies(agent);
            var closestSpeciesDistance = closestSpecies.Distance(agent);

            if (closestSpeciesDistance < 2 * agent.GenomeThreshold) {
                closestSpecies.AddAgent(agent);
                _agentsSpeciesMap[agent] = closestSpecies;
                
                return closestSpecies;
            }
            
            var newSpecies = new Species<T>();
            newSpecies.AddAgent(agent);
            _agentsSpecies.AddLast(newSpecies);
            _agentsSpeciesMap[agent] = newSpecies;
            
            return newSpecies;
        }
        
        public bool RemoveAgent(T agent) {
            var species = GetAgentsCurrentSpecies(agent);
            
            if (species == null) {
                return false;
            }
            
            var result = species.RemoveAgent(agent);
            
            if (species.CountOfAgents == 0) {
                _agentsSpecies.Remove(species);
            }
            
            _agentsSpeciesMap.Remove(agent);
            
            return result;
        }
    }
    
    public class Species<T> where T: Liveable {
        private FuzzyCognitiveMap _averageFcm;

        private int _countOfAgents = 0;
        private readonly Dictionary<Vector2Int, List<T>> _agents = new Dictionary<Vector2Int, List<T>>();

        public int CountOfAgents {
            get => _countOfAgents;
        }

        public double Distance(T other) {
            return _averageFcm?.CalculateDistance(other.CognitiveMap) ?? 0;
        }

        public void AddAgent(T agent) {
            if (CountOfAgents == 0) {
                _averageFcm = agent.CognitiveMap;
            } else {
                _averageFcm = (_averageFcm * CountOfAgents + agent.CognitiveMap) / (CountOfAgents + 1);
            }

            var position = agent.CurrentPosition;
            
            if (!_agents.ContainsKey(position)) {
                _agents[position] = new List<T>();
            }
            
            _agents[position].Add(agent);
            _countOfAgents++;
        }

        public bool RemoveAgent(T agent) {
            var position = agent.CurrentPosition;
            
            if (!_agents.ContainsKey(position) || !_agents[position].Remove(agent)) {
                return false;
            }

            if (CountOfAgents == 1) {
                _averageFcm = null;
            } else {
                _averageFcm = (_averageFcm * CountOfAgents - agent.CognitiveMap) / (CountOfAgents - 1);
            }
            
            _countOfAgents--;
            return true;
        }
    }
}