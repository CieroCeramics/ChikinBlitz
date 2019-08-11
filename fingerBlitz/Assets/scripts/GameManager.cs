﻿using System.Collections;
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



    public static float gameSpeed = 1f;
    public int maxGameSpeed = 1;
    public bool gobackyn;
    public bool destroyAllBullets;

    public GameObject startScroll;
    public GameObject key;
    public GameObject next;
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
    public int mode =1;
    Vector3 originalPos;
    int difficulty;// = GameControl.control.LevelNumber;
    int timestouched = 0;
    Vector2 zoomPoint;
    public Partitions.Sector StartSector = new Partitions.Sector();

    Level thisLevel;// = new Level();
    // Start is called before the first frame update
    private void Awake()
    {
        

    }
    void Start()
    {
        gameSpeed= 0;
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
        gameLayout = new Partitions(Maze.xSize,Maze.ySize, screenSize);
        //gameLayout.xPartitions = 1 + complexity;
        //gameLayout.yPartitions = 2 + complexity;
        gameLayout.createSectors();
        //for (int i = 0; i < gameLayout.sectors.Length; i++) 
        //{

        //}
        if (GameControl.control.doGenerateNextLevel)
        {
            GameControl.control.LevelNumber++;
            thisLevel = new Level();
            newSeed();
        }
        else oldSeed();
        print(GameControl.control.LevelNumber + " " + thisLevel.number);
        //Instantiate(key, gameLayout.RandomWalk(StartSector).centroid, transform.rotation);
        player.transform.position = start.transform.position;
        //Vector2 pos = gameLayout.RandomWalk(StartSector).centroid;
        
       
        OpenTutorial(player, ("this is the Chicken, tap and move your finger to control it"));
       
        StartCoroutine(LevelInfoScroll());

        //Vector2.lerp
    }bool doStartScroll = true;
  
    void newSeed()
    {
        //thisLevel.start = new Ve;
        thisLevel.Maze = new MazeContainer();
        thisLevel.Maze.walls = new List<Wall>();
        //thisLevel.Maze = new Maze();
        CreateStartAndFinish();

        //  difficultySeed = 2;
        
        CreateGuys();
        
        Instantiate(key, StartSector.centroid, transform.rotation);
        CreateUpgrade();
        //GameObject[] MazeWalls = GetComponent<Maze>().wallHolder.GetComponentsInChildren<GameObject>().;
        


        //thisLevel.Maze= GetComponent<Maze>().wallHolder;
        thisLevel.number = GameControl.control.LevelNumber;
        //foreach (GameObject wall in GetComponent<Maze>().wallHolder.GetComponentsInChildren<GameObject>())
        //{
        //    wall.transform.parent = thisLevel.Maze.transform;
        //}
        GameControl.control.mazeContainer.Add(GetComponent<Maze>().wallHolder);
        
    }
    void oldSeed()
    {

        //Level thislevel = new Level();
            thisLevel=GameControl.control.leveltoLoad;
        start.transform.position = new Vector2(GameControl.control.leveltoLoad.start.x, GameControl.control.leveltoLoad.start.y);// new Vector2(thisLevel.start.x, thislevel.start.y);
        end.transform.position = new Vector2(GameControl.control.leveltoLoad.finish.x, GameControl.control.leveltoLoad.finish.y);//new Vector2(thisLevel.finish.x, thislevel.finish.y);
       foreach (SerializableVector2 pg in thisLevel.pguys)
        {
            Instantiate(pGuy, new Vector2(pg.x, pg.y), Quaternion.identity);
        }
        foreach (SerializableVector2 lg in thisLevel.lguys)
        {
            Instantiate(lGuy, new Vector2(lg.x, lg.y), Quaternion.identity);
        }
        //foreach (GameObject thing in GameControl.control.leveltoLoad.p)
        //{
        //    Instantiate(thing);
        //}
    }
    IEnumerator LevelInfoScroll()
        
    {
        float currentLerp =0f;
        float lerpTime = 4f;
        bool pass = false;
        Vector2 curPos = startScroll.GetComponentInChildren<Text>().rectTransform.localPosition;
        float passingPoint = (Camera.main.ScreenToWorldPoint(new Vector2(Screen.width+10, Screen.height))).x;
        while (doStartScroll)
        {
            //curPos = startScroll.GetComponentInChildren<Text>().transform.position;
            currentLerp += Time.deltaTime;
            float perc = currentLerp / lerpTime;
            if (currentLerp<=lerpTime)
            {
                perc = Easing.Quadratic.Out(perc);
                startScroll.GetComponentInChildren<Text>().rectTransform.localPosition =(Vector2.Lerp(new Vector2(curPos.x, curPos.y), new Vector2(Screen.width+10, curPos.y), perc));
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
        tutorialPanel.transform.position =Camera.main.WorldToScreenPoint(Item.transform.localPosition);
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
        
        thisLevel.lguys = new List<SerializableVector2>();
        thisLevel.lguys.Clear();
        thisLevel.pguys = new List<SerializableVector2>();
        thisLevel.pguys.Clear();
        float g = Random.value;
       
        
        if (difficulty %10==0)
        {
          //  difficulty -= difficulty;
        }
        for (int i = 1; i < difficulty%10; i++)
        {
            if (i < gameLayout.xPartitions * gameLayout.yPartitions-1)
            {


                //     print("Lguy" + i);
                if (Random.value > 0.4)
                {
                    Partitions.Sector spot = new Partitions.Sector();
                    spot = checknplace(lGuy.gameObject);
                    Instantiate(lGuy, spot.centroid, Quaternion.identity);
                    thisLevel.lguys.Add(spot.centroid);
                }
                else
                {
                    Partitions.Sector spot = new Partitions.Sector();
                    spot = checknplace(pGuy.gameObject);
                    Instantiate(pGuy, spot.centroid, Quaternion.identity);
                    thisLevel.pguys.Add(spot.centroid);
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
        start.transform.position = StartSector.centroid;
        thisLevel.start = new Vector2(start.transform.position.x, start.transform.position.y);
        //print("finish");
        Partitions.Sector endsect = checknplace(end);
        end.GetComponent<Finish>().createPath(0, end.GetComponent<Finish>().sectors);
        print(endsect.distance);
        //endsect = checknplace(end);
            
        
        end.transform.position = endsect.centroid;
        thisLevel.finish = new Vector2( end.transform.position.x, end.transform.position.y);
        //  Instantiate(end, new Vector2(Random.Range(-1.7f, 3.7f), Random.Range(-3.5f, 6.5f)), transform.rotation);
        //start.transform.position = gameLayout.sectors[Random.Range(1, gameLayout.sectors.Length)].centroid; //Randomer();

        //end.transform.position = gameLayout.sectors[Random.Range(0, gameLayout.sectors.Length)].centroid;
        //if (Vector2.Distance(start.transform.position, end.transform.position) < mindestdist)
        //{
        //   // Destroy(end);
        //    CreateStartAndFinish();

        //}
    }
    public void TimeStop()
    {

        if (GameControl.control.times > 0)
        {
            print("timestopped");
            maxGameSpeed /= 100;
            GameControl.control.times--;
        }

        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
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
    void storeWallsandLevel()
    {
        GameObject walhold = GetComponent<Maze>().wallHolder;
        int i = 0;
        foreach (Transform wall in walhold.GetComponentsInChildren<Transform>())
        {
            Wall wally = new Wall();
            wally.position = new Vector2(wall.position.x, wall.position.y);
            wally.rotation = wall.rotation.eulerAngles;
            wally.scale = new Vector2( wall.localScale.x, wall.localScale.y);
            if (wall.gameObject.tag == "wall")
            {
                wally.type = 1;
            }
            if (wall.gameObject.tag == "spikes")
            {
                wally.type = 2;
            }
            if (wall.gameObject.tag == "lock")
            {
                wally.type = 3;
            }
            thisLevel.Maze.walls.Add(wally);
            i++;
        }
        GameControl.control.stagedata[GameControl.control.Stage].levels.Add(thisLevel);
       // LevelSelect.lSelect.AddButton(thisLevel);
    }
    public void Died(int lives)
    {
        
    }
    public void Finished(string state, Sprite script, Sprite keepPlaying)
    {
        gameSpeed = 0;
        if (GameControl.control.doGenerateNextLevel)
        {
            storeWallsandLevel();
            LevelSelect.lSelect.AddButton(thisLevel);
        }
        next.gameObject.SetActive(true);
        // next.GetComponentInChildren<Text>().text = outcome;
        //next.GetComponentInChildren<Image>().sprite = tex;
        destroyAllBullets = true;
        int min = (int)Time.time / 60;
        int sec = (int)Time.time % 60;
        next.GetComponentInChildren<Text>().text = "Time:  " + min.ToString()+":"+sec.ToString() + "\n Score:  000000";
        foreach (Image img in next.GetComponentsInChildren<Image>())
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
            }
            if (img.tag == "livesFront")
            {
                if (state == "alive")
                {
                    img.GetComponentInChildren<Text>().text = GameControl.control.lives.ToString();
                    img.GetComponent<Rigidbody2D>().gravityScale = 0;

                }
                else if (state == "dead")
                {
                    img.GetComponentInChildren<Text>().text = (GameControl.control.lives + 1).ToString();
                    img.GetComponent<Animator>().Play("paper", 0, 0);
                    img.GetComponent<Rigidbody2D>().gravityScale = 1;

                }
                else if(state =="GO")
                {
                    Invoke ("GameOverCountdown", 3);
                }
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

    public virtual void loadLevel(bool goback)
    {
        gameSpeed = 1;
        goback = gobackyn;
        if (goback)
        {
         
            reloadLevel();
        }
        else if (goback == false)
        {
            GameControl.control.Save();
            GameControl.control.doGenerateNextLevel = true;
            if (GameControl.control.LevelNumber == 10)
            {
                GameControl.control.Stage++;
                SceneManager.LoadScene("TransitionScene");
            }
           
            SceneManager.LoadScene("Board");
            
        }
    }
    public void reloadLevel()
    {
        GameControl.control.leveltoLoad =thisLevel;
        GameControl.control.doGenerateNextLevel = false;
        SceneManager.LoadScene("Board");

        player.transform.position = start.transform.position;
        next.gameObject.SetActive(false);
        Time.timeScale = 1f;
        destroyAllBullets = false;
        player.GetComponent<Animator>().SetBool("Dead", false);
        
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
}


