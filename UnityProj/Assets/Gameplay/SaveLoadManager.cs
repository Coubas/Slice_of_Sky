using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class GameData
{
    public bool     invertYAxis;
    public bool     alwaysShowControls;
    public int      highScore;
    public int[]    gaugesStocks;
    public int[]    gaugesLvl;

    public GameData()
    {
        invertYAxis = false;
        alwaysShowControls = true;
        highScore = 0;
        gaugesStocks = new int[5];
        gaugesLvl = new int[5];
    }

    public override string ToString()
    {
        string output = "GameData \n { \n";
        output += "\t Invert Y = " + invertYAxis.ToString() + "\n";
        output += "\t Always show controls = " + alwaysShowControls.ToString() + "\n";
        output += "\t High score = " + highScore.ToString() + "\n";
        output += "\t Spirits collected = " + gaugesStocks[0] + gaugesStocks[1] + gaugesStocks[2] + gaugesStocks[3] + gaugesStocks[4] + "\n";
        output += "\t Lantern lvl = " + gaugesLvl[0] + gaugesLvl[1] + gaugesLvl[2] + gaugesLvl[3] + gaugesLvl[4] + "\n";
        output += "} \n";

        return output;
    }
}

public static class SaveLoadManager
{

    public static GameData savedData = new GameData();

    public static void SetSavedData(bool _invertYAxis, bool _alwaysShowControls, int _highScore, int[] _gaugesStocks, int[] _gaugesLvl)
    {
        savedData.invertYAxis = _invertYAxis;
        savedData.alwaysShowControls = _alwaysShowControls;
        savedData.highScore = _highScore;
        savedData.gaugesStocks = _gaugesStocks;
        savedData.gaugesLvl = _gaugesLvl;
    }

    public static void GetSavedData(ref bool _invertYAxis, ref bool _alwaysShowControls, ref int _highScore, ref int[] _gaugesStocks, ref int[] _gaugesLvl)
    {
        _invertYAxis = savedData.invertYAxis;
        _alwaysShowControls = savedData.alwaysShowControls;
        _highScore = savedData.highScore;
        _gaugesStocks = savedData.gaugesStocks;
        _gaugesLvl = savedData.gaugesLvl;
    }

    //it's static so we can call it from anywhere
    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        Debug.Log("Data : " + savedData.ToString());
        Debug.Log("Data saved here : " + Application.persistentDataPath);
        FileStream file = File.Create(Application.persistentDataPath + "/savedData.gd"); //you can call it anything you want
        bf.Serialize(file, savedData);
        file.Close();
    }

    public static bool Load()
    {
        Debug.Log("Trying to load from " + Application.persistentDataPath);
        if (File.Exists(Application.persistentDataPath + "/savedData.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedData.gd", FileMode.Open);
            savedData = (GameData)bf.Deserialize(file);
            file.Close();

            Debug.Log("Data Loaded from : " + Application.persistentDataPath);
            Debug.Log("Data : " + savedData.ToString());
            return true;
        }

        return false;
    }
}