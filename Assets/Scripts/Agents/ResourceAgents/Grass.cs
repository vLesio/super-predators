using Settings;

namespace Agents.ResourceAgents
{
    public class Grass : ResourceAgent
    {
        public Grass() {
            Quantity = DevSet.I.simulation.growGrass;
        }

        public Grass(float amount) {
            Quantity = amount;
        }
        
        public override void ChooseAction()
        {
            throw new System.NotImplementedException();
        }

        public override void Act()
        {
            throw new System.NotImplementedException();
        }

        public override bool UpdateQuantity() {
            if (Quantity <= 0) {
                return false;
            }

            Quantity += DevSet.I.simulation.growGrass;
            return true;
        }
    }
}