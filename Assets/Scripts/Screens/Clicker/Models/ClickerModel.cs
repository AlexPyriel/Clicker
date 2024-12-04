using Infrastructure;
using UnityEngine;

namespace Screens.Clicker.Models
{
    public class ClickerModel
    {
        private readonly ClickerConfig _config;
        
        public int Currency { get; private set; }
        public int Energy { get; private set; }

        public ClickerModel(ClickerConfig config)
        {
            _config = config;
            Energy = _config.MaxEnergy;
        }

        public void AddCurrency(int amount)
        {
            Currency += Mathf.Max(0, amount);;
        }

        public bool TrySpendEnergy(int amount)
        {
            if (Energy < amount) return false;
            
            Energy -= amount;
            return true;
        }

        public void RegenEnergy(int amount)
        {
            Energy = Mathf.Min(Energy + amount, _config.MaxEnergy);
        }
    }
}