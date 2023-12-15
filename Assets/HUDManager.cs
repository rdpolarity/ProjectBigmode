using System.Collections;
using System.Collections.Generic;
using Bigmode;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDManager : MonoBehaviour
{
    private UIDocument HUD;
    private VisualElement root;

    void Awake() {
        HUD = GetComponent<UIDocument>();
        root = HUD.rootVisualElement;
    }

    void Start() {
        OnTimerChanged(0);
        OnRoundChanged(RoundManager.Instance.Round);
    }

    void OnEnable() {
        RoundManager.OnRoundChangedEvent += OnRoundChanged;
        RoundManager.OnRoundTimerChangedEvent += OnTimerChanged;
        Biggie.OnSelectedMinionChangedEvent += OnSelectedMinionChanged;
    }

    void OnDisable() {
        RoundManager.OnRoundChangedEvent -= OnRoundChanged;
        RoundManager.OnRoundTimerChangedEvent -= OnTimerChanged;
        Biggie.OnSelectedMinionChangedEvent -= OnSelectedMinionChanged;
    }

    void OnSelectedMinionChanged(MinionType minion)
    {
        root.Q<Label>("Title").text = minion.name;
        root.Q<Label>("Description").text = minion.description;
        root.Q<VisualElement>("Icon").style.backgroundImage = new StyleBackground(minion.sprite);
    }

    void OnTimerChanged(float roundTimer)
    {
        var roundTime = RoundManager.Instance.GetRoundDuration();
        var timeLeft = Mathf.FloorToInt(roundTime - roundTimer);
        if (timeLeft < 0) timeLeft = 0;
        root.Q<Label>("Time").text = $"{timeLeft}";
    }

    void OnRoundChanged(int round)
    {
        root.Q<Label>("Round").text = $"Round {round}";

        // upgradeCard.Q<Button>("Upgrade").clicked += () => OnClickUpgrade(upgrade);
        // upgradeCard.Q<Label>("Title").text = upgrade.Title;
        // upgradeCard.Q<Label>("Description").text = upgrade.Description;
        // upgradeCard.Q<VisualElement>("Icon").style.backgroundImage = new StyleBackground(upgrade.Icon);
    }
}
