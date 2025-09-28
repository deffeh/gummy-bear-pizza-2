using UnityEngine;

namespace Phone
{
    public class GainMultiplierReel : Reel
    {
        [SerializeField] private float energyRegenAmount = 5.0f;
        public override void OnActivate()
        {
            PlayerManager.Instance?.UpdateEnergy(energyRegenAmount);
            WordManager.Instance?.GainRewardMultiplier();
        }
    }
}