using UnityEngine;
using System.Collections;
using System;

public class PauseUI : IMenuUI
{
    public override void CloseMenu()
    {
        CGameObject.SetActive(false);
        GameInstance.Get().SetState(EGameState.EPlayState);
    }

    public override void OpenMenu()
    {
        CGameObject.SetActive(true);
        GameInstance.Get().SetState(EGameState.EPauseState);
    }
   
    public void OnRetry()
    {
        GameInstance.Get().StartNewGame();
    }
    public void OnExit()
    {
        GameInstance.Get().SetState(EGameState.EExitState);
    }

}
