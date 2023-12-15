using System;
using System.Collections;
using System.Collections.Generic;
using Bigmode;
using Sirenix.OdinInspector;
using UnityEngine;

public class RoundManager : Singleton<RoundManager>
{
    public static event Action OnRoundEnd;
    public int Round = 1;

    [SerializeField] private float roundDurationInSeconds = (60f * 2f);
    [SerializeField, ReadOnly] private float roundTimer = 0f;
    [SerializeField] private GameObject upgradeUI;

    public delegate void OnRoundChanged(int round);
    public static event OnRoundChanged OnRoundChangedEvent;
    public delegate void OnRoundTimerChanged(float roundTimer);
    public static event OnRoundTimerChanged OnRoundTimerChangedEvent;

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
        OnRoundChangedEvent?.Invoke(Round);
        upgradeUI.SetActive(false);
        StartRound();
    }

    void Update()
    {
        if (isRoundActive)
        {
            roundTimer += Time.deltaTime;
            OnRoundTimerChangedEvent.Invoke(roundTimer);
            if (roundTimer >= roundDurationInSeconds)
            {
                EndRound();
            }
        }
    }

    public float GetRoundDuration()
    {
        return roundDurationInSeconds;
    }
}
