using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GameUI : IMenuUI
{ 
    public Text m_textScore; 
    public Text m_textTime;
 
    public override void CloseMenu()
    {
        CGameObject.SetActive(false);
    }

    public override void OpenMenu()
    {
        CGameObject.SetActive(true);
    }

    public void UpdateScoreUI(int score, float time)
    {
        m_textScore.text = score.ToString();
        m_textTime.text = Convert.ToInt32(time).ToString();
    }

    public void OnPause()
    {
        GameInstance.Get().SetState(EGameState.EPauseState);        
    }
}
