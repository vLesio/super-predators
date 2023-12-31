using Agents.LiveableAgents;
using Agents.ResourceAgents;
using LogicGrid;
using Settings;

namespace Agents.Actions.LiveableActions
{
    public class Eat : LiveableAction
    {
        public readonly LiveableActionType Type = LiveableActionType.Eat;
        public override bool CheckConditions(Liveable agent)
        {
            ResourceAgent currentGrass = Liveable.IsPrey(agent) ? Finder.FindGrassOnAgentPosition(agent) : Finder.FindMeatOnAgentPosition(agent);
            
            // TODO: PDF BAD HELP
            return currentGrass is { Quantity: >= 1 };
        }

        public override void Invoke(Liveable agent)
        {
            ResourceAgent currentFood = Liveable.IsPrey(agent) ? Finder.FindGrassOnAgentPosition(agent) : Finder.FindMeatOnAgentPosition(agent);
            
            currentFood.Quantity -= 1;
            agent.attributes[LiveableAttribute.Energy] += ResourceAgent.IsGrass(currentFood) ? DevSet.I.simulation.energyGrass : DevSet.I.simulation.energyMeat;
            //TODO: Divide cognitive map for hunger by 4
        }
    }
}