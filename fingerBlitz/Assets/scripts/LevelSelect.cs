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
    public MultiDimensionalGO[] phase;
    // public GameObject[][]phase;
    public GameObject PhaseHolder;
    // Start is called before the first frame update
    public Queue<Button> inactivebuttons = new Queue<Button>();
    private void Awake()
    {

        mystage = GameControl.control.Stage;
        if (lSelect == null)
        {
            DontDestroyOnLoad(gameObject);
            lSelect = this;
        }
        else if (lSelect != this)
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


    void scanButtons(int stagenum, int phasenum)
    {
        int i = 0;
        foreach (Button button in phase[stagenum].intArray[phasenum].GetComponentsInChildren<Button>())
        {
            if (GameControl.control.stagedata[stagenum].phases[phasenum].levels == null)
            {
                print("nodata");
            }
            else if (i < GameControl.control.stagedata[stagenum].phases[phasenum].levels.Count)
            {
                Level level = GameControl.control.stagedata[stagenum].phases[phasenum].levels[i];
                button.onClick.AddListener(delegate { loadSelectedLevel(level); });
                button.GetComponentInChildren<Text>().text = level.number.ToString();
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
        for (int f = 0; f < GameControl.control.numUnlockedStages; f++)
        {
            StagePanel[f].SetActive(true);
            //scanButtons(f);
            //i++;

        }

    }

    [System.Serializable]
    public class MultiDimensionalGO
    {
        public GameObject[] intArray;
    }
    void changebuttoncolor(Button thebutton)
    {
        thebutton.GetComponent<Image>().color = Color.white;
    }
    ////// phase[stage][phase]
    public void addPhase(int phasenum, int stagenum)
    {

        GameControl.control.stagedata[stagenum].phases.Add(new Phase());
        GameControl.control.stagedata[stagenum].phases[phasenum].levels = new List<Level>();
        GameControl.control.stagedata[stagenum].phases[phasenum].levels.Clear();
        inactivebuttons.Clear();
        phase[stagenum].intArray[phasenum].SetActive(true);
        int q = 0;
       // GameControl.control.stagedata[GameControl.control.Stage].phases.Add(new Phase());
        foreach (Button butt in phase[stagenum].intArray[phasenum].GetComponentsInChildren<Button>())
        {
            phase[stagenum].intArray[phasenum].SetActive(true);
            GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].levels.Add(new Level());

            GameControl.control.stagedata[stagenum].phases[phasenum].levels[q].number = GameControl.control.stagedata[GameControl.control.Stage].highestReachedLevel + q;
            GameControl.control.stagedata[stagenum].phases[phasenum].levels[q].stage = stagenum;
            GameControl.control.stagedata[stagenum].phases[phasenum].levels[q].phase = phasenum;
            GameControl.control.stagedata[stagenum].phases[phasenum].levels[q].randomSeed = Random.Range(0, 255);
            Level level = GameControl.control.stagedata[stagenum].phases[phasenum].levels[q];
            butt.onClick.AddListener(delegate { loadSelectedLevel(level); });
            butt.onClick.AddListener(delegate { changebuttoncolor(butt); });
            butt.GetComponentInChildren<Text>().text = level.number.ToString();

            inactivebuttons.Enqueue(butt);

            butt.interactable = false;
            q++;

        }

        //foreach(Level l in GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].levels)
        //{
        //    l.number = GameControl.control.stagedata[GameControl.control.Stage].highestReachedLevel + q;
        //    l.randomSeed = Random.Range(0,255);
        //}
    }
    public void AddStage(int stage)
    {
        StagePanel[stage].SetActive(true);


    }


    public void AddButton(Level level)
    {
        foreach (Button btn in phase[GameControl.control.Stage].intArray[GameControl.control.phasenum].gameObject.GetComponentsInChildren<Button>())
        {
            btn.GetComponent<Image>().color = Color.white;
        }
        Button levelButtonClone = inactivebuttons.Dequeue();
       
        levelButtonClone.interactable = true;// = Instantiate(LevelButton, StagePanel[mystage].gameObject.transform);
        
        levelButtonClone.GetComponent<Image>().color = Color.yellow;
    }
    private void OnEnable()
    { 
        loadStagedata();
    }
    
    void loadSelectedLevel(Level lNumber)
    {
        GameControl.control.leveltoLoad = lNumber;
        GameControl.control.LevelNumber = lNumber.number;
       // GameControl.control.doGenerateNextLevel = false;
        gameObject.SetActive(false);
        SceneManager.LoadScene("Board");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateNewLevel()
    {

    }






    void ScanLevelButtons (int container)
    {

    }




























}
