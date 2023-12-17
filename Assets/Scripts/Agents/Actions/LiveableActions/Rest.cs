using Agents.LiveableAgents;

namespace Agents.Actions.LiveableActions
{
    public class Rest : LiveableAction
    {
        public readonly LiveableActionType Type = LiveableActionType.Rest;
        public override bool CheckConditions(Liveable agent)
        {
            throw new System.NotImplementedException();
        }

        public override void Invoke(Liveable agent)
        {
            throw new System.NotImplementedException();
        }
    }
}