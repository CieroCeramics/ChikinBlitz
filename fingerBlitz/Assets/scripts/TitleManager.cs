using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    
    public LevelSelect levelselect;
    public AdventureBag adventurebag;
    // Start is called before the first frame update
   // public LevelSelect levelSelect;
    void Start()
    {
        GameControl.control.Load();
                          

        //GameControl.control.Load();
       // GameControl.control.updateinfo();

        // StartCoroutine(OpenTitle());
    }

    //IEnumerator OpenTitle()
    //{
    //    while (true)
    //    {

    //    }
    //   // StopCoroutine(OpenTitle());
    //    //return null;
    //}
    // Update is called once per frame
    void LoadPreviousSession()
    {
        //if(locationisopen)
       

    }
    void FixedUpdate()
    {
        if (GetComponent<Image>().fillAmount < 1.0f)
        {
            GetComponent<Image>().fillAmount += 0.01f;
        }
    }
    public void STARTClASSIC()
    {


        if (GameControl.control.fileExists)
        {
            if(LevelSelect.lSelect==null)
            {
                LevelSelect.lSelect = levelselect;
                AdventureBag.aBag = adventurebag;
            }
            LevelSelect.lSelect.gameObject.SetActive(true);//LevelSelect.lSelect.gameObject.SetActive(true);
            LevelSelect.lSelect.loadStagedata();

            print(GameControl.control.leveltoLoad.number);


        }

        else
        {
            GameControl.control.stagedata = new List<Stage>();
            GameControl.control.controlType = 1;
            GameControl.control.lives = 3;
            GameControl.control.Stage = 0;
                              SceneManager.LoadScene("TransitionScene");

        }

        
    }
    public void STARTADVENTURE()
    {
        SceneManager.LoadScene("AdventureBoard");
    }
    public void EXIT()
    {
        Application.Quit();
    }
}
