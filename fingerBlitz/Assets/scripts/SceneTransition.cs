using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransition : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1;
        GameControl.control.phasenum = 0;
        GameControl.control.LevelNumber= 0;
        // foreach( LevelSelect.lSelect.)
        int stagenum = GameControl.control.Stage;
        if (stagenum > 0)
        {
            if(GameControl.control.lives ==0)
            {
                GameControl.control.lives = 3;
            }
            GameControl.control.numUnlockedStages++;
        }
            //if(GameControl.control.stagedata == null)
        if (GameControl.control.stagedata == null)
        {
            
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
        GameControl.control.stagedata[GameControl.control.Stage].phases = new List<Phase>();
        GameControl.control.stagedata[GameControl.control.Stage].highestReachedLevel = 0;
        
       
        
        LevelSelect.lSelect.addPhase(0,GameControl.control.Stage,true);
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
      //  LevelSelect.lSelect.addPhase(GameControl.control.phasenum, GameControl.control.Stage);
        //GameControl.control.phasenum++;
        //GameControl.control.doGenerateNextLevel = true;
        GameControl.control.leveltoLoad = GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].levels[0];
        LevelSelect.lSelect.AddButton(GameControl.control.leveltoLoad);
        SceneManager.LoadScene("Board");
    }

    
    void decideSceneCap()
    {

    }
}
