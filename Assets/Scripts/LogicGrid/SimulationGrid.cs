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

    }

}