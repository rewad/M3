using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LocalizationString : MonoBehaviour {

    public string Category;
    public string Key;
	void Start ()
    {
        OnUpdate();
        LocalizationComponent.Get().AddString(this);
	}
    
    public void OnUpdate()
    {
        string str=LocalizationComponent.Get().GetKey(Category,Key);
        Text text = GetComponent<Text>();
        text.text = str;
    } 
}
