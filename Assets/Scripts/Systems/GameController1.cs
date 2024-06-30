using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController1 : MonoBehaviour
{
    [SerializeField] PlayerController1 player1;

    public static GameController1 Instance { get; private set; }

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
        }
        else if (state == GameState.Dialog)
        {
            DialogueManager.Instance.HandleUpdate();
        }
    }
}
