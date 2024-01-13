using System;
using Agents.LiveableAgents;
using UnityEngine;

namespace LogicGrid
{
    public static class Walker {
        public static bool TryToMoveTowardsDirections(Liveable agent, Vector2Int destination) {
            if (!SimulationGrid.CheckIfDestinationIsInSimulation(destination)) {
                return false;
            }
            
            MoveTowardsDirection(agent, destination);
            return true;
        }

        private static Vector2Int CalculatePositionAfterMove(Liveable agent, Vector2Int destination) {
            var agentPosition = agent.CurrentPosition;
            var difference = destination - agentPosition;
            
            var absDx = Math.Abs(difference.x);
            var absDy = Math.Abs(difference.y);
            
            var minPart = Math.Min(absDx, absDy);
            var restPart = Math.Abs(absDx - absDy);
            var distance = minPart + restPart;
            
            var speed = (int) agent.Speed;
            var direction = new Vector2Int(Math.Sign(difference.x), Math.Sign(difference.y));
            
            if (speed < minPart) {
                return agent.CurrentPosition + direction * speed;
            } else if (speed < distance) {
                var restSpeed = speed - minPart;
                
                var restDirection = (absDx > absDy) ?
                    new Vector2Int(direction.x, 0) :
                    new Vector2Int(0, direction.y);
                
                return agent.CurrentPosition + direction * minPart + restDirection * restSpeed;
            }
            return destination;
        }
        
        private static void MoveTowardsDirection(Liveable agent, Vector2Int destination) {
            var newPosition = CalculatePositionAfterMove(agent, destination);
            
            if (agent.CurrentPosition == newPosition) {
                return;
            }

            switch (agent) {
                case Prey prey:
                    SimulationGrid.MoveAgent(prey, newPosition);
                    break;
                case Predator predator:
                    SimulationGrid.MoveAgent(predator, newPosition);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(agent));
            }
        }

        private static bool CheckIfDestinationIsReachableByAgent(Liveable agent, Vector2Int destination) {
            return agent.CurrentPosition == CalculatePositionAfterMove(agent, destination);
        }
    }
}