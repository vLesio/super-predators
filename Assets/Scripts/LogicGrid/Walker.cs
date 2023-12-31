using Agents.LiveableAgents;
using UnityEngine;

namespace LogicGrid
{
    public static class Walker
    {
        public static bool TryToMoveTowardsDirections(Liveable agent, Vector2Int destination)
        {
            if (!SimulationGrid.CheckIfDestinationIsInSimulation(destination))
            {
                return false;
            }
            
            MoveTowardsDirection(agent, destination);
            return true;
        }

        public static void MoveTowardsDirection(Liveable agent, Vector2Int destination)
        {
            
        }

        private static bool CheckIfDestinationIsReachableByAgent()
        {
            return true;
        }
    }
}