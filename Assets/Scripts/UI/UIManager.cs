using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Bigmode;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour, IMassChangeListener, IMinionCountChangeListener
{
    [SerializeField]
    private GameObject GameOverScreen;

    [SerializeField]
    private TextMeshProUGUI MinionCountText;

    [SerializeField]
    private TextMeshProUGUI MassCountText;

    [SerializeField]
    private Biggie PlayerBiggie;

    [SerializeField]
    private string HUD_MINION_COUNT_APPEND_TEXT = "Biggie small count: ";

    [SerializeField]
    private string HUD_MASS_APPEND_TEXT = "Big-ness: ";

    private int minionCount = 0;

    void OnEnable() {
        Biggie.OnMassChangedEvent += MassChanged;
    }

    void OnDisable() {
        Time.timeScale = 1f;
        Biggie.OnMassChangedEvent -= MassChanged;
    }

    void Start()
    {
        // not pretty but I couldn't work out how to do this via the GUI c'::
        PlayerBiggie.minionCountChangeListeners.Add(this);
    }

    // this will only be set up to listen to the Player's mass change
    public void MassChanged(int playerMass){
        MassCountText.SetText(HUD_MASS_APPEND_TEXT + playerMass);
        if (playerMass == 0)
            MassCountText.SetText(HUD_MASS_APPEND_TEXT + "tiny 'lil guy");
        if (playerMass <= 5) {
            MassCountText.color = Color.red;
        }
        if (playerMass > 5) {
            MassCountText.color = Color.white;
        }
        if (playerMass <= 0){ // pause and display Game Over screen if the player is dead
            MassCountText.SetText(HUD_MASS_APPEND_TEXT + "Oh no");
            Time.timeScale = 0f;
            GameOverScreen.SetActive(true);
            // not going to pause for now as I think it's kind of fun to not.
            // if we would like to though, here is the code:
            // Time.timeScale = 0f;
        }
    }

    public void MinionCountChanged(int delta){
        minionCount += delta;
        MinionCountText.SetText(HUD_MINION_COUNT_APPEND_TEXT + minionCount);
    }
}

public interface IMassChangeListener
{
    public void MassChanged(int currentMass);
}

public interface IMinionCountChangeListener
{
    public void MinionCountChanged(int delta);
}

