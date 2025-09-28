using UnityEngine;

namespace Phone
{
    public class GainEnergyReel : Reel
    {
        [SerializeField] private float energyRegenAmount = 15.0f;
        public override void OnActivate()
        {
            PlayerManager.Instance?.UpdateEnergy(energyRegenAmount);
        }
    }
}