using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menus.Upgrades
{
    public  class StatUpgrade : MonoBehaviour
    {
        [SerializeField] public string upgradeName = "Default name";
        [SerializeField] public int upgradeCost = 50;
        [SerializeField] public int upgradeLevel = 1;

        [SerializeField] public TMP_Text nameAndLevelText;
        [SerializeField] public TMP_Text costText;
        [SerializeField] public Button upgradeButton;
        
        public void UpdateStatInfo()
        {
            if (nameAndLevelText) nameAndLevelText.text = upgradeName + " [Lvl " + upgradeLevel + "]";
            if (costText) costText.text = "$" + upgradeCost;
        }
    }
}
