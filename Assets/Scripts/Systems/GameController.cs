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
    [SerializeField] Player player;

    GameState state;


    private void Start()
    {
        DialogueManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
            if (player != null) { player.interactText.gameObject.SetActive(false); }
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

            if (player != null)
            {
                player.HandleUpdate();
            }
        }
        else if (state == GameState.Dialog)
        {
            DialogueManager.Instance.HandleUpdate();
        }
    }
}
