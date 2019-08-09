using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        //GameControl.control.Load();

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
        GameControl.control.lives = 3;
        if (GameControl.control.stagedata == null)
        {
            GameControl.control.Stage = 0;
            
           
        }
        SceneManager.LoadScene("TransitionScene");

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
