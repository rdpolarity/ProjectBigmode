using System.Collections;
using System.Collections.Generic;
using Bigmode;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [SerializeField] private List<Upgrade> upgrades;
    [SerializeField] private List<Upgrade> currentUpgrades;

    // Will choose 'amount' random upgrades from the list of upgrades
    // Each upgrade generated HAS to be unique
    // Returns a list of upgrades
    public List<Upgrade> GenerateRandomUpgrades(int amount)
    {
        List<Upgrade> shuffledUpgrades = new(upgrades);
        System.Random rng = new();
        int n = shuffledUpgrades.Count;

        // Shuffle the list
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (shuffledUpgrades[n], shuffledUpgrades[k]) = (shuffledUpgrades[k], shuffledUpgrades[n]);
        }

        // Take the first 'amount' upgrades from the shuffled list
        return shuffledUpgrades.GetRange(0, Mathf.Min(amount, shuffledUpgrades.Count));
    }

    public void PickUpgrade(Upgrade upgrade)
    {
        currentUpgrades.Add(upgrade);
    }
}
