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
    public Sprite Lose;
    public Sprite GameOver;
    public GameObject winLoseCanvas;
    public GameObject tutorialPanel;
    public GameObject levelSelector;
    public GameObject infoPanel;


    public static float gameSpeed = 1f;
    public float maxGameSpeed = 1;
    public bool gobackyn;
    //public bool destroyAllBullets;

    public GameObject startScroll;
    public GameObject key;
    public GameObject levelCard;
    public GameObject start;
    public GameObject end;
    public GameObject player;
    public LaserGuy lGuy;
    public PhaserGuy pGuy;




    public float numguys = 1;
    public float minComfyDist = 0.3f;

    public Partitions gameLayout;// =new Partitions((;
    public int difficultySeed;
    public GameObject zoomCam;
    public Upgrade upgrade;

    bool open;
    bool timestopactivate = false;
    public int mode = 1;
    Vector3 originalPos;
    int difficulty;// = GameControl.control.LevelNumber;
    int timestouched = 0;
    Vector2 zoomPoint;
    public Partitions.Sector StartSector = new Partitions.Sector();
    public bool itsoverman = false;
    public Level thisLevel;// = new Level();
    // Start is called before the first frame update
    int stack = 0;

    bool startTimeStop = false;
    private void Awake()
    {
        FSE = GetComponentInChildren<SpriteRenderer>();
        FSE.gameObject.SetActive(false);
        Time.timeScale = (1);
    }
    SpriteRenderer FSE;
    void Start()
    {
       // GetComponent<AdsManager>().Display_Banner();
        thisLevel = GameControl.control.leveltoLoad;
        GameControl.control.LevelNumber = thisLevel.number;
        gameSpeed = 0;
        LevelSelect.lSelect.gameObject.SetActive(false);
        if (GameControl.control.stagedata.Count == 0)
        {

            //GameControl.control.Stage = 0;
            GameControl.control.stagedata.Add(new Stage());
            //GameControl.control.stagedata[GameControl.control.Stage].levels = new List<Level>();
            //GameControl.control.stagedata = new List<Stage>();
        }

        Vector2 screenSize;
        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)));// * 0.5; //Grab the world-space position values of the start and end positions of the screen, then calculate the distance between them and store it as half, since we only need half that value for distance away from the camera to the edge
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)));// * 0.5f;


        //mode = 1;
        difficulty = GameControl.control.LevelNumber;

        originalPos = Camera.main.transform.position;
        int complexity = 1 + (GameControl.control.LevelNumber / 10);

        open = true;
        StartCoroutine(OpenCloseInfo());
        gameLayout = new Partitions(Maze.xSize, Maze.ySize, screenSize);

        gameLayout.createSectors();

        Random.InitState(thisLevel.randomSeed);
        CreateStartAndFinish();



        CreateGuys();

        Instantiate(key, StartSector.centroid, transform.rotation);
        CreateUpgrade();

        thisLevel.number = GameControl.control.LevelNumber;


        player.transform.position = start.transform.position;


        OpenTutorial(player, ("this is the Chicken, tap and move your finger to control it"));

        StartCoroutine(LevelInfoScroll());

    } bool doStartScroll = true;

    void newLevel()
    {

        //thisLevel.start = new Ve;
        // thisLevel.Maze = new MazeContainer();
        //thisLevel.Maze.walls = new List<Wall>();
        //thisLevel.Maze = new Maze();


    }
    void oldSeed()
    {
        Random.InitState(thisLevel.randomSeed);
        //Level thislevel = new Level();

    }
    IEnumerator LevelInfoScroll()

    {
        float currentLerp = 0f;
        float lerpTime = 4f;
        bool pass = false;
        Vector2 curPos = startScroll.GetComponentInChildren<Text>().rectTransform.localPosition;
        float passingPoint = (Camera.main.ScreenToWorldPoint(new Vector2(Screen.width + 10, Screen.height))).x;
        while (doStartScroll)
        {
            //curPos = startScroll.GetComponentInChildren<Text>().transform.position;
            currentLerp += Time.deltaTime;
            float perc = currentLerp / lerpTime;
            if (currentLerp <= lerpTime)
            {
                perc = Easing.Quadratic.Out(perc);
                startScroll.GetComponentInChildren<Text>().rectTransform.localPosition = (Vector2.Lerp(new Vector2(curPos.x, curPos.y), new Vector2(Screen.width + 10, curPos.y), perc));
                //startScroll.GetComponentInChildren<Text>().transform.Translate(Vector2.Lerp(new Vector2(curPos.x, 0), new Vector2(Screen.width, 0), perc));//right * 10f);// * Easing.Quadratic.InOut(1f));
                // transform.Translate()
                //currentLerp = 0;
            }


            if (perc <= 0.5)
            {
                perc = Easing.Exponential.In(perc);
            }
            if (perc > 0.5)
            {
                perc = Easing.Quartic.Out(perc);
            }

            if (startScroll.GetComponentInChildren<Text>().text == ("START"))
            {
                doStartScroll = false;
                startScroll.SetActive(false);
                yield return null;
            }




            startScroll.GetComponentInChildren<Text>().text = ("\nLevel: " + GameControl.control.LevelNumber);






            if (startScroll.GetComponentInChildren<Text>().transform.localPosition.x < passingPoint)
            {

                startScroll.GetComponentInChildren<Text>().text = ("Stage:1" + "\nLevel: " + GameControl.control.LevelNumber);
                startScroll.GetComponentInChildren<Text>().transform.Translate(Vector2.Lerp(new Vector2(0, 0), new Vector2(Screen.width / 2, 0), perc));//right * 10f);// * Easing.Quadratic.InOut(1f));
            }
            else if ((startScroll.GetComponentInChildren<Text>().transform.localPosition.x >= passingPoint) &&
                 (startScroll.GetComponentInChildren<Text>().transform.localPosition.x < Screen.width)
                && (!pass))
            {
                startScroll.GetComponentInChildren<Text>().transform.Translate(Vector2.Lerp(new Vector2(Screen.width / 2, 0), new Vector2(Screen.width, 0), perc));// * Easing.Quadratic.InOut(1f));
                pass = true;
                yield return new WaitForSeconds(1f);
            }
            else if (startScroll.GetComponentInChildren<Text>().transform.localPosition.x >= passingPoint && (pass))
            {
                startScroll.GetComponentInChildren<Text>().transform.Translate(Vector2.right * 10f);// * Easing.Quadratic.InOut(1f));
                pass = true; ;

            }
            if (startScroll.GetComponentInChildren<Text>().rectTransform.localPosition.x >= Screen.width)
            {
                startScroll.GetComponentInChildren<Text>().text = ("START");
                startScroll.GetComponentInChildren<Text>().transform.localPosition = new Vector2(0, curPos.y); ;// * Easing.Quadratic.InOut(1f));
                pass = false;
                yield return new WaitForSeconds(1f);

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


    void DrawZoomCam(float angle, int quadrant)
    {

    }
    void CreateGuys()
    {

        // thisLevel.lguys = new List<SerializableVector2>();
        // thisLevel.lguys.Clear();
        // thisLevel.pguys = new List<SerializableVector2>();
        // thisLevel.pguys.Clear();
        float g = Random.value;


        if (difficulty % 10 == 0)
        {
            //  difficulty -= difficulty;
        }
        for (int i = 1; i < difficulty % 10; i++)
        {
            if (i < gameLayout.xPartitions * gameLayout.yPartitions - 1)
            {


                //     print("Lguy" + i);
                if (Random.value > 0.4)
                {
                    Partitions.Sector spot = new Partitions.Sector();
                    spot = checknplace(lGuy.gameObject);
                    Instantiate(lGuy, spot.centroid, Quaternion.identity);
                    //thisLevel.lguys.Add(spot.centroid);
                }
                else
                {
                    Partitions.Sector spot = new Partitions.Sector();
                    spot = checknplace(pGuy.gameObject);
                    Instantiate(pGuy, spot.centroid, Quaternion.identity);
                    //  thisLevel.pguys.Add(spot.centroid);
                }
            }
        }

    }

    public void GameOverCountdown()
    {
        GameControl.control.stagedata.Clear();
        SceneManager.LoadScene("Title");
    }
    void CreateUpgrade()
    {
        //  upgrade = new Upgrade();
        if (Random.value > 0.1f)
        {
            int rando = Random.Range(0, gameLayout.sectors.Length);
            while (gameLayout.sectors[rando].centroid == new Vector2(start.transform.position.z, start.transform.position.y))
            { rando = Random.Range(0, gameLayout.sectors.Length); }
            Instantiate(upgrade, gameLayout.sectors[rando].centroid, transform.rotation);
            //  thisLevel.upgrade = upgrade.gameObject.transform.position ;
        }
    }

    void CreateStartAndFinish()
    {
        //print("Start");

        StartSector = checknplace(start);
        end.GetComponent<Finish>().sectors.Enqueue(StartSector);
        end.GetComponent<Finish>().PlaceFinish();
        start.transform.position = StartSector.centroid;
        // thisLevel.start = new Vector2(start.transform.position.x, start.transform.position.y);
        //print("finish");
       // Partitions.Sector endsect = checknplace(end);
        //end.GetComponent<Finish>().createPath(0, end.GetComponent<Finish>().sectors);
        //        print(endsect.distance);
        //endsect = checknplace(end);


       // end.transform.position = endsect.centroid;

    }
    public void TimeStop()
    {
        
        if (GameControl.control.times > 0)
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
            GameControl.control.times--;
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
                        journeyLength = 1 - (1 / (2 * stack));//Vector2.Distance(transform.position, touchPosition);
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

    void Update()
    {
        if (!timestopactivate)
        {
            StopCoroutine(returntonorm());
        }
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), player.transform.position )< 0.5f)
                    {
                    StartCoroutine(OpenUpgradePanel());
                    openinfo = true;
                }
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
                txt.text = GameControl.control.zooms.ToString();
            }
            else if (txt.gameObject.tag == ("timeStop"))
            {
                txt.text = GameControl.control.times.ToString();
            }
            else if (txt.gameObject.tag == ("fly"))
            {
                txt.text = GameControl.control.flys.ToString();
            }
            else if (txt.gameObject.tag == ("life"))
            {
                txt.text = GameControl.control.lives.ToString();
            }

        }

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
    Level nextLevel()
    {
        return GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].levels[(thisLevel.number + 1) % 5];
    }
    public void Finished(FinishState FS, Sprite script, Sprite keepPlaying)
    {
       // Time.timeScale = (0);
        itsoverman = true;
        //print("THIS +RIUGH HERE" + GameControl.control.stagedata[GameControl.control.Stage].levels.Count + " : " + thisLevel.number);
        gameSpeed = 0;
        int s = GameControl.control.stagedata[GameControl.control.Stage].highestReachedLevel;
        int p = thisLevel.number;
        bool DGNL = p + 1 > s ? true : false;
        // print("current phase:" + GameControl.control.phasenum + " max phases" + LevelSelect.lSelect.phase[GameControl.control.Stage].intArray.Length);

        switch (FS)
        {
            case FinishState.ALIVE:

                if (LevelSelect.lSelect.inactivebuttons.Count != 0)
                {
                    if (DGNL)
                    {
                        GameControl.control.stagedata[GameControl.control.Stage].highestReachedLevel = nextLevel().number;
                        LevelSelect.lSelect.AddButton(nextLevel());

                        GameControl.control.leveltoLoad = nextLevel();
                        // GameControl.control.seedToLoad = thisLevel.randomSeed++;
                    }

                }
                else
                {
                    GameControl.control.phasenum++;
                    if (GameControl.control.phasenum < LevelSelect.lSelect.phase[GameControl.control.Stage].intArray.Length)
                    {

                        GameControl.control.stagedata[GameControl.control.Stage].highestReachedLevel++;

                        LevelSelect.lSelect.addPhase(GameControl.control.phasenum, GameControl.control.Stage);
                        GameControl.control.leveltoLoad = nextLevel();
                        LevelSelect.lSelect.AddButton(GameControl.control.leveltoLoad);
                        // GameControl.control.stagedata[GameControl.control.Stage].highestReachedLevel = nextLevel().number;

                    }

                    else
                    {
                        GameControl.control.Stage++;
                        SceneManager.LoadScene("TransitionScene");
                    }
                }
                break;
            case FinishState.DEAD:

                GameControl.control.leveltoLoad = thisLevel;
                break;
            case FinishState.GAMEOVER:
                GameControl.control.stagedata[GameControl.control.Stage].highestReachedLevel -= (4 - LevelSelect.lSelect.inactivebuttons.Count);
                LevelSelect.lSelect.addPhase(GameControl.control.phasenum, GameControl.control.Stage);
                GameControl.control.leveltoLoad = GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].levels[0];
                LevelSelect.lSelect.AddButton(GameControl.control.leveltoLoad);
                Invoke("GameOverCountdown", 3);
                break;

        }



        levelCard.gameObject.SetActive(true);
        // next.GetComponentInChildren<Text>().text = outcome;
        //next.GetComponentInChildren<Image>().sprite = tex;
        // destroyAllBullets = true;
        int min = (int)Time.time / 60;
        int sec = (int)Time.time % 60;
        levelCard.GetComponentInChildren<Text>().text = "Time:  " + min.ToString() + ":" + sec.ToString() + "\n Score:  000000";
        foreach (Image img in levelCard.GetComponentsInChildren<Image>())
        {

            if (img.tag == "timeStop")
            {
                img.GetComponentInChildren<Text>().text = GameControl.control.times.ToString();
            }
            if (img.tag == "zoom")
            {
                img.GetComponentInChildren<Text>().text = GameControl.control.zooms.ToString();
            }
            if (img.tag == "fly")
            {
                img.GetComponentInChildren<Text>().text = GameControl.control.flys.ToString();
            }
            if (img.tag == "resultText")
            {
                img.sprite = script;
            }
            if (img.tag == "winloseButton")
            {
                img.sprite = keepPlaying;
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

    }

    public void OpenLevelSelect()
    {
        //levelSelector = GameObject.FindGameObjectWithTag("levelSelect");
        LevelSelect.lSelect.gameObject.SetActive(true);// levelSelector.SetActive(true);
    }

    public virtual void loadLevel(Level leveltoload)
    {
        if (5 % (GameControl.control.LevelNumber+1) == 0)
        {
            GetComponent<AdsManager>().HandleInterstitialAdEvents(true);
            GetComponent<AdsManager>().Display_Interstitial();
        }
        else
        {
            SceneManager.LoadScene("Board");
        }
        //gameSpeed = 1;

        //leveltoload = GameControl.control.leveltoLoad;
        //seed = GameControl.control.seedToLoad;
        GameControl.control.leveltoLoad = leveltoload;
        GameControl.control.Save();
        //GameControl.control.doGenerateNextLevel = true;
        
    }
    

    void newphase(int num)
    {
        LevelSelect.lSelect.addPhase(num, GameControl.control.Stage);
    }
    public void reloadLevel()
    {
      
        GameControl.control.leveltoLoad = thisLevel;
        //GameControl.control.doGenerateNextLevel = false;
        SceneManager.LoadScene("Board");

        player.transform.position = start.transform.position;
        levelCard.gameObject.SetActive(false);
        Time.timeScale = 1f;
        //destroyAllBullets = false;
        player.GetComponent<Animator>().SetBool("Dead", false);

    }

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
                    text.text = GameControl.control.flys.ToString();
                }
                if (text.tag == "zoom")
                {
                    text.text = GameControl.control.zooms.ToString();
                }
                if (text.tag == "timeStop")
                {
                    text.text = GameControl.control.times.ToString();
                }
                if (text.tag == "life")
                {
                    text.text = GameControl.control.lives.ToString();
                }
                if(text.tag =="stats")
                {
                  //  text.text = "S:"+GameControl.control.Stage+" L:"+GameControl.control.LevelNumber +"\n Time:"
                }
            }
            switch (openinfo)
            {
                case true:
                    if (infoPanel.transform.rotation.z <0)
                    {
                        infoPanel.transform.Rotate(0, 0, 200 * Time.deltaTime);

                    }
                    break;
                case false:

                    if (infoPanel.transform.rotation.z >-0.9f) 
                    {
                        infoPanel.transform.Rotate(0, 0, -200 * Time.deltaTime);

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


