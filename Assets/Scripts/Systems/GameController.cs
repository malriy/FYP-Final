using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    FreeRoam,
    Dialog
}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController1 player1;
    [SerializeField] Player player2;

    public static GameController Instance { get; private set; }

    GameState state;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DialogueManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
            if (player2 != null) { Player.Instance.interactText.gameObject.SetActive(false); }
            if (player1 != null) { PlayerController1.Instance.interactText.gameObject.SetActive(false); }
        };
        DialogueManager.Instance.OnHideDialog += () =>
        {
            if (state == GameState.Dialog)
            {
                state = GameState.FreeRoam;
            }
        };
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            if (player1 != null)
            {
                player1.Update();
            }

            if (player2 != null)
            {
                player2.HandleUpdate();
            }
        }
        else if (state == GameState.Dialog)
        {
            DialogueManager.Instance.HandleUpdate();
        }
    }
}
