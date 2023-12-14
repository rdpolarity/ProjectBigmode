using UnityEngine;
using UnityEngine.UIElements;

public class UpgradeUIController : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset upgradeCardTemplate;

    void OnEnable() {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var upgradeContainer = root.Q<VisualElement>("Upgrades");

        // create an upgrade card and add it to upgradeContainer
        var upgradeCard = upgradeCardTemplate.CloneTree();
        upgradeCard.Q<Button>("Upgrade").clicked += OnClickUpgrade;
        upgradeCard.Q<Label>("Title").text = "Hello!";
        upgradeContainer.Add(upgradeCard);
    }

    void OnClickUpgrade() {
        RoundManager.Instance.OnUpgradeSelected();
    }
}
