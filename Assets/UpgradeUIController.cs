using UnityEngine;
using UnityEngine.UIElements;

public class UpgradeUIController : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset upgradeCardTemplate;

    void OnEnable() {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var upgradeContainer = root.Q<VisualElement>("Upgrades");

        var upgrades = UpgradeManager.Instance.GenerateRandomUpgrades(3);
        foreach (var upgrade in upgrades) {
            var upgradeCard = MakeUpgradeCard(upgrade);
            upgradeContainer.Add(upgradeCard);
        }
    }

    TemplateContainer MakeUpgradeCard(Upgrade upgrade) {
        var upgradeCard = upgradeCardTemplate.CloneTree();
        upgradeCard.Q<Button>("Upgrade").clicked += () => OnClickUpgrade(upgrade);
        upgradeCard.Q<Label>("Title").text = upgrade.Title;
        upgradeCard.Q<Label>("Description").text = upgrade.Description;
        upgradeCard.Q<VisualElement>("Icon").style.backgroundImage = new StyleBackground(upgrade.Icon);
        return upgradeCard;
    }

    void OnClickUpgrade(Upgrade upgrade) {
        RoundManager.Instance.OnUpgradeSelected();
        UpgradeManager.Instance.PickUpgrade(upgrade);
    }
}
