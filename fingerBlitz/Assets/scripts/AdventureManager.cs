using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AdventureManager : GameManager
{


    //public Sprite nextLevl;
    //public Sprite restart;
    //public Sprite Win;
    //public Sprite Lose;
    //public Sprite GameOver;
    //public float gameSpeed = 1f;
    //public int maxGameSpeed = 1;
    //public bool gobackyn;
    //public bool destroyAllBullets;

    //public GameObject startScroll;
    //public GameObject key;
    //public GameObject next;
    //public GameObject start;
    //public GameObject end;
    //public GameObject player;
    //public LaserGuy lGuy;
    //public PhaserGuy pGuy;
    //public GameObject canvas;
    //public GameObject tutorialPanel;

    //public float numguys = 1;
    //public float minComfyDist = 0.3f;

   // public Partitions gameLayout;
    //public int difficultySeed;
    //public GameObject zoomCam;
    //public Upgrade upgrade;

    bool open;
    //int mode = 2;
    Vector3 originalPos;
    int difficulty;// = GameControl.control.LevelNumber;
    int timestouched = 0;
    Vector2 zoomPoint;
    //Partitions.Sector StartSector = new Partitions.Sector();


    // Start is called before the first frame update
    //private void Awake()
    //{
    //    GameControl.control.LevelNumber++;
    //}

    void Start()
    {
        
        difficulty = GameControl.control.LevelNumber;
        //GameControl.control.LevelNumber++;
        originalPos = Camera.main.transform.position;
        int complexity = 1 + (GameControl.control.LevelNumber / 10);

        open = true;
        StartCoroutine(OpenCloseInfo());
        Vector2 size = new Vector2(Maze.xSize * GetComponent<Maze>().wallLength.x, Maze.ySize * GetComponent<Maze>().wallLength.y);
        gameLayout = new Partitions(Maze.xSize, Maze.ySize, size);
       // gameLayout.xPartitions = 1 + complexity;
       // gameLayout.yPartitions = 2 + complexity;
        gameLayout.createSectors();
        //for (int i = 0; i < gameLayout.sectors.Length; i++) 
        //{

        //}

        CreateStartAndFinish();

        //  difficultySeed = 2;

        CreateGuys();

        Instantiate(key, StartSector.centroid, transform.rotation);
        player.transform.position = start.transform.position;
        OpenTutorial(player, ("this is the Chicken, tap and move your finger to control it"));
        CreateUpgrade();
        StartCoroutine(LevelInfoScroll());

        //Vector2.lerp
    }
    bool doStartScroll = true;

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
                startScroll.GetComponentInChildren<Text>().rectTransform.localPosition = (Vector2.Lerp(new Vector2(curPos.x, 0), new Vector2(Screen.width + 10, 0), perc));
                //startScroll.GetComponentInChildren<Text>().transform.Translate(Vector2.Lerp(new Vector2(curPos.x, 0), new Vector2(Screen.width, 0), perc));//right * 10f);// * Easing.Quadratic.InOut(1f));
                // transform.Translate()
                //currentLerp = 0;
            }


            //if(perc<=0.5)
            //{
            //    perc = Easing.Exponential.In(perc);
            //}
            //if (perc > 0.5)
            //{
            //    perc = Easing.Quartic.Out(perc);
            //}

            if (startScroll.GetComponentInChildren<Text>().text == ("START"))
            {
                doStartScroll = false;
                startScroll.SetActive(false);
                yield return null;
            }




            startScroll.GetComponentInChildren<Text>().text = ("Stage:1" + "\nLevel: " + GameControl.control.LevelNumber);






            //if (startScroll.GetComponentInChildren<Text>().transform.localPosition.x < passingPoint)
            //{

            //    startScroll.GetComponentInChildren<Text>().text = ("Stage:1" + "\nLevel: " + GameControl.control.LevelNumber);
            //    startScroll.GetComponentInChildren<Text>().transform.Translate(Vector2.Lerp(new Vector2(0, 0), new Vector2(Screen.width / 2, 0), perc));//right * 10f);// * Easing.Quadratic.InOut(1f));
            //}
            //else if ((startScroll.GetComponentInChildren<Text>().transform.localPosition.x >= passingPoint) &&
            //     (startScroll.GetComponentInChildren<Text>().transform.localPosition.x < Screen.width)  
            //    && (!pass))
            //{
            //    startScroll.GetComponentInChildren<Text>().transform.Translate(Vector2.Lerp(new Vector2(Screen.width / 2, 0), new Vector2(Screen.width ,0), perc));// * Easing.Quadratic.InOut(1f));
            //    pass = true;
            //    yield return new WaitForSeconds(1f);
            //}
            //else if (startScroll.GetComponentInChildren<Text>().transform.localPosition.x >= passingPoint && (pass))
            //{
            //    startScroll.GetComponentInChildren<Text>().transform.Translate(Vector2.right * 10f);// * Easing.Quadratic.InOut(1f));
            //    pass = true; ;

            //}
            if (startScroll.GetComponentInChildren<Text>().rectTransform.localPosition.x >= Screen.width)
            {
                startScroll.GetComponentInChildren<Text>().text = ("START");
                startScroll.GetComponentInChildren<Text>().transform.localPosition = new Vector2(0, 0); ;// * Easing.Quadratic.InOut(1f));
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
                    Instantiate(lGuy, checknplace(lGuy.gameObject).centroid, Quaternion.identity);
                }
                else
                {
                    Instantiate(pGuy, checknplace(pGuy.gameObject).centroid, Quaternion.identity);
                }
            }
        }

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

        }
    }

    void CreateStartAndFinish()
    {
        //print("Start");
        StartSector = checknplace(start);
        start.transform.position = StartSector.centroid;
        //print("finish");
        end.transform.position = checknplace(end).centroid;
        //  Instantiate(end, new Vector2(Random.Range(-1.7f, 3.7f), Random.Range(-3.5f, 6.5f)), transform.rotation);
        //start.transform.position = gameLayout.sectors[Random.Range(1, gameLayout.sectors.Length)].centroid; //Randomer();

        //end.transform.position = gameLayout.sectors[Random.Range(0, gameLayout.sectors.Length)].centroid;
        //if (Vector2.Distance(start.transform.position, end.transform.position) < mindestdist)
        //{
        //   // Destroy(end);
        //    CreateStartAndFinish();

        //}
    }
    //public void TimeStop()
    //{

    //    if (GameControl.control.times > 0)
    //    {
    //        print("timestopped");
    //        maxGameSpeed /= 100;
    //        GameControl.control.times--;
    //    }


    //}
    // Update is called once per frame
    void Update()
    {
        mode = 2;
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
    //public void Finished(bool alive, Sprite script, Sprite keepPlaying)
    //{
    //    next.gameObject.SetActive(true);
    //    next.GetComponentInChildren<Text>().text = outcome;
    //    next.GetComponentInChildren<Image>().sprite = tex;
    //    destroyAllBullets = true;
    //    int min = (int)Time.time / 60;
    //    int sec = (int)Time.time % 60;
    //    next.GetComponentInChildren<Text>().text = "Time:  " + min.ToString() + ":" + sec.ToString() + "\n Score:  000000";
    //    foreach (Image img in next.GetComponentsInChildren<Image>())
    //    {

    //        if (img.tag == "timeStop")
    //        {
    //            img.GetComponentInChildren<Text>().text = GameControl.control.times.ToString();
    //        }
    //        if (img.tag == "zoom")
    //        {
    //            img.GetComponentInChildren<Text>().text = GameControl.control.zooms.ToString();
    //        }
    //        if (img.tag == "fly")
    //        {
    //            img.GetComponentInChildren<Text>().text = GameControl.control.flys.ToString();
    //        }
    //        if (img.tag == "resultText")
    //        {
    //            img.sprite = script;
    //        }
    //        if (img.tag == "winloseButton")
    //        {
    //            img.sprite = keepPlaying;
    //        }
    //        if (img.tag == "livesFront")
    //        {
    //            if (alive)
    //            {
    //                img.GetComponentInChildren<Text>().text = GameControl.control.lives.ToString();
    //                img.GetComponent<Rigidbody2D>().gravityScale = 0;

    //            }
    //            else if (!alive)
    //            {
    //                img.GetComponentInChildren<Text>().text = (GameControl.control.lives + 1).ToString();
    //                img.GetComponent<Animation>().Play();
    //                img.GetComponent<Rigidbody2D>().gravityScale = 1;

    //            }
    //        }
    //        if (img.tag == "livesBack")
    //        {
    //            img.GetComponentInChildren<Text>().text = (GameControl.control.lives).ToString();
    //        }
    //    }

    //}



    public override void loadLevel(bool goback)
    {

        goback = gobackyn;
        if (goback)
        {
            reloadLevel();
        }
        else if (goback == false) SceneManager.LoadScene("AdventureBoard");

    }
    //public void reloadLevel()
    //{

    //    player.transform.position = start.transform.position;
    //    next.gameObject.SetActive(false);
    //    Time.timeScale = 1f;
    //    destroyAllBullets = false;
    //}

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
    //public IEnumerator screenShake()
    //{
    //    float amount = 0.7f;
    //    while (true)
    //    {
    //        if (amount > 0)
    //        {
    //            Camera.main.transform.localPosition = originalPos + Random.insideUnitSphere * 0.7f;
    //            amount -= 0.1f;
    //        }
    //        else { Camera.main.transform.localPosition = originalPos; }
    //        yield return new WaitForEndOfFrame();
    //    }
    //}
    //public IEnumerator screenPulse()
    //{
    //    int status = 1;
    //    while (true)
    //    {
    //        switch (status)
    //        {

    //            case 1:
    //                if (Camera.main.orthographicSize > 4f)
    //                {
    //                    Camera.main.orthographicSize -= 0.1f;
    //                }
    //                else { status++; }
    //                break;

    //            case 2:
    //                if (Camera.main.orthographicSize <= 5f)
    //                {
    //                    Camera.main.orthographicSize += 0.1f;
    //                }
    //                else
    //                {
    //                    Camera.main.orthographicSize = 5f;
    //                    StopCoroutine(screenPulse());
    //                }
    //                break;




    //        }
    //        yield return new WaitForSeconds(0);
    //    }

    //}
}


