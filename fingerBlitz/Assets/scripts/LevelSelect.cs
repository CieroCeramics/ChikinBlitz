using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelSelect : MonoBehaviour
{
    public static LevelSelect lSelect;
   public int mystage = 0;
    public Button LevelButton;
    public GameObject[] StagePanel;
    // Start is called before the first frame update
    Queue<Button> inactivebuttons = new Queue<Button>();
    private void Awake()
    {
        if (lSelect == null)
        {
            DontDestroyOnLoad(gameObject);
            lSelect = this;
        }
        else if (lSelect!=this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {      
        //int i = 0;
        //if (GameControl.control.stagedata[mystage].levels != null)
        //{
        //    for(int j = 0;j<GameControl.control.stagedata[mystage].levelCap;j++)
        //    {
        //        Button levelButtonClone;
        //        levelButtonClone = Instantiate(LevelButton, StagePanel[mystage].gameObject.transform);
        //        levelButtonClone.interactable = false;
        //    }
        //    foreach (Level level in GameControl.control.stagedata[mystage].levels)
        //    {
                
        //        //levelButtonClone.onClick.AddListener(delegate { loadSelectedLevel(level); });
        //        i++;
        //    }
        //}
    }


    void scanButtons(int stagenum)
    {
        int i = 0;
        foreach(Button button in StagePanel[stagenum].GetComponentsInChildren<Button>())
        {
            if (GameControl.control.stagedata[stagenum].levels==null)
            {
                print("nodata");
            }
          else if( i < GameControl.control.stagedata[stagenum].levels.Count)
            {
                Level level = GameControl.control.stagedata[stagenum].levels[i];
                button.onClick.AddListener(delegate { loadSelectedLevel(level); });
                button.interactable = true;
               // inactivebuttons.Dequeue();
                i++;
            }
            else button.interactable = false;
        }
    }

    void loadStagedata()
    {
        int i = 0;

        //foreach (Stage stage in GameControl.control.stagedata)
        for (int f = 0; f <= GameControl.control.Stage; f++)
        {
            StagePanel[f].SetActive(true);
            scanButtons(f);
            //i++;

        }

    }









    







    public void AddStage(Stage stage)
    {
        for (int j = 0; j < stage.levelCap; j++)
        {
            Button levelButtonClone;
            levelButtonClone = Instantiate(LevelButton, StagePanel[mystage].gameObject.transform);
            levelButtonClone.interactable = false;
            inactivebuttons.Enqueue(levelButtonClone);
        }
    }


    public void AddButton(Level level)
    {
        Button levelButtonClone;
        levelButtonClone = Instantiate(LevelButton, StagePanel[mystage].gameObject.transform);
        levelButtonClone.onClick.AddListener(delegate { loadSelectedLevel(level); });

    }
    private void OnEnable()
    {
        loadStagedata();
    }
    void loadSelectedLevel(Level lNumber)
    {
        GameControl.control.leveltoLoad = lNumber;
        GameControl.control.LevelNumber = lNumber.number;
        GameControl.control.doGenerateNextLevel = false;
        gameObject.SetActive(false);
        SceneManager.LoadScene("Board");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
