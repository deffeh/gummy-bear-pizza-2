using UnityEngine;

namespace Phone
{
    public class ResetMultiplierReel : Reel
    {
        
        public override void OnActivate()
        {
            ActivateText = "Multiplier Reset";
            index = 0;
            base.OnActivate();
            WordManager.Instance?.ResetRewardMultiplier();
        }
    }
}