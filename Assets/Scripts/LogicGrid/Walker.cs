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

        private static void MoveTowardsDirection(Liveable agent, Vector2Int destination) {
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
                agent.CurrentPosition += direction * speed;
            } else if (speed < distance) {
                var restSpeed = speed - minPart;
                
                var restDirection = (absDx > absDy) ?
                    new Vector2Int(direction.x, 0) :
                    new Vector2Int(0, direction.y);
                
                agent.CurrentPosition += direction * minPart + restDirection * restSpeed;
            } else {
                agent.CurrentPosition = destination;
            }
        }

        private static bool CheckIfDestinationIsReachableByAgent() {
            return true;
        }
    }
}