using UnityEngine;

namespace Phone
{
    public class ResetMultiplierReel : Reel
    {
        public override void OnActivate()
        {
            WordManager.Instance?.ResetRewardMultiplier();
        }
    }
}