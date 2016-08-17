using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LocalizationComponent
{
    private static LocalizationComponent m_instance;

    private Dictionary<string, string> m_dictWords;
    private List<LocalizationString> m_strings;
    private string m_lang;

    public static LocalizationComponent Get()
    {
        if (m_instance == null)
        {
            m_instance = new LocalizationComponent();
            m_instance.UpdateLocalizationzDict();
        }
        return m_instance;
    }
    private void LoadLanguage()
    {
        m_lang = PlayerPrefs.GetString("Lang");
        if (m_lang == "")
        {
            m_lang = "en";
            PlayerPrefs.SetString("Lang", m_lang);
        }
    }

    public void UpdateLocalizationzDict()
    {
        Get();
        LoadLanguage();
        m_dictWords = new Dictionary<string, string>();
        if (m_strings == null)
            m_strings = new List<LocalizationString>();
        string file_name = "Localization/" + m_lang + "";
        TextAsset ta = Resources.Load<TextAsset>(file_name);
        string[] files = ta.ToString().Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string file in files)
        {
            TextAsset text = Resources.Load<TextAsset>("Localization/" + file.Split('.')[0]);
            string[] pairs = text.ToString().Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            string category = pairs[0];
            for (int i = 1; i < pairs.Length; i++)
            {
                string pair = pairs[i];
                string value;
                string key = ParseString(pairs[i], out value);
                m_dictWords.Add(category + "|" + key, value);
            }

        }
    }
    public void AddString(LocalizationString str)
    {
        m_strings.Add(str);
    }
    private string ParseString(string str, out string key)
    {
        int index = str.IndexOf('|');
        key = str.Substring(index + 1);
        return str.Substring(0, index);
    }
    public string GetKey(string category, string key)
    {
        if (m_dictWords == null)
            UpdateLocalizationzDict();

        string category_key = category + "|" + key;
        string str;
        if (m_dictWords.TryGetValue(category_key, out str))
        {
            return str;
        }
        return "none";

    }
    public void UpdateAllLocalizationString(string lang)
    {
        m_lang = lang;
        PlayerPrefs.SetString("Lang", lang);
        UpdateLocalizationzDict();
        foreach (var str in m_strings)
        {
            bool active = str.gameObject.activeSelf;
            if (!str.gameObject.activeSelf)
                str.gameObject.SetActive(true);
            str.OnUpdate();

            if (!active)
                str.gameObject.SetActive(active);

        }
    }
}
