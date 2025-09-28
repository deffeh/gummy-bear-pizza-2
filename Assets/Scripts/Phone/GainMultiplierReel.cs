using UnityEngine;

namespace Phone
{
    public class GainMultiplierReel : Reel
    {
        [SerializeField] public static float energyRegenAmount = 5.0f;
        public override void OnActivate()
        {
            ActivateText = "+Multiplier";
            base.OnActivate();
            PlayerManager.Instance?.UpdateEnergy(energyRegenAmount);
            WordManager.Instance?.GainRewardMultiplier();
        }
    }
}