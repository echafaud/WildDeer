using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager
{
    public static string LastCheckPointName { get; private set; } = null;
    public static void SaveGame()
    {
        if (LastCheckPointName != null)
        {
            PlayerPrefs.SetString("LastCheckPoint", LastCheckPointName);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetString("LastCheckPoint", "Map1CheckPoint0");
        }
        PlayerPrefs.Save();
    }

    public static void LoadGame()
    {
        if (PlayerPrefs.HasKey("LastCheckPoint"))
        {
            LastCheckPointName = PlayerPrefs.GetString("LastCheckPoint");
        }
        else
        {
            LastCheckPointName = "Map1CheckPoint0";
        }
    }

    public static void DeleteSaves()
    {
        PlayerPrefs.DeleteAll();
    }

    public static bool isAnySaves()
    {
        return PlayerPrefs.HasKey("LastCheckPoint");
    }

    public static void SetLastCheckPointName(string name)
    {
        if (name != null && !name.Equals(""))
        {
            LastCheckPointName = name;
        }
    }
}
