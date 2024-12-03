using UnityEngine;

namespace Infrastructure
{
    [CreateAssetMenu(fileName = nameof(ClickerConfig), menuName = "Configs/Clicker Config")]
    public class ClickerConfig : ScriptableObject
    {
        [Header("Currency")]
        [Tooltip("Currency click reward (integer)")]
        [SerializeField] private int _currencyReward = 1;
        
        [Space]
        [Header("Energy")]
        [Tooltip("Energy click cost (integer)")]
        [SerializeField] private int _energyCost = 1;

        [Tooltip("Energy regeneration amount (integer)")]
        [SerializeField] private int _energyReward = 10;
        
        [Tooltip("Energy maximum value (integer)")]
        [SerializeField] private int _maxEnergy = 1000;
        
        [Tooltip("Energy regeneration delay in milliseconds (integer)")]
        [SerializeField] private int _energyRegenDelay = 10000;
        
        [Tooltip("Auto collect delay in milliseconds (integer)")]
        [SerializeField] private int _autoCollectDelay = 3000;

        public int CurrencyReward => _currencyReward;
        public int EnergyCost => _energyCost;
        public int EnergyReward => _energyReward;
        public int MaxEnergy => _maxEnergy;
        public int EnergyRegenDelay => _energyRegenDelay;
        public int AutoCollectDelay => _autoCollectDelay;
    }
}