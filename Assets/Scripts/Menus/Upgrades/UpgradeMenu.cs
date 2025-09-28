using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Menus.Upgrades
{
    public class UpgradeMenu : MonoBehaviour
    {
        [SerializeField] private int totalMoney;
        [SerializeField] private TMP_Text totalMoneyText;
        [SerializeField] private StatUpgrade MoreThoughtsUpgrade;
        [SerializeField] private StatUpgrade LongerThoughtsUpgrade;
        [SerializeField] private StatUpgrade CriticalThinkingUpgrade;
        [SerializeField] private StatUpgrade EnergyRegenUpgrade;
        [SerializeField] private StatUpgrade WordsTypedUpgrade;
        [SerializeField] private StatUpgrade LessTimeLossUpgrade;
        [SerializeField] private Button GoToNextLevelButton;
        
        public void Start()
        {
            totalMoney = PlayerManager.Instance.totalMoney;
            
            MoreThoughtsUpgrade.upgradeLevel = PlayerManager.Instance.moreThoughtsLevel;
            LongerThoughtsUpgrade.upgradeLevel = PlayerManager.Instance.longerThoughtsLevel;
            CriticalThinkingUpgrade.upgradeLevel = PlayerManager.Instance.criticalThinkingLevel;
            EnergyRegenUpgrade.upgradeLevel = PlayerManager.Instance.energyRegenLevel;
            WordsTypedUpgrade.upgradeLevel = PlayerManager.Instance.wordsTypedLevel;
            LessTimeLossUpgrade.upgradeLevel = PlayerManager.Instance.lessTimeLossLevel;
            
            MoreThoughtsUpgrade.upgradeButton.onClick.AddListener(UpgradeMoreThoughts);
            LongerThoughtsUpgrade.upgradeButton.onClick.AddListener(UpgradeLongerThoughts);
            CriticalThinkingUpgrade.upgradeButton.onClick.AddListener(UpgradeCriticalThinking);
            EnergyRegenUpgrade.upgradeButton.onClick.AddListener(UpgradeEnergyRegen);
            WordsTypedUpgrade.upgradeButton.onClick.AddListener(UpgradeWordsTyped);
            LessTimeLossUpgrade.upgradeButton.onClick.AddListener(UpgradeLessTimeLoss);
            GoToNextLevelButton.onClick.AddListener(() =>
            {
                LoadingScreen.Instance.LoadNextRound(GameManager.Instance._round++);
            });
            
            MoreThoughtsUpgrade.UpdateStatInfo();
            LongerThoughtsUpgrade.UpdateStatInfo();
            CriticalThinkingUpgrade.UpdateStatInfo();
            EnergyRegenUpgrade.UpdateStatInfo();
            WordsTypedUpgrade.UpdateStatInfo();
            LessTimeLossUpgrade.UpdateStatInfo();
            
            SpendMoney(0);
        }

        public void Update()
        {
            MoreThoughtsUpgrade.upgradeButton.interactable = (MoreThoughtsUpgrade.upgradeCost <= totalMoney);
            LongerThoughtsUpgrade.upgradeButton.interactable = (LongerThoughtsUpgrade.upgradeCost <= totalMoney);
            CriticalThinkingUpgrade.upgradeButton.interactable = (CriticalThinkingUpgrade.upgradeCost <= totalMoney);
            EnergyRegenUpgrade.upgradeButton.interactable = (EnergyRegenUpgrade.upgradeCost <= totalMoney);
            WordsTypedUpgrade.upgradeButton.interactable = (WordsTypedUpgrade.upgradeCost <= totalMoney);
            LessTimeLossUpgrade.upgradeButton.interactable = (LessTimeLossUpgrade.upgradeCost <= totalMoney);
        }

        private void SpendMoney(int moneySpent)
        {
            totalMoney -= moneySpent;
            totalMoneyText.text = "$" + totalMoney;
            PlayerManager.Instance.totalMoney = totalMoney;
        }

        public void UpgradeMoreThoughts()
        {
            Debug.Log("More thoughts!");

            MoreThoughtsUpgrade.upgradeLevel++;
            PlayerManager.Instance.moreThoughtsLevel = MoreThoughtsUpgrade.upgradeLevel;
            SpendMoney(MoreThoughtsUpgrade.upgradeCost);
            MoreThoughtsUpgrade.UpdateStatInfo();
        }

        public void UpgradeLongerThoughts()
        {
            Debug.Log("Longer thoughts!");
            
            LongerThoughtsUpgrade.upgradeLevel++;
            PlayerManager.Instance.longerThoughtsLevel = LongerThoughtsUpgrade.upgradeLevel;
            SpendMoney(LongerThoughtsUpgrade.upgradeCost);
            LongerThoughtsUpgrade.UpdateStatInfo();
        }

        public void UpgradeCriticalThinking()
        {
            Debug.Log("Critical thinking!");
            
            CriticalThinkingUpgrade.upgradeLevel++;
            PlayerManager.Instance.criticalThinkingLevel = CriticalThinkingUpgrade.upgradeLevel;
            SpendMoney(CriticalThinkingUpgrade.upgradeCost);
            CriticalThinkingUpgrade.UpdateStatInfo();
        }

        public void UpgradeEnergyRegen()
        {
            Debug.Log("Energy regen!");
            
            CriticalThinkingUpgrade.upgradeLevel++;
            PlayerManager.Instance.energyRegenLevel = CriticalThinkingUpgrade.upgradeLevel;
            SpendMoney(EnergyRegenUpgrade.upgradeCost);
            EnergyRegenUpgrade.UpdateStatInfo();
        }

        public void UpgradeWordsTyped()
        {
            Debug.Log("Words typed!");
            
            WordsTypedUpgrade.upgradeLevel++;
            PlayerManager.Instance.wordsTypedLevel = WordsTypedUpgrade.upgradeLevel;
            SpendMoney(WordsTypedUpgrade.upgradeCost);
            WordsTypedUpgrade.UpdateStatInfo();
        }

        public void UpgradeLessTimeLoss()
        {
            Debug.Log("Less time loss!");
            
            LessTimeLossUpgrade.upgradeLevel++;
            PlayerManager.Instance.lessTimeLossLevel = LessTimeLossUpgrade.upgradeLevel;
            SpendMoney(LessTimeLossUpgrade.upgradeCost);
            LessTimeLossUpgrade.UpdateStatInfo();
        }
    }
}
