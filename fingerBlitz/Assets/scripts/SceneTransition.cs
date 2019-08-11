using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransition : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        int stagenum = GameControl.control.Stage;
        GameControl.control.numUnlockedStages++;
        //if(GameControl.control.stagedata == null)
        if (GameControl.control.stagedata == null)
        {
            GameControl.control.stagedata = new List<Stage>();
            
            GameControl.control.lives = 3;
        }
        //  GameControl.control.stagedata = new List<Stage>();

        GameControl.control.stagedata.Add(new Stage());
        
       // GameControl.control.stagedata[GameControl.control.Stage].levelCap = getLevlcap(GameControl.control.Stage);
        //switch (stagenum)
        //{
        //    case 0:
        //        GameControl.control.stagedata[stagenum].levelCap = 10;
        //        break;
        //    case 1:
        //        GameControl.control.stagedata[stagenum].levelCap = 16;
        //        break;
        //    case 2:
        //        GameControl.control.stagedata[stagenum].levelCap = 32;
        //        break;
        //}

        GameControl.control.stagedata[GameControl.control.Stage].levels = new List<Level>();
        GameControl.control.stagedata[GameControl.control.Stage].levels.Clear();
        // GameControl.control.Stage++;
         
       // loadNextScene();
    }
    private void Start()
    {
        
        // LevelSelect.lSelect.AddStage(GameControl.control.stagedata[GameControl.control.Stage]);

        Invoke("loadNextScene", 7);
    }
    public int getLevlcap(int n)
    {
        if (n == 0) { return 10; }
        if (n == 1) { return 16; }
        if (n == 2) { return 32; }
        else return 0;
    }
    // Update is called once per frame
    void loadNextScene()
    {
        LevelSelect.lSelect.AddStage(GameControl.control.Stage );
        GameControl.control.doGenerateNextLevel = true;
        SceneManager.LoadScene("Board");
    }

    void decideSceneCap()
    {

    }
}
