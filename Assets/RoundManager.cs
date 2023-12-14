using System;
using System.Collections;
using System.Collections.Generic;
using Bigmode;
using Sirenix.OdinInspector;
using UnityEngine;

public class RoundManager : Singleton<RoundManager>
{
    public static event Action OnRoundEnd;
    public float Round = 1;

    [SerializeField] private float roundDurationInSeconds = (60f * 2f);
    [SerializeField, ReadOnly] private float roundTimer = 0f;
    [SerializeField] private GameObject upgradeUI;

    private bool isRoundActive = false;

    void Start()
    {
        StartRound();
    }

    [Button]
    void StartRound()
    {
        isRoundActive = true;
        roundTimer = 0f;
        Time.timeScale = 1;
    }

    [Button]
    void EndRound()
    {
        isRoundActive = false;
        OnRoundEnd?.Invoke(); // Trigger the event
        Time.timeScale = 0;
        upgradeUI.SetActive(true);
    }

    [Button]
    public void OnUpgradeSelected()
    {
        Round++;
        upgradeUI.SetActive(false);
        StartRound();
    }

    void Update()
    {
        if (isRoundActive)
        {
            roundTimer += Time.deltaTime;
            if (roundTimer >= roundDurationInSeconds)
            {
                EndRound();
            }
        }
    }
}
