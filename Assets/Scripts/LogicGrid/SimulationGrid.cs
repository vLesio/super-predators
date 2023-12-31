﻿using System.Collections.Generic;
using System.ComponentModel;
using Agents;
using Agents.LiveableAgents;
using Agents.ResourceAgents;
using Settings;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

namespace LogicGrid
{ 

    public static class SimulationGrid {
        public static Vector2Int GridSize => DevSet.I.simulation.gridSize;

        public static readonly Dictionary<Vector2Int, LinkedList<Grass>> GrassAgents = new Dictionary<Vector2Int, LinkedList<Grass>>();
        public static readonly Dictionary<Vector2Int, LinkedList<Prey>> PreyAgents = new Dictionary<Vector2Int, LinkedList<Prey>>();

        public static readonly Dictionary<Vector2Int, LinkedList<Predator>> PredatorAgents =
            new Dictionary<Vector2Int, LinkedList<Predator>>();

        public static readonly Dictionary<Vector2Int, LinkedList<Meat>> ObstacleAgents = new Dictionary<Vector2Int, LinkedList<Meat>>();
        
        public static readonly GrassAgentsAdapter GrassAgentsAdapter = new GrassAgentsAdapter(GrassAgents);
        public static readonly PreyAgentsAdapter PreyAgentsAdapter = new PreyAgentsAdapter(PreyAgents);
        public static readonly PredatorAgentsAdapter PredatorAgentsAdapter = new PredatorAgentsAdapter(PredatorAgents);
        public static readonly MeatAgentsAdapter ObstacleAgentsAdapter = new MeatAgentsAdapter(ObstacleAgents);
        
        private static void MoveAgent<T>(T agent, Vector2Int destination, Dictionary<Vector2Int, LinkedList<T>> agents)
                                            where T: Liveable {
            var oldPosition = agent.CurrentPosition;
            
            if (oldPosition == destination) {
                return;
            }
            
            if (agents.TryGetValue(oldPosition, out var agentsInOldPosition)) {
                agentsInOldPosition.Remove(agent);
            }
            
            if (agents.TryGetValue(destination, out var agentsInDestination)) {
                agentsInDestination.AddLast(agent);
            } else {
                agents.Add(destination, new LinkedList<T>(new[] {agent}));
            }
            
            agent.CurrentPosition = destination;
        }
        
        public static List<Vector2Int> GetNeighbours(Vector2Int position) {
            var neighbours = new List<Vector2Int>();

            for (var i = -1; i <= 1; i++) {
                for (var j = -1; j <= 1; j++) {
                    var neighbour = new Vector2Int(position.x + i, position.y + j);
                    if (neighbour != position && CheckIfDestinationIsInSimulation(neighbour)) {
                        neighbours.Add(neighbour);
                    }
                }
            }
            
            return neighbours;
        }

        public static bool CheckIfDestinationIsInSimulation(Vector2Int destination) {
            return (destination.x >= 0 && 
                    destination.x < GridSize.x && 
                    destination.y >= 0 &&
                    destination.y < GridSize.y);
        }

        public static float DistanceFromAgentToAgent(CellAgent agent1, CellAgent agent2) {
            return Vector2Int.Distance(agent1.CurrentPosition, agent2.CurrentPosition);
        }
        
        public static MapAdapter GetMapAdapterForMap(Maps map) {
            switch (map) {
                case Maps.Grass:
                    return SimulationGrid.GrassAgentsAdapter;
                case Maps.Meat:
                    return SimulationGrid.ObstacleAgentsAdapter;
                case Maps.Prey:
                    return SimulationGrid.PreyAgentsAdapter;
                case Maps.Predator:
                    return SimulationGrid.PredatorAgentsAdapter;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
        
        public static void MoveAgent(Prey agent, Vector2Int destination) {
            MoveAgent(agent, destination, PreyAgents);
        }
        
        public static void MoveAgent(Predator agent, Vector2Int destination) {
            MoveAgent(agent, destination, PredatorAgents);
        }
    }
}