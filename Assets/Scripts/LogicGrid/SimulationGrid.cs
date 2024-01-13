﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Agents;
using Agents.LiveableAgents;
using Agents.ResourceAgents;
using GridSystem;
using Settings;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

namespace LogicGrid
{ 

    public static class SimulationGrid {
        public static Vector2Int GridSize => DevSet.I.simulation.gridSize;

        public static readonly Dictionary<Vector2Int, Grass> GrassAgents = new Dictionary<Vector2Int, Grass>();
        public static readonly Dictionary<Vector2Int, LinkedList<Prey>> PreyAgents = new Dictionary<Vector2Int, LinkedList<Prey>>();

        public static readonly Dictionary<Vector2Int, LinkedList<Predator>> PredatorAgents =
            new Dictionary<Vector2Int, LinkedList<Predator>>();

        public static readonly Dictionary<Vector2Int, Meat> ObstacleAgents = new Dictionary<Vector2Int, Meat>();
        
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
            CGrid.I.MoveLiveable(agent, oldPosition, destination);
        }
        
        private static List<T> GetAllAgentsInIncreasingAgeOrder<T>(Dictionary<Vector2Int, LinkedList<T>> agents)
                                                                    where T: Liveable {
            var allAgents = new List<T>();
            
            foreach (var agentList in agents.Values) {
                allAgents.AddRange(agentList);
            }
            
            return allAgents.OrderBy(agent => agent.Age).ToList();
        }

        private static void SetResourceAgent<T>(Vector2Int position, float amount, Dictionary<Vector2Int, T> agents)
            where T: ResourceAgent, new() {
            if (agents.TryGetValue(position, out var agent)) {
                agent.Quantity = amount;
            } else {
                agents.Add(position, new T() {
                    CurrentPosition = position,
                    Quantity = amount
                });
            }
        }
        
        private static void SetLiveable<T>(Vector2Int position, T agent, Dictionary<Vector2Int, LinkedList<T>> agents)
                                            where T: Liveable {
            agent.CurrentPosition = position;
            if (agents.TryGetValue(position, out var agentsInPosition)) {
                agentsInPosition.AddLast(agent);
            } else {
                agents.Add(position, new LinkedList<T>(new[] {agent}));
            }
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

        public static Vector2Int FindRandomDirection()
        {
            var random = new System.Random();
            var randomDirection = new Vector2Int(random.Next(-1, 1), random.Next(-1, 1));
            return randomDirection * (GridSize.x > GridSize.y ? GridSize.x : GridSize.y);
        }

        public static bool CheckIfDestinationIsInSimulation(Vector2Int destination) {
            return (destination.x >= 0 && 
                    destination.x < GridSize.x && 
                    destination.y >= 0 &&
                    destination.y < GridSize.y);
        }
        
        public static Vector2Int FindOppositeDirectionToAgent(CellAgent requestingAgent, CellAgent targetAgent) {
            var direction = targetAgent.CurrentPosition - requestingAgent.CurrentPosition;
            return new Vector2Int(-direction.x, -direction.y);
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
        
        public static List<Prey> GetAllPreysInIncreasingAgeOrder() {
            return GetAllAgentsInIncreasingAgeOrder(PreyAgents);
        }
        
        public static List<Predator> GetAllPredatorsInIncreasingAgeOrder() {
            return GetAllAgentsInIncreasingAgeOrder(PredatorAgents);
        }

        public static void SetGrass(Vector2Int position, float amount) {
            SetResourceAgent(position, amount, GrassAgents);
            CGrid.I.SetGrass(position, amount);
        }
        
        public static void SetMeat(Vector2Int position, float amount) {
            SetResourceAgent(position, amount, ObstacleAgents);
            CGrid.I.SetMeat(position, amount);
        }
        
        public static void SpawnPrey(Prey prey, Vector2Int position) {
            SetLiveable(position, prey, PreyAgents);
            CGrid.I.SpawnLiveable(prey, position);
        }
        
        public static void SpawnPredator(Predator predator, Vector2Int position) {
            SetLiveable(position, predator, PredatorAgents);
            CGrid.I.SpawnLiveable(predator, position);
        }
        
        public static void RemoveGrass(Vector2Int position) {
            GrassAgents.Remove(position);
            CGrid.I.SetGrass(position, 0f);
        }
        
        public static void RemoveMeat(Vector2Int position) {
            ObstacleAgents.Remove(position);
            CGrid.I.SetMeat(position, 0f);
        }
        
        public static void RemovePrey(Prey prey) {
            if (PreyAgents.TryGetValue(prey.CurrentPosition, out var agentsInPosition)) {
                agentsInPosition.Remove(prey);
                CGrid.I.DespawnLiveable(prey, prey.CurrentPosition);
            }
        }
        
        public static void RemovePredator(Predator predator) {
            if (PredatorAgents.TryGetValue(predator.CurrentPosition, out var agentsInPosition)) {
                agentsInPosition.Remove(predator);
                CGrid.I.DespawnLiveable(predator, predator.CurrentPosition);
            }
        }
    }
}