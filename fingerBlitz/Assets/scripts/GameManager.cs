using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Sprite nextLevl;
    public Sprite restart;
    public Sprite Win;
    public Sprite PhaseComplete;
    public Sprite Lose;
    public Sprite GameOver;
    public GameObject winLoseCanvas;
    public GameObject tutorialPanel;
    public GameObject levelSelector;
    public GameObject infoPanel;
    public GameObject settingsPanel;



    public GameObject startScroll;
    public GameObject key;
    public GameObject Lock;
    public GameObject levelCard;
    public GameObject start;
    public GameObject end;
    public GameObject player;
    public GameObject chickie;
    public LaserGuy lGuy;
    public PhaserGuy pGuy;
    public Partitions.Sector StartSector = new Partitions.Sector();
    public Scrollbar scrollbar;



    public Text StageScoreText;
    public Text Scoretext;
    public Text Scoretextmultiplier;
    public Partitions gameLayout;// =new Partitions((;
   
    public GameObject zoomCam;
    public Upgrade upgrade;

    public static float gameSpeed = 1f;
    public float numguys = 1;
    public float minComfyDist = 0.3f;
    string pCircleScale = "circleScale";
    public float maxGameSpeed = 1;

     public int levelScore = 5000;
     public int difficultySeed;
     public int mode = 1;
     int difficulty;// = GameControl.control.LevelNumber;
     int timestouched = 0;
     int stack = 0;

     public bool itsoverman = false;
     public bool gobackyn;
     bool timestopactivate = false;
     bool startTimeStop = false;
     bool newseed;bool open;
     
    Vector3 originalPos;
   
    Vector2 zoomPoint;
    
    
    public Level thisLevel;// = new Level();
    // Start is called before the first frame update
   
    


    private void Awake()
    {
        scrollbar.onValueChanged.AddListener(scrollbarCallBack);
        FSE = GetComponentInChildren<SpriteRenderer>();
        FSE.gameObject.SetActive(false);
        float circleScale = PlayerPrefs.HasKey(pCircleScale) ? PlayerPrefs.GetFloat(pCircleScale):5 ;
        if (PlayerPrefs.HasKey(pCircleScale))
        {

        }
        player.GetComponent<DragMove>().radial.transform.localScale = new Vector2(circleScale, circleScale);
        Time.timeScale = (1);
    }
    void scrollbarCallBack(float value)
    {
        value = Mathf.Clamp(value, 0.1f, 1);
        float circleScale = 5 * value;
        player.GetComponent<DragMove>().radial.transform.localScale = new Vector2(circleScale, circleScale);
        PlayerPrefs.SetFloat(pCircleScale, circleScale);
       // GameControl.control.circleSize = value;

    }
    public Vector2 screenSize;
    SpriteRenderer FSE;
    void Start()
    {  
      
        newseed = (thisLevel.number==GameControl.control.stagedata[GameControl.control.Stage].highestReachedLevel);
       // GetComponent<AdsManager>().Display_Banner();
        thisLevel = GameControl.control.leveltoLoad;
        GameControl.control.Stage = thisLevel.stage;
        GameControl.control.LevelNumber = thisLevel.number;
        GameControl.control.phasenum = thisLevel.phase;
        gameSpeed = 0;
        LevelSelect.lSelect.gameObject.SetActive(false);
        AdventureBag.aBag.gameObject.SetActive(false);
        if (GameControl.control.stagedata.Count == 0)
        {
            GameControl.control.stagedata.Add(new Stage());
        }
        

        
        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)));// * 0.5; //Grab the world-space position values of the start and end positions of the screen, then calculate the distance between them and store it as half, since we only need half that value for distance away from the camera to the edge
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)));// * 0.5f;

        int a;

        if (GameControl.control.Stage == 0)
        {
           a =    5;
            difficulty = Mathf.Clamp(thisLevel.number, 0, a);//GameControl.control.LevelNumber;
                       
        }
        if (GameControl.control.Stage == 1)
        {
            a =  12;
            difficulty = Mathf.Clamp(thisLevel.number, 0, a);//GameControl.control.LevelNumber;

        }
        if (GameControl.control.Stage == 2)
        {
            a =  29;
            difficulty = Mathf.Clamp(thisLevel.number, 0, a);//GameControl.control.LevelNumber;

        }
        

        originalPos = Camera.main.transform.position;
        int complexity = 1 + (GameControl.control.LevelNumber / 10);
        open = true;
        StartCoroutine(OpenCloseInfo());
        gameLayout = new Partitions(Maze.xSize, Maze.ySize, screenSize);
        gameLayout.Dimensions = screenSize;
        gameLayout.createSectors();

        Random.InitState(thisLevel.randomSeed);
        //CreateStartAndFinish();

        StartCoroutine(Loadlevel());

       

        thisLevel.number = GameControl.control.LevelNumber;


       // GameControl.control.updateinfo();


        OpenTutorial(player, ("this is the Chicken, tap and move your finger to control it"));

        StartCoroutine(LevelInfoScroll());
        StartCoroutine(dropScore());
        GameControl.control.Save();
    } bool doStartScroll = true;
    public enum LevelState
    {
        LOADING, LOADED
    };
    public LevelState levelstate = LevelState.LOADING;

    IEnumerator dropScore()
    {
        while (true)
        {
            if(gameSpeed>0)
            {
            levelScore--;
            Scoretext.text = levelScore.ToString();
            yield return new WaitForSeconds(0.05f/gameSpeed);
             }
         yield return null;
         }
       
    }


  //  bool haskey = GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].completed
    IEnumerator Loadlevel()
    {
        while (levelstate == LevelState.LOADING)
        {
            if (new Vector2(end.transform.position.x, end.transform.position.y) == Vector2.zero)
            {
                //end.GetComponent<Finish>().sectors.Enqueue(StartSector);
                CreateStartAndFinish();

                yield return new WaitForSeconds(1f);
            }
            else
            {

                gameLayout.sectors[EndSector.number].inhabitants.Add(end);
                player.transform.position = start.transform.position;
                

                //
 
                    CreateUpgrade();

                if (thisLevel.number % 5 == 4)
                {
                    Lock = Instantiate(Lock, end.transform.position, transform.rotation);
                }
                CreateChickies();
                CreateGuys();
                levelstate = LevelState.LOADED;
                yield return null;
            }
        }
        
    }

    IEnumerator LevelInfoScroll()

    {
        float currentLerp = 0f;
        float lerpTime = 2f;
       
        Vector2 curPos = startScroll.GetComponentInChildren<Text>().rectTransform.localPosition;
        float passingPoint = Screen.width+10;//(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width + 10, Screen.height))).x;
        while (doStartScroll)
        {
            
            currentLerp += Time.deltaTime;
            float perc = currentLerp / lerpTime;
            perc = Easing.Quadratic.InOut(perc);
            

            if (startScroll.GetComponentInChildren<Text>().text == ("START"))
            {
                doStartScroll = false;
                startScroll.SetActive(false);
                yield return null;
            }

            startScroll.GetComponentInChildren<Text>().text = ("\nLevel: " + GameControl.control.LevelNumber);

            if (startScroll.GetComponentInChildren<Text>().transform.localPosition.x < passingPoint)
            {

                startScroll.GetComponentInChildren<Text>().text = ("Stage: " + GameControl.control.Stage + "\nLevel: " + GameControl.control.LevelNumber);
                startScroll.GetComponentInChildren<Text>().transform.Translate(Vector2.Lerp(new Vector2(-10, 0), new Vector2(Screen.width +10, 0), perc));//right * 10f);// * Easing.Quadratic.InOut(1f));
            }

            if (startScroll.GetComponentInChildren<Text>().rectTransform.localPosition.x >= Screen.width)
            {
                startScroll.GetComponentInChildren<Text>().text = ("START");
                startScroll.GetComponentInChildren<Text>().transform.localPosition = new Vector2(0, curPos.y); ;// * Easing.Quadratic.InOut(1f));
                yield return new WaitForSeconds(0.5f);

            }

            yield return new WaitForEndOfFrame();
        }
        StopCoroutine(LevelInfoScroll());
    }
    void OpenTutorial(GameObject Item, string itemDescription)
    {
        tutorialPanel.transform.position = Camera.main.WorldToScreenPoint(Item.transform.localPosition);
        tutorialPanel.GetComponentInChildren<Text>().text = itemDescription;

    }

    Partitions.Sector checknplace(GameObject Obj)
    {



        int rand = Random.Range(0, gameLayout.sectors.Length);
        while (gameLayout.sectors[rand].inhabitants.Count >= 1)
        {

            rand = Random.Range(0, gameLayout.sectors.Length);

        }
        if (gameLayout.sectors[rand].inhabitants.Count < 1)
        {
            gameLayout.sectors[rand].inhabitants.Add(Obj);

        }
        return gameLayout.sectors[rand];
    }


    void CreateGuys()
    {


        float g = Random.value;



        int j = 0;
        foreach (Partitions.Sector s in gameLayout.sectors)
        {
            if (s.inhabitants.Count == 0)
            {
                if(j<=difficulty)
                {
                    if (Random.value > 0.4)
                    {
                       
                        Instantiate(lGuy, s.centroid, Quaternion.identity);
                        //thisLevel.lguys.Add(spot.centroid);
                    }
                    else
                    {
                        
                        Instantiate(pGuy, s.centroid, Quaternion.identity);
                        //  thisLevel.pguys.Add(spot.centroid);
                    }
                }
                j++;
            }
        }
        //for (int i = 1; i < difficulty % 10; i++)
        //{
        //    if (i < gameLayout.xPartitions * gameLayout.yPartitions - 2)
        //    {


        //        //     print("Lguy" + i);
        //        if (Random.value > 0.4)
        //        {
        //            Partitions.Sector spot = new Partitions.Sector();
        //            spot = checknplace(lGuy.gameObject);
        //            //while(spot.distance)
        //            Instantiate(lGuy, spot.centroid, Quaternion.identity);
        //            //thisLevel.lguys.Add(spot.centroid);
        //        }
        //        else
        //        {
        //            Partitions.Sector spot = new Partitions.Sector();
        //            spot = checknplace(pGuy.gameObject);
        //            Instantiate(pGuy, spot.centroid, Quaternion.identity);
        //            //  thisLevel.pguys.Add(spot.centroid);
        //        }
        //    }
        //}

    }
    public void hideMultiplier()
    {
    Scoretextmultiplier.gameObject.SetActive(false);
    }
    public bool keyExists = false;
      
    
    
    
    
    //void CreateKey()
    //{

    //    keyExists = true;
    //    float g = Random.value;
    //    //  GameObject keyClone;
    //    Partitions.Sector spot = new Partitions.Sector();
    //    spot = checknplace(key);

    //    GameObject keyClone = Instantiate(key, spot.centroid, Quaternion.identity);
    //    //  thisLevel.pguys.Add(spot.centroid);
    //    //keyClone.GetComponent<Chickie>().Home = spot;
    //}
    void CreateChickies()
    {

       
        //float g = Random.value;
      //  GameObject chickieClone; 
        Partitions.Sector spot = new Partitions.Sector();
        spot = checknplace(chickie);
        
      GameObject ChickieClone =  Instantiate (chickie, spot.centroid, Quaternion.identity);
                    //  thisLevel.pguys.Add(spot.centroid);
          ChickieClone.GetComponent<Chickie>().Home = spot;      
     }
    public void GameOverCountdown()
    {
        AdventureBag.aBag.zooms = 0;
        AdventureBag.aBag.times = 0;
        AdventureBag.aBag.flys = 0;
        GameControl.control.lives = 3;
        GameControl.control.Save();
        SceneManager.LoadScene("Title");
    }
    void CreateUpgrade()
    {
        //  upgrade = new Upgrade();
      
            //int rando = Random.Range(0, gameLayout.sectors.Length);
            //while (gameLayout.sectors[rando].centroid == new Vector2(start.transform.position.z, start.transform.position.y))
            //{ rando = Random.Range(0, gameLayout.sectors.Length); }
            Instantiate(upgrade, checknplace(upgrade.gameObject).centroid, transform.rotation);
            //  thisLevel.upgrade = upgrade.gameObject.transform.position ;
        
    }
Partitions.Sector EndSector = new Partitions.Sector();
   public void CreateStartAndFinish()
    {
        //print("Start");

        end.GetComponent<Finish>().ResetPartitions(gameLayout.sectors);
        StartSector = checknplace(start);
        
        end.GetComponent<Finish>().sectors.Enqueue(StartSector);
        
        start.transform.position = StartSector.centroid;
       EndSector = end.GetComponent<Finish>().createPath(0, end.GetComponent<Finish>().sectors);
       if(EndSector!=null){
        end.transform.position = EndSector.centroid;
        }
        //gameLayout.getSectorFromVector(end.transform.position,gameLayout.sectors).inhabitants.Add(end);
       
      
        // end.transform.position = endsect.centroid;

    }
    public void TimeStop()
    {
        
        if (AdventureBag.aBag.times > 0)
        {
            if (timestopactivate) { maxGameSpeed /= 2; stack++; }
            if (!timestopactivate)
            {
                timestopactivate = true;
                stack++;
                StartCoroutine(returntonorm());
                FSE.gameObject.SetActive(true);
                print("timestopped");
                maxGameSpeed /= 2;
                FSE.color = new Color(0.8f, 0.8f, 0, -0.5f);
            }
            AdventureBag.aBag.times--;
        }


    }
    float startTime = 0;
    int count = 0;
    float journeyLength = 0;
    float incrementor3000 = 0;
    bool openinfo;
    IEnumerator returntonorm()
    {
        while (timestopactivate)
        {

            if (FSE.color.a < 0.5f && !startTimeStop)
            {
                FSE.color += new Color(0f, 0f, 0, 0.05f);
            }
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                count++;
                if (touch.phase == TouchPhase.Began)
                {
                    yield return new WaitForEndOfFrame();
                    print(count + " " + stack);
                    if (count > stack && !startTimeStop)
                    {

                        startTime = Time.time;
                        journeyLength = 1 - (1 / (2 * stack));//we are halving the speed each TimeStop and subtracting from the end point (1 normal speed)
                        //Vector2.Distance(transform.position, touchPosition);
                                                              //Vector2 dirr = new Vector2(transform.position.x, transform.position.y) - new Vector2(touchPosition.x, touchPosition.y);
                        startTimeStop = true;

                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {


                }
                if (startTimeStop)
                {
                    float distCovered = (Time.time - startTime) * 0.0002f;
                    float fracJourney = Easing.Exponential.Out(distCovered / journeyLength);


                    maxGameSpeed = Mathf.Lerp(maxGameSpeed, 1, fracJourney);
                    print("SPEED: " + maxGameSpeed);
                    if (FSE.color.a > 0.0f)
                    {
                        FSE.color -= new Color(0, 0, 0, fracJourney / 2.5f);
                    }
                    else { FSE.gameObject.SetActive(false); }
                    if (maxGameSpeed >= 0.9f)
                    {
                        maxGameSpeed = 1;
                        count = 0; timestopactivate = false; FSE.gameObject.SetActive(false);
                        startTimeStop = false;
                        stack = 0;
                        yield break;
                    }

                }

            }
            yield return new WaitForFixedUpdate();
        }
    }
    // Update is called once per frame
    bool startFlag = false;
    void Update()
    {
        if(!startFlag)//do last minute operations
        {

            startFlag = true;
        }
        if (!timestopactivate)
        {
            StopCoroutine(returntonorm());
        }
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), player.transform.position )< 0.5f)
                //    {
                //    StartCoroutine(OpenUpgradePanel());
                //    openinfo = true;
                //}
            }
                open = false;
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                open = true;
            }
        }
                                
        foreach (Text txt in winLoseCanvas.GetComponentsInChildren<Text>())
        {
            if (txt.gameObject.tag == ("zoom"))
            {
                txt.text = AdventureBag.aBag.zooms.ToString();
            }
            else if (txt.gameObject.tag == ("timeStop"))
            {
                txt.text = AdventureBag.aBag.times.ToString();
            }
            else if (txt.gameObject.tag == ("fly"))
            {
                txt.text = AdventureBag.aBag.flys.ToString();
            }
            else if (txt.gameObject.tag == ("life"))
            {
                txt.text = GameControl.control.lives.ToString();
            }

        }

    }

    public void openSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        bool open= settingsPanel.activeSelf;
        if (open)
        {
            player.GetComponent<DragMove>().radial.SetActive(true);
            player.GetComponent<DragMove>().radial.transform.position = new Vector2(786.6f, 1282.6f) ;
        }
        else player.GetComponent<DragMove>().radial.SetActive(false);
    }
    public void assignControls(int type)
    {
        GameControl.control.controlType = type;
    }
    public void openMenu() {
        StartCoroutine(OpenUpgradePanel());
        openinfo = true;
    }
    //void storeWallsandLevel()
    //{
    //    GameObject walhold = GetComponent<Maze>().wallHolder;
    //    int i = 0;
    //    foreach (Transform wall in walhold.GetComponentsInChildren<Transform>())
    //    {
    //        Wall wally = new Wall();
    //        wally.position = new Vector2(wall.position.x, wall.position.y);
    //        wally.rotation = wall.rotation.eulerAngles;
    //        wally.scale = new Vector2( wall.localScale.x, wall.localScale.y);
    //        if (wall.gameObject.tag == "wall")
    //        {
    //            wally.type = 1;
    //        }
    //        if (wall.gameObject.tag == "spikes")
    //        {
    //            wally.type = 2;
    //        }
    //        if (wall.gameObject.tag == "lock")
    //        {
    //            wally.type = 3;
    //        }
    //        //thisLevel.Maze.walls.Add(wally);
    //        i++;
    //    }
    //    GameControl.control.stagedata[GameControl.control.Stage].levels.Add(thisLevel);
    //   // LevelSelect.lSelect.AddButton(thisLevel);
    //}
    public void Died(int lives)
    {

    }
    public enum FinishState
    {
        DEAD, ALIVE, GAMEOVER
    };  
    void getphasescore ()
    {
       // int fakescore=0;
        foreach (Level l in GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].levels)
        {
            GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].score += l.score;
        }
    }

    IEnumerator scoreup(int scoreToAdd)
    {

        int oldscore = GameControl.control.stagedata[GameControl.control.Stage].score;
        int newscore = GameControl.control.stagedata[GameControl.control.Stage].score + scoreToAdd;
       // getphasescore();
        GameControl.control.stagedata[GameControl.control.Stage].score += scoreToAdd;
      //  int phzscr = GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].score +fakenum;
     
        while(oldscore<newscore)
        {
            oldscore+=100;
                StageScoreText.text= "Stage Score: " + oldscore.ToString();
            if (oldscore>newscore)
            {
                oldscore = newscore;
                
            }
            yield return null ;
        }
            
    }
    Level nextLevel()
    {
        
        return GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].levels[(thisLevel.number + 1) % 5];
    }
    bool newstage=false;
    public void Finished(FinishState FS, Sprite script)
    {
        LevelSelect.lSelect.loadStagedata();
        Sprite keepPlaying = restart;
       //set a boolean to switch off any remaining actions
        itsoverman = true;
        StopCoroutine(dropScore());

        //stop bullets from moving
        gameSpeed = 0;
        scrollbar.onValueChanged.RemoveListener(scrollbarCallBack);
        switch (FS)
        {
            //the player wins!
            case FinishState.ALIVE:

                //Have we already beat this level?
                if (thisLevel.complete == true)
                {
                    keepPlaying = restart;
                    //did we get a new highscore?
                    if (thisLevel.score < levelScore)
                    {
                        //set our new level score switch to true
                        NLS.SetActive(true);
                        //calculate the new score
                        int dif = levelScore - thisLevel.score;
                        thisLevel.score = levelScore;
                        if (GameControl.control.stagedata[GameControl.control.Stage].phases[thisLevel.phase].completed)
                        {
                            StartCoroutine(scoreup(dif));
                        }
                        //GameControl.control.stagedata[GameControl.control.Stage].score += dif;
                       
                    }
                    
                     //make the button click load the same level
                    GameControl.control.leveltoLoad = thisLevel;
                }
                //did we beat a new level?
                else
                {
                    //set our new level score switch to true
                    NLS.SetActive(true);
                    //set the save data to load this level as complete
                    GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].levels[thisLevel.number%5].complete = true;
                    //make the button click load the next level
                    keepPlaying = nextLevl;
                    
                    //are there more levels in the phase?
                    if (LevelSelect.lSelect.inactivebuttons.Count != 0)
                    {
                         //calculate the new score
                        thisLevel.score = levelScore;
                        //if we decided we are going to genereate the next level

                        
                        GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].levels[thisLevel.number%5].complete = true;
                        //set the highest reached level reference to the number of the next level./
                        GameControl.control.stagedata[GameControl.control.Stage].highestReachedLevel = nextLevel().number;
                        //add a button to the phase
                        LevelSelect.lSelect.AddButton(nextLevel());

                        //make the button click load the next level
                        GameControl.control.leveltoLoad = nextLevel();



                    }
                    //is the phase complete?
                    else
                    {
                        //calculate the new score
                        thisLevel.score = levelScore;
                        //set the save data to load this phase as complete
                        GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].completed = true;
                        
                        //are there still phases in the stage?
                        if (GameControl.control.phasenum+1 < LevelSelect.lSelect.phase[GameControl.control.Stage].intArray.Length)
                        {
                            getphasescore();
                            StartCoroutine(scoreup(GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].score));
                            //change the phase num to reference the next phase
                            GameControl.control.phasenum++;
                            //check if achievements were unlocked
                            LevelSelect.lSelect.achUn = true;
                           // LevelSelect.lSelect.unlockAchievement(GameControl.control.stagedata[GameControl.control.Stage].score);
                          
                             //set the next level number as the highest reached level.
                            GameControl.control.stagedata[GameControl.control.Stage].highestReachedLevel++;

                            //add a new instance of a phase to the stage
                            LevelSelect.lSelect.addPhase(GameControl.control.phasenum, GameControl.control.Stage, true);
                            //make the button click load the next level
                            GameControl.control.leveltoLoad = nextLevel();
                          //add a new button to the phase.   
                            LevelSelect.lSelect.AddButton(GameControl.control.leveltoLoad);
                           

                        }
                           //did we complete the stage?
                        else
                        {
                            //change the Stage num to reference the next stage. 
                            GameControl.control.Stage++;
                            // GameControl.control.leveltoLoad = nextLevel();
                            newstage = true;
                        }
                    }
                }
                break;    //end the operation
                //the player died    but still has lives.
            case FinishState.DEAD:
                //set the button to restart button graphic
                keepPlaying = restart;
                //if this was not an old level
                if (!thisLevel.complete)
                {
                    // did the player have a key?
                    if (GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].keyLevel == thisLevel.number)

                    {
                        //well, not anymore. 
                        GameControl.control.key = false;
                    }
                    //lose a life too. 
                    GameControl.control.lives--;
                }
                else// if this was an old level
                {
                    if (thisLevel.number==GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].levels[4].number)

                    {
                        //make sure the doors unlocked
                        GameControl.control.key = true;
                    }
                }
                //make the button click load the same level
                GameControl.control.leveltoLoad = thisLevel;
                break;
                //the player died and has no lives
            case FinishState.GAMEOVER:
                //the player didnt complete this level
                if (!thisLevel.complete)
                {
                    //the probelm here is I need to set the Buttons back to the disabled queue
                    //take away all the progress of the current stage. 
                    //make the highest reached level the last level of the previous phase. 
                    GameControl.control.stagedata[GameControl.control.Stage].highestReachedLevel -= (4 - LevelSelect.lSelect.inactivebuttons.Count);
                    // erase all memory of the current phase
                    GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].levels.Clear();
                    //just completely removin the thing. better check into this line of code...................................
                    GameControl.control.stagedata[GameControl.control.Stage].phases.RemoveAt(GameControl.control.phasenum);
                    //add a new instance of a phase to the stage
                    LevelSelect.lSelect.addPhase(GameControl.control.phasenum, GameControl.control.Stage, true);
                    //make the button click load the first level if the phase
                    GameControl.control.leveltoLoad = GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].levels[0];
                    //add the new levels button
                    LevelSelect.lSelect.AddButton(GameControl.control.leveltoLoad);

                    Invoke("GameOverCountdown", 3);
                }
                //  the player did complete this level
                else GameControl.control.leveltoLoad = thisLevel;
                break;

        }
      //  GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].levels[thisLevel.number % 5] = thisLevel;
                       //save everything for next load session
        GameControl.control.Save();
        //make the  level card appear
        levelCard.gameObject.SetActive(true);
        //calculate how much time has passed
        int min = (int)Time.time / 60;
        int sec = (int)Time.time % 60;
        levelCard.GetComponentInChildren<Text>().text = "Time:  " + min.ToString() + ":" + sec.ToString() + "\n Score: " + levelScore;
        foreach (Image img in levelCard.GetComponentsInChildren<Image>())
        {

            if (img.tag == "stats")
            {
                img.GetComponentInChildren<Text>().text = "Score " + levelScore +"\n + Time: ";
            }
            if (img.tag == "timeStop")
            {
                img.GetComponentInChildren<Text>().text = AdventureBag.aBag.times.ToString();
            }
            if (img.tag == "zoom")
            {
                img.GetComponentInChildren<Text>().text = AdventureBag.aBag.zooms.ToString();
            }
            if (img.tag == "fly")
            { 
                        img.GetComponentInChildren<Text>().text = AdventureBag.aBag.flys.ToString();
            }
          
                            if (img.tag == "resultText")
            {
                img.GetComponent<Image>().sprite = script;
            }
            if (img.tag == "winloseButton")
            {
                img.GetComponent<Image>().sprite = keepPlaying;
                //Level lv = GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum]
                img.gameObject.GetComponent<Button>().onClick.AddListener(() => loadLevel(GameControl.control.leveltoLoad));
            }
            if (img.tag == "livesFront")
            {
                if (FS == FinishState.ALIVE)
                {
                    img.GetComponentInChildren<Text>().text = GameControl.control.lives.ToString();
                    img.GetComponent<Rigidbody2D>().gravityScale = 0;
                                           
                }
                else if (FS == FinishState.DEAD)
                {

                    img.GetComponentInChildren<Text>().text = (GameControl.control.lives + 1).ToString();
                    img.GetComponent<Animator>().Play("paper", 0, 0);
                    img.GetComponent<Rigidbody2D>().gravityScale = 1;

                }
                //else if(FS == FinishState.GAMEOVER)
                //{
                //    Invoke ("GameOverCountdown", 3);
                //}
            }
            if (img.tag == "livesBack")
            {
                img.GetComponentInChildren<Text>().text = (GameControl.control.lives).ToString();
            }
        }
        foreach (Text img in levelCard.GetComponentsInChildren<Text>())
        {
            if (img.tag == "LevelScoreText")
            {
                img.GetComponent<Text>().text = ("Level Score: " + thisLevel.score.ToString());
            }
            if (img.tag == "CountdownText")
            {
                int dif = levelScore - thisLevel.score;
                img.GetComponent<Text>().text = ("Stage Score: " + GameControl.control.stagedata[GameControl.control.Stage].score);
          
                
            }
          
   
        }
    }
    
    public GameObject NLS;
    public void OpenLevelSelect()
    {
        LevelSelect.lSelect.loadStagedata();
        //levelSelector = GameObject.FindGameObjectWithTag("levelSelect");
        LevelSelect.lSelect.gameObject.SetActive(true);// levelSelector.SetActive(true);
    }

    public void loadLevel(Level leveltoload)
    {
      
        if ( (GameControl.control.LevelNumber+1)%5 == 0)
        {
            GetComponent<AdsManager>().HandleInterstitialAdEvents(true);
            GetComponent<AdsManager>().Display_Interstitial();
        }
        else
        {
            if (newstage)
            {
                SceneManager.LoadScene("TransitionScene");
            }
            else SceneManager.LoadScene("Board");
        }
        //gameSpeed = 1;

        //leveltoload = GameControl.control.leveltoLoad;
        //seed = GameControl.control.seedToLoad;
        GameControl.control.leveltoLoad = leveltoload;
        
        //GameControl.control.doGenerateNextLevel = true;
        
    }
    

    //void newphase(int num)
    //{
    //    LevelSelect.lSelect.addPhase(num, GameControl.control.Stage,true);
    //}
    //public void reloadLevel()
    //{
      
    //    GameControl.control.leveltoLoad = thisLevel;
    //    //GameControl.control.doGenerateNextLevel = false;
    //    SceneManager.LoadScene("Board");

    //    player.transform.position = start.transform.position;
    //    levelCard.gameObject.SetActive(false);
    //    Time.timeScale = 1f;
    //    //destroyAllBullets = false;
    //    player.GetComponent<Animator>().SetBool("Dead", false);

    //}

    float st;
    float fl;
    IEnumerator OpenUpgradePanel()
    {
        while (true)
        {
            foreach(Text text in infoPanel.GetComponentsInChildren<Text>())
            {
                if(text.tag=="fly")
                {
                    text.text = AdventureBag.aBag.flys.ToString();
                }
                if (text.tag == "zoom")
                {
                    text.text = AdventureBag.aBag.zooms.ToString();
                }
                if (text.tag == "timeStop")
                {
                    text.text = AdventureBag.aBag.times.ToString();
                }
                if (text.tag == "life")
                {
                    text.text = GameControl.control.lives.ToString();
                }
                if(text.tag =="stats")
                {
                    text.text = ("Time"+ Time.time +
                        "\n Score: " + levelScore );
                  //  text.text = "S:"+GameControl.control.Stage+" L:"+GameControl.control.LevelNumber +"\n Time:"
                }
            }
            switch (openinfo)
            {
                case true:
                infoPanel.SetActive(true);
                    if (infoPanel.transform.rotation.z <0)
                    {
                        infoPanel.transform.Rotate(0, 0, 200 * Time.deltaTime);

                  }
                    break;
                case false:

                    if (infoPanel.transform.rotation.z > -0.9f)
                    {
                        infoPanel.transform.Rotate(0, 0, -200 * Time.deltaTime);

                    }
                    else
                    {
                        settingsPanel.SetActive(false);
                        infoPanel.SetActive(false);
                    }
                    break;
            }
            print(infoPanel.transform.rotation.z);
           
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator OpenCloseInfo()
    {
        while (true)
        {
            
            if (open && winLoseCanvas.GetComponentInChildren<Image>().fillAmount < 1)
            {
                winLoseCanvas.GetComponentInChildren<Image>().fillAmount += 0.1f;
                yield return new WaitForEndOfFrame();
            }
            else if (!open && winLoseCanvas.GetComponentInChildren<Image>().fillAmount > 0)
            {
             
                winLoseCanvas.GetComponentInChildren<Image>().fillAmount -= 0.1f;
                yield return new WaitForEndOfFrame();
            }
            //StopCoroutine(OpenCloseInfo());
            // StopCoroutine(OpenCloseInfo());
            yield return null;
        }

    }
    public IEnumerator screenShake()
    {
        float amount = 0.7f;
        while (true)
        {
            if (amount > 0)
            {
                Camera.main.transform.localPosition = originalPos + Random.insideUnitSphere * 0.7f;
                amount -= 0.1f;
            }
            else { Camera.main.transform.localPosition = originalPos; }
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator screenPulse()
    {
        int status = 1;
        while (true)
        {
            switch (status)
            {

                case 1:
                    if (Camera.main.orthographicSize > 4f)
                    {
                        Camera.main.orthographicSize -= 0.1f;
                    }
                    else { status++; }
                    break;

                case 2:
                    if (Camera.main.orthographicSize <= 5f)
                    {
                        Camera.main.orthographicSize += 0.1f;
                    }
                    else
                    {
                        Camera.main.orthographicSize = 5f;
                        StopCoroutine(screenPulse());
                    }
                    break;




            }
            yield return new WaitForSeconds(0);
        }

    }
    public void openinfoclicked()
    {
        print("??");
        openinfo = false;
    }
}


