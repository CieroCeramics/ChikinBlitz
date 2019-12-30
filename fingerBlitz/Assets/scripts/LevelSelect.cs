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
    public GameObject Rewards1, Rewards2, Rewards3;
    public GameObject rewardStar;
    //public GameObject achievement1, achievement2, achievement3;
    public GameObject[] StagePanel;
    public MultiDimensionalGO[] phase;
    public MultiDimensionalGO[] Stars;
    // public GameObject[][]phase;
    public GameObject PhaseHolder;
    public GameObject AdventrBag;

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
    private void Update()
    {
                       if(achUn)
        {
             if (Input.touchCount > 0)
            {
                Rewards1.SetActive(false);
                Rewards2.SetActive(false);
                Rewards3.SetActive(false);
            }
        }
    }
    public bool achUn = false;
    public void unlockAchievement(float score)
    {
      //  Phase mPhase = GameControl.control.stagedata[stagenum].phases[phasenum];
      //  Stage mStage = GameControl.control.stag[stagenum];
        //foreach(Level l in )
        if (score>40000&&score<60000)
        {
            Rewards1.SetActive(true);
            rewardStar.GetComponent<Image>().color =new Color(0.601f, 0.4720343f, 0.361201f);
            GameControl.control.lives +=3;
            GameControl.control.times += 3;
            GameControl.control.zooms += 2;
            GameControl.control.flys += 2;


            //   achUn = true;// Stars[GameControl.control.Stage].intArray[0].SetActive(true);

        }
        if (score > 60000&&score<80000)
        {
            Rewards2.SetActive(true);
            rewardStar.GetComponent<Image>().color = new Color(0.771261f, 0.807471f, 0.811f);
            GameControl.control.lives += 3;
            GameControl.control.times += 3;
            GameControl.control.zooms += 2;
            GameControl.control.flys += 2;
            //achUn = true;//Stars[GameControl.control.Stage].intArray[1].SetActive(true);
        }
        if (score > 80000)
        {
            Rewards3.SetActive(true);
            rewardStar.GetComponent<Image>().color = new Color(0.9496731f, 0.9496731f, 4292453f);
            GameControl.control.lives += 3;
            GameControl.control.times += 3;
            GameControl.control.zooms += 2;
            GameControl.control.flys += 2;
            //  achUn = true;//Stars[GameControl.control.Stage].intArray[2].SetActive(true);
        }
    }
    public void openBag()
    {
        AdventrBag.SetActive(true);
        AdventureBag.aBag.setupBag();
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
        Phase mPhase = GameControl.control.stagedata[stagenum].phases[phasenum];
        Stage mStage = GameControl.control.stagedata[stagenum];
        int i = 0;
        inactivebuttons.Clear();
        foreach (Button button in phase[stagenum].intArray[phasenum].GetComponentsInChildren<Button>())
        {
            if (GameControl.control.stagedata[stagenum].phases[phasenum].levels == null)
            {
                print("nodata");
            }
           
            else if (i <= mPhase.levels.Count)
            {
                Level level = mPhase.levels[i];
                button.onClick.AddListener(delegate { loadSelectedLevel(level); });
                button.GetComponentInChildren<Text>().text = level.number.ToString();
               
                // inactivebuttons.Dequeue();
                
            }
            if(mPhase.levels[i].complete&&mStage.highestReachedLevel<=mPhase.levels[i].number)
            {
                mStage.highestReachedLevel = mPhase.levels[i].number+1;
                GameControl.control.stagedata[stagenum].highestReachedLevel = mPhase.levels[i].number+1;
            }
            if (mPhase.levels[i].number<=mStage.highestReachedLevel
                ||mPhase.levels[i].number==0
                ||stagenum<GameControl.control.numUnlockedStages)
            {
                foreach (Text t in button.GetComponentsInChildren<Text>())
                {
                    if (t.gameObject.tag == "ScoreAdder")
                    {
                      t.text = mPhase.levels[i].score.ToString();
                    }

                   else if (t.gameObject.tag == "lnum")
                    {
                        t.text = mPhase.levels[i].number.ToString();
                    }
                }
                button.interactable = true;

            }
            else
            {
                button.interactable = false;
                inactivebuttons.Enqueue(button);
            }
            i++;
        }
    }
    public void checkAchievments(int score, int stagenum)
    {
        if (score > 40000)
        {
             Stars[stagenum].intArray[0].SetActive(true);
            
        }
        if (score > 60000)
        {
              Stars[stagenum].intArray[1].SetActive(true);
        }
        if (score > 80000)
        {
               Stars[stagenum].intArray[2].SetActive(true);
        }
    }
    public void loadStagedata()
    {
        int i = 0;

        //foreach (Stage stage in GameControl.control.stagedata)
        for (int f = 0; f <= GameControl.control.numUnlockedStages; f++)
        {
            int ph = 0;
            StagePanel[f].SetActive(true);
            foreach (Phase phz in GameControl.control.stagedata[f].phases)
            {
                phase[f].intArray[phz.number].SetActive(true);
                scanButtons(f, phz.number);
                ph++;
            }
            checkAchievments(GameControl.control.stagedata[f].score, f);

            //i++;

        }
        GameControl.control.Save();
    }
    public void addPhase(int phasenum, int stagenum, bool toseed)
    {
        //int key = Random.Range(0, 4);
   GameControl.control.stagedata[stagenum].phases.Add(new Phase());
        GameControl.control.stagedata[stagenum].phases[phasenum].levels = new List<Level>();
        GameControl.control.stagedata[stagenum].phases[phasenum].levels.Clear();
        GameControl.control.stagedata[stagenum].phases[phasenum].number = phasenum;
        GameControl.control.stagedata[stagenum].phases[phasenum].keyLevel = Random.Range(0, 4);
        inactivebuttons.Clear();
        phase[stagenum].intArray[phasenum].SetActive(true);
        int q = 0;
        // GameControl.control.stagedata[GameControl.control.Stage].phases.Add(new Phase());
        foreach (Button butt in phase[stagenum].intArray[phasenum].GetComponentsInChildren<Button>())
        {

            // butt.gameObject.SetActive(true);
            GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].levels.Add(new Level());
                                                                                                                                                                                                                                          
            GameControl.control.stagedata[stagenum].phases[phasenum].levels[q].number = GameControl.control.stagedata[GameControl.control.Stage].highestReachedLevel + q;
            GameControl.control.stagedata[stagenum].phases[phasenum].levels[q].stage = stagenum;
            GameControl.control.stagedata[stagenum].phases[phasenum].levels[q].phase = phasenum;
            GameControl.control.stagedata[stagenum].phases[phasenum].levels[q].complete = false;
            GameControl.control.stagedata[stagenum].phases[phasenum].levels[q].score = 0;
            GameControl.control.stagedata[stagenum].phases[phasenum].completed = false;
            if (toseed)
            {
                GameControl.control.stagedata[stagenum].phases[phasenum].levels[q].randomSeed = Random.Range(0, 255);
            }

            Level level = GameControl.control.stagedata[stagenum].phases[phasenum].levels[q];
            butt.onClick.AddListener(delegate { loadSelectedLevel(level); });
            butt.onClick.AddListener(delegate { changebuttoncolor(butt); });
            foreach (Text t in butt.GetComponentsInChildren<Text>())
            {
                if (t.gameObject.tag == "ScoreAdder")
                {
                    t.text = level.score.ToString();
                }

                else if (t.gameObject.tag == "lnum")
                {
                    t.text = level.number.ToString();
                }
            }

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
                        if(achUn)
        {
            unlockAchievement(GameControl.control.stagedata[GameControl.control.Stage].score);
        }
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

    void CreateNewLevel()
    {

    }






    void ScanLevelButtons (int container)
    {

    }




























}
