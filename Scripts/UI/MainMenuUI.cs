using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuUI : CachedObject
{
    public void ChangeLocalization(string lang)
    {
        LocalizationComponent.Get().UpdateAllLocalizationString(lang);
    }

    
    public void OnStartGameClick(int num)
    {
        GameInstance.Get().StartNewGame(num);
        GameInstance.Get().SetState(EGameState.EPlayState);
    }
}
