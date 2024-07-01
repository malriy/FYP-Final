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
        DialogueManager1.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
            if (player1 != null) { PlayerController1.Instance.interactText.gameObject.SetActive(false); }
            Time.timeScale = 0f;
        };
        DialogueManager1.Instance.OnHideDialog += () =>
        {
            if (state == GameState.Dialog)
            {
                state = GameState.FreeRoam;
                Time.timeScale = 1f;
            }
        };
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            if (player1 != null)
            {
                player1.HandleUpdate();
            }
        }
        else if (state == GameState.Dialog)
        {
            DialogueManager1.Instance.HandleUpdate();
        }
    }
}
