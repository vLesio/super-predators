using System.Collections.Generic;
using System.ComponentModel;
using Agents;
using Agents.Actions.LiveableActions;
using Agents.LiveableAgents;
using Agents.ResourceAgents;
using Settings;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

namespace LogicGrid
{ 

    public static class SimulationGrid
    {
        public static Vector2Int GridSize => DevSet.I.simulation.gridSize;

        public static Dictionary<Vector2Int, List<Grass>> GrassAgents = new Dictionary<Vector2Int, List<Grass>>();
        public static Dictionary<Vector2Int, List<Prey>> PreyAgents = new Dictionary<Vector2Int, List<Prey>>();

        public static Dictionary<Vector2Int, List<Predator>> PredatorAgents =
            new Dictionary<Vector2Int, List<Predator>>();

        public static Dictionary<Vector2Int, List<Meat>> ObstacleAgents = new Dictionary<Vector2Int, List<Meat>>();
        
        public static GrassAgentsAdapter GrassAgentsAdapter = new GrassAgentsAdapter(GrassAgents);
        public static PreyAgentsAdapter PreyAgentsAdapter = new PreyAgentsAdapter(PreyAgents);
        public static PredatorAgentsAdapter PredatorAgentsAdapter = new PredatorAgentsAdapter(PredatorAgents);
        public static MeatAgentsAdapter ObstacleAgentsAdapter = new MeatAgentsAdapter(ObstacleAgents);
        
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

        public static bool CheckIfDestinationIsInSimulation(Vector2Int destination)
        {
            return (destination.x >= 0 && 
                    destination.x < GridSize.x && 
                    destination.y >= 0 &&
                    destination.y < GridSize.y);
        }

        public static float DistanceFromAgentToAgent(CellAgent agent1, CellAgent agent2)
        {
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
    }
}