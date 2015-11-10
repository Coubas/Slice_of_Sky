using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class GameData
{
    public bool     invertYAxis;
    public int      highScore;
    public int[]    gaugesStocks;

    public GameData()
    {
        invertYAxis = false;
        highScore = 0;
        gaugesStocks = new int[5];
    }
}

public static class SaveLoadManager
{

    public static GameData savedData = new GameData();

    public static void SetSavedData(bool _invertYAxis, int _highScore, int[] _gaugesStocks)
    {
        savedData.invertYAxis = _invertYAxis;
        savedData.highScore = _highScore;
        savedData.gaugesStocks = _gaugesStocks;
    }

    public static void GetSavedData(ref bool _invertYAxis, ref int _highScore, ref int[] _gaugesStocks)
    {
        _invertYAxis = savedData.invertYAxis;
        _highScore = savedData.highScore;
        _gaugesStocks = savedData.gaugesStocks;
    }

    //it's static so we can call it from anywhere
    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        Debug.Log("Data saved here : " + Application.persistentDataPath);
        FileStream file = File.Create(Application.persistentDataPath + "/savedData.gd"); //you can call it anything you want
        bf.Serialize(file, savedData);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedData.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedData.gd", FileMode.Open);
            savedData = (GameData)bf.Deserialize(file);
            file.Close();

            Debug.Log("Data Loaded");
        }
    }
}