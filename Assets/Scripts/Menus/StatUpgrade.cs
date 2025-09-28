using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    public class StatUpgrade : MonoBehaviour
    {
        [SerializeField] private string upgradeName = "Default name";
        [SerializeField] private int upgradeCost = 50;
        [SerializeField] private int upgradeLevel = 1;

        [SerializeField] private TMP_Text nameAndLevelText;
        [SerializeField] private TMP_Text costText;
        [SerializeField] private Button upgradeButton;

        public void Start()
        {
            // Load and update player stat level
            
            upgradeButton.onClick.AddListener(() =>
            {
                
            });
            
            UpdateStatInfo();
        }

        private void OnUpgrade()
        {
            
        }
        
        public void UpdateStatInfo()
        {
            if (nameAndLevelText) nameAndLevelText.text = upgradeName + " [Lvl " + upgradeLevel + "]";
            if (costText) costText.text = "$" + upgradeCost;
        }
        
        
    }
}
