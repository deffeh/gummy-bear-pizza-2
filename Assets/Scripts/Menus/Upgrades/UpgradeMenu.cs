using System;
using Phone;
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
        [SerializeField] private StatUpgrade moreThoughtsUpgrade;
        [SerializeField] private StatUpgrade longerThoughtsUpgrade;
        [SerializeField] private StatUpgrade criticalThinkingUpgrade;
        [SerializeField] private StatUpgrade energyRegenUpgrade;
        [SerializeField] private StatUpgrade wordsTypedUpgrade;
        [SerializeField] private StatUpgrade lessTimeLossUpgrade;
        [SerializeField] private Button goToNextLevelButton;

        [SerializeField] private float moreThoughtsUpgradeAmount = 0.2f;
        [SerializeField] private float longerThoughtsUpgradeAmount = 0.5f;
        [SerializeField] private float criticalThinkingUpgradeAmount = 0.05f;
        [SerializeField] private float energyRegenUpgradeAmount = 2.5f;
        [SerializeField] private int wordsTypedUpgradeAmount = 5;
        [SerializeField] private int lessTimeLossUpgradeAmount = 1;
        
        public void Start()
        {
            totalMoney = PlayerManager.Instance.totalMoney;
            
            moreThoughtsUpgrade.upgradeLevel = PlayerManager.Instance.moreThoughtsLevel;
            longerThoughtsUpgrade.upgradeLevel = PlayerManager.Instance.longerThoughtsLevel;
            criticalThinkingUpgrade.upgradeLevel = PlayerManager.Instance.criticalThinkingLevel;
            energyRegenUpgrade.upgradeLevel = PlayerManager.Instance.energyRegenLevel;
            wordsTypedUpgrade.upgradeLevel = PlayerManager.Instance.wordsTypedLevel;
            lessTimeLossUpgrade.upgradeLevel = PlayerManager.Instance.lessTimeLossLevel;
            
            moreThoughtsUpgrade.upgradeButton.onClick.AddListener(UpgradeMoreThoughts);
            longerThoughtsUpgrade.upgradeButton.onClick.AddListener(UpgradeLongerThoughts);
            criticalThinkingUpgrade.upgradeButton.onClick.AddListener(UpgradeCriticalThinking);
            energyRegenUpgrade.upgradeButton.onClick.AddListener(UpgradeEnergyRegen);
            wordsTypedUpgrade.upgradeButton.onClick.AddListener(UpgradeWordsTyped);
            lessTimeLossUpgrade.upgradeButton.onClick.AddListener(UpgradeLessTimeLoss);
            goToNextLevelButton.onClick.AddListener(() =>
            {
                LoadingScreen.Instance.LoadNextRound(++PlayerManager.Instance.round);
            });
            
            moreThoughtsUpgrade.UpdateStatInfo();
            longerThoughtsUpgrade.UpdateStatInfo();
            criticalThinkingUpgrade.UpdateStatInfo();
            energyRegenUpgrade.UpdateStatInfo();
            wordsTypedUpgrade.UpdateStatInfo();
            lessTimeLossUpgrade.UpdateStatInfo();
            
            SpendMoney(0);
        }

        private void UpdateButtonState()
        {
            moreThoughtsUpgrade.upgradeButton.interactable = (moreThoughtsUpgrade.upgradeCost <= totalMoney);
            longerThoughtsUpgrade.upgradeButton.interactable = (longerThoughtsUpgrade.upgradeCost <= totalMoney);
            criticalThinkingUpgrade.upgradeButton.interactable = (criticalThinkingUpgrade.upgradeCost <= totalMoney);
            energyRegenUpgrade.upgradeButton.interactable = (energyRegenUpgrade.upgradeCost <= totalMoney);
            wordsTypedUpgrade.upgradeButton.interactable = (wordsTypedUpgrade.upgradeCost <= totalMoney);
            lessTimeLossUpgrade.upgradeButton.interactable = (lessTimeLossUpgrade.upgradeCost <= totalMoney);
        }

        private void SpendMoney(int moneySpent)
        {
            totalMoney -= moneySpent;
            totalMoneyText.text = "$" + totalMoney;
            PlayerManager.Instance.totalMoney = totalMoney;
            UpdateButtonState();
        }

        public void UpgradeMoreThoughts()
        {
            Debug.Log("More thoughts!");

            WordManager.Instance._bubbleRate -= moreThoughtsUpgradeAmount;

            moreThoughtsUpgrade.upgradeLevel++;
            PlayerManager.Instance.moreThoughtsLevel = moreThoughtsUpgrade.upgradeLevel;
            SpendMoney(moreThoughtsUpgrade.upgradeCost);
            moreThoughtsUpgrade.UpdateStatInfo();
        }

        public void UpgradeLongerThoughts()
        {
            Debug.Log("Longer thoughts!");
            
            WordManager.Instance._bubbleDuration += longerThoughtsUpgradeAmount;
            
            longerThoughtsUpgrade.upgradeLevel++;
            PlayerManager.Instance.longerThoughtsLevel = longerThoughtsUpgrade.upgradeLevel;
            SpendMoney(longerThoughtsUpgrade.upgradeCost);
            longerThoughtsUpgrade.UpdateStatInfo();
        }

        public void UpgradeCriticalThinking()
        {
            Debug.Log("Critical thinking!");
            
            WordManager.Instance._critChance += criticalThinkingUpgradeAmount;
            
            criticalThinkingUpgrade.upgradeLevel++;
            PlayerManager.Instance.criticalThinkingLevel = criticalThinkingUpgrade.upgradeLevel;
            SpendMoney(criticalThinkingUpgrade.upgradeCost);
            criticalThinkingUpgrade.UpdateStatInfo();
        }

        public void UpgradeEnergyRegen()
        {
            Debug.Log("Energy regen!");

            GainMultiplierReel.energyRegenAmount += energyRegenUpgradeAmount;
            DoNothingReel.energyRegenAmount += energyRegenUpgradeAmount;
            GainMultiplierReel.energyRegenAmount += energyRegenUpgradeAmount;
            
            energyRegenUpgrade.upgradeLevel++;
            PlayerManager.Instance.energyRegenLevel = energyRegenUpgrade.upgradeLevel;
            SpendMoney(energyRegenUpgrade.upgradeCost);
            energyRegenUpgrade.UpdateStatInfo();
        }

        public void UpgradeWordsTyped()
        {
            Debug.Log("Words typed!");
            
            WordManager.Instance._rewardWordsPerBubble += wordsTypedUpgradeAmount;
            
            wordsTypedUpgrade.upgradeLevel++;
            PlayerManager.Instance.wordsTypedLevel = wordsTypedUpgrade.upgradeLevel;
            SpendMoney(wordsTypedUpgrade.upgradeCost);
            wordsTypedUpgrade.UpdateStatInfo();
        }

        public void UpgradeLessTimeLoss()
        {
            Debug.Log("Less time loss!");
            
            Reel.timeLostPerReel -= lessTimeLossUpgradeAmount;
            
            lessTimeLossUpgrade.upgradeLevel++;
            PlayerManager.Instance.lessTimeLossLevel = lessTimeLossUpgrade.upgradeLevel;
            SpendMoney(lessTimeLossUpgrade.upgradeCost);
            lessTimeLossUpgrade.UpdateStatInfo();
        }
    }
}
