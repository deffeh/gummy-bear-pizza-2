using UnityEngine;

namespace Phone
{
    public class GainEnergyReel : Reel
    {
        [SerializeField] public static float energyRegenAmount = 15.0f;
        public override void OnActivate()
        {
            ActivateText = "+Energy";
            base.OnActivate();
            PlayerManager.Instance?.UpdateEnergy(energyRegenAmount);
        }
    }
}