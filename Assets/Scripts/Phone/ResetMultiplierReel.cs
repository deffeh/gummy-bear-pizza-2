using UnityEngine;

namespace Phone
{
    public class ResetMultiplierReel : Reel
    {
        
        public override void OnActivate()
        {
            ActivateText = "Multiplier Reset";
            base.OnActivate();
            WordManager.Instance?.ResetRewardMultiplier();
        }
    }
}