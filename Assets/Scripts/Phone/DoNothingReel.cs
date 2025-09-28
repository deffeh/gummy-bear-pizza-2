using UnityEngine;

namespace Phone
{
    public class DoNothingReel : Reel
    {
        [SerializeField] public static float energyRegenAmount = 5.0f;
        public override void OnActivate()
        {
            PlayerManager.Instance?.UpdateEnergy(energyRegenAmount);
        }
    }
}
