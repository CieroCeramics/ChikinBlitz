using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
public class GameControl : MonoBehaviour
{
    
    public static GameControl control;
    public Text info;
   // public bool doGenerateNextLevel = true;
    public Level leveltoLoad;
   // public int seedToLoad;
    public List<GameObject> mazeContainer;
    public bool tutorialMode;

    public int numUnlockedStages;
    public int LevelNumber;
    public int phasenum;

    public float circleSize =10;
    public int controlType =1;
    public int Stage;
    public int lives;
    public int flys, zooms, times,bawks;
        public bool key;
    //public float difficulty;
    public List<Stage> stagedata;// = new List<Stage>();
    public bool fileExists =false;
   
    void Awake()
    {

        //doGenerateNextLevel = true;
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control!= this)
        {
            Destroy(gameObject);
        }
        

    }

    public void StoreFinishedLevel()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/levelInfo.dat");

       // Stage StageData
    }
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData
        {
            //standard oop design?
            lives = lives,
            times = times,
            zooms = zooms,
            flys = flys,
            bawks = bawks,
            stageData = stagedata,
            unlockedStages = numUnlockedStages
        };
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath+"/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            bawks = data.bawks;
            lives = data.lives;
            flys = data.flys;
            zooms = data.zooms;
            times = data.times;
            stagedata = data.stageData;
            numUnlockedStages = data.unlockedStages;
            fileExists = true;
        }
    }   
//     public void updateinfo()
//     {
//         info.text = ("Stage : " + Stage + " / " + numUnlockedStages + "         inactive button queue " + LevelSelect.lSelect.inactivebuttons.Count+
//                 "\n Level  : " + LevelNumber + " / " + stagedata[Stage].highestReachedLevel +
//                 "\n Phase : " + phasenum + " / " + stagedata[Stage].phases.Count);
//     }
}

[Serializable]
class PlayerData
{
    //gets and sets?
    public int lives, flys,zooms,times,bawks;
    public int unlockedStages;
    public List<Stage> stageData;// = new List<Stage>();

    //public PlayerData(int lives, int flys, int zooms, int times, List<Stage>stageData)
    //{
    //    _lives = lives;
    //    _flys = flys;
    //    _zooms = zooms;
    //    _times = times;
    //    _stageData = stageData;
    //}
    //public int Lives
    //{
    //    get => _lives;
    //    set=
    //}
    //    //public List<GameObject> upgrades;
}

[Serializable]
public class Phase
{
    public int score;
    public bool completed;
    public int number;
    public int keyLevel;
    public List<Level> levels;
}

[Serializable]
public class Stage
{
    public int score;
    public List<Phase> phases;
    public int highestReachedLevel;
    public int stars;
}
[Serializable]
public class Level
{
public int score;
    public int number;
    public int randomSeed;
    public int stage;
    public int phase;
    public bool complete;
    //public MazeContainer Maze;
    //public SerializableVector2 start, finish,key;
    //public List<SerializableVector2> lguys;
    //public List<SerializableVector2> pguys;
   // public Vector2 upgrade;
    //public List<GameObject> things;
    //public GameObject start, finish;
}
//[Serializable]
//public class MazeContainer
//{
//  public  List <Wall> walls;
//}
//[Serializable]
//public class Wall
//{
//   // public Wall(int _type, Vector2 _position, Vector2 _rotation)
//    public int type;
//    public SerializableVector3 rotation;
//    public SerializableVector2 position;
//    public SerializableVector2 scale;
//    //public seriali
    
//}