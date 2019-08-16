using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{


    [System.Serializable]
    public class Cell
    {
        public bool visited;
        public GameObject north;//1
        public GameObject east;//2
        public GameObject west;//3
        public GameObject south;//4
    }


    public GameObject Spikeywall;
    public GameObject lockedWall;
    public GameObject wall;
    public Vector2 wallLength = new Vector2(63.0f, 63.0f);

    public static int xSize;// = 2;
    public static int ySize;// = 3;

    public GameObject wallHolder;
    private Vector2 initialPos;
    private Vector2 initialPosy;
    public Cell[] cells;
    private int currentCell = 0;
    private int totalCells;
    private int visitedCells = 0;
    private bool startedBuilding = false;
    private int currentNeighbour = 0;
    private List<int> lastCells;
    private int backingUp = 0;
    private int wallToBreak = 0;
    Vector2 player;

    private Vector2 ScreenSize;
    int complexity;//= 1 +(GameControl.control.LevelNumber/10);
                   // Start is called before the first frame update

    void Awake()
    {

        if (GameControl.control.Stage == 0)
        {
                xSize = 2;
                ySize = 4;

        }
        if (GameControl.control.Stage == 1)
        {
            xSize = 3;
            ySize = 5;

        }
        if (GameControl.control.Stage == 2)
        {
            xSize = 4;
            ySize = 8;

        }
        
        ScreenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        //if (GameControl.control.doGenerateNextLevel == true)
        //{
            Random.InitState(GameControl.control.leveltoLoad.randomSeed);
            if (gameObject.GetComponent<AdventureManager>() != null)
            {
                CreateAdventureWalls();
            }
            else
            {
                CreateWalls();
            }
            Transform[] wl = wallHolder.GetComponentsInChildren<Transform>();
            Vector2 CP = calculateCentroid(wl);
            wallHolder.transform.position = wallHolder.transform.position - (new Vector3(CP.x, CP.y, 0));

        //}
        //else
        //{

        //   restoreWalls(GameControl.control.leveltoLoad.Maze);
        //}


    }
    public Vector2 calculateCentroid(Transform[] centerPoints)
    {
        Vector2 centroid = new Vector2(0, 0);
        int numPoints = centerPoints.Length;
        foreach (Transform point in centerPoints)
        {
            Vector2 point2D = new Vector2(point.position.x, point.position.y);
            centroid += point2D;
        }

        centroid /= numPoints;

        return centroid;
    }

    void CreateAdventureWalls()
    {
        xSize = 5;
        ySize = 5;
        wallHolder = new GameObject
        {
            name = "AdventureMaze",
            //  tag = "maze"
        };
        wallLength.x = 3;
        wallLength.y = 3;
        initialPos = new Vector2((-xSize / 2) + wallLength.x / 2, (-ySize / 2) + wallLength.y / 2);

        Vector2 myPos = initialPos;
        GameObject tempWall;

        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j <= xSize; j++)
            {
                myPos = new Vector2(initialPos.x + (j * wallLength.x) - wallLength.x / 2, initialPos.y + (i * wallLength.y) - wallLength.y / 2);
                //print(i + j + " " + myPos.x +" ," + myPos.y);
                if (Random.value > 0.1f)
                {
                    tempWall = Instantiate(wall, myPos, Quaternion.identity) as GameObject;
                    Bounds bounds = tempWall.GetComponent<SpriteRenderer>().sprite.bounds;
                    float stretchToWorldScale = bounds.size.y;
                    // print(tempWall.transform.lossyScale);
                    tempWall.transform.localScale = new Vector3(0.1f, (wallLength.y / stretchToWorldScale), 1);
                    tempWall.transform.parent = wallHolder.transform;
                }
                else
                {
                    tempWall = Instantiate(Spikeywall, myPos, Quaternion.identity) as GameObject;
                    Bounds bounds = tempWall.GetComponent<SpriteRenderer>().sprite.bounds;
                    float stretchToWorldScale = bounds.size.y;
                    // print(tempWall.transform.lossyScale);
                    tempWall.transform.localScale = new Vector3(0.1f, (wallLength.y / stretchToWorldScale), 1);
                    tempWall.transform.parent = wallHolder.transform;
                }

            }
        }
        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector2(initialPos.x + (j * wallLength.x), initialPos.y + (i * wallLength.y) - wallLength.y);
                if (Random.value > 0.03f)
                {
                    tempWall = Instantiate(wall, myPos, Quaternion.Euler(0, 0, 90.0f)) as GameObject;
                    Bounds bounds = tempWall.GetComponent<SpriteRenderer>().sprite.bounds;
                    float stretchToWorldScale = bounds.size.y;
                    tempWall.transform.localScale = new Vector3(0.1f, (wallLength.x / stretchToWorldScale), 1);
                    tempWall.transform.parent = wallHolder.transform;
                }
                else
                {
                    tempWall = Instantiate(Spikeywall, myPos, Quaternion.Euler(0, 0, 90.0f)) as GameObject;
                    Bounds bounds = tempWall.GetComponent<SpriteRenderer>().sprite.bounds;
                    float stretchToWorldScale = bounds.size.y;
                    tempWall.transform.localScale = new Vector3(0.1f, (wallLength.x / stretchToWorldScale), 1);
                    tempWall.transform.parent = wallHolder.transform;
                }

            }
        }
        CreateCells();
    }
  //  MazeContainer mazecont = new MazeContainer();
    void CreateWalls()
    {
        //mazecont.walls = new List<Wall>();
        complexity = 1 + (GameControl.control.LevelNumber / 10);
        // wallLength.x = complexity;// Camera.main.ViewportToScreenPoint(new Vector3(complexity, complexity)) ;
        // wallLength.y = complexity;
        //xSize = 1 + complexity;// / 6;
       // ySize = 2 + complexity;
        wallHolder = new GameObject
        {
            name = "Maze",
            //  tag = "maze"
        };
        // wallLength = Camera.main.ViewportToWorldPoint(new Vector2(wallLength.x, wallLength.y));
        wallLength = (new Vector2((ScreenSize.x / xSize) * 2f, (ScreenSize.y / ySize) * 2f));
        //wallLength = new Vector2(wallLength.x / xSize, wallLength.y / ySize);


        //print(wallLength);
        initialPos = new Vector2((-xSize / 2) + wallLength.x / 2, (-ySize / 2) + wallLength.y / 2);//////////////////
                                                                                                   // print("xSize: " + xSize + " Ysize: " + ySize + "initial Pos: " + initialPos + "wall Length: " + wallLength + "Screen Size: "+ ScreenSize);
                                                                                                   // print(initialPos.x + " ," + initialPos.y);
        Vector2 myPos = initialPos;
        GameObject tempWall;

        
        //print(VPcoords.x + ", " + VPcoords.y);
        //for x axis
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j <= xSize; j++)
            {
                myPos = new Vector2(initialPos.x + (j * wallLength.x) - wallLength.x / 2, initialPos.y + (i * wallLength.y) - wallLength.y / 2);
                //print(i + j + " " + myPos.x +" ," + myPos.y);
                if (Random.value > 0.1f)
                {
                    tempWall = Instantiate(wall, myPos, Quaternion.identity) as GameObject;
                    Bounds bounds = tempWall.GetComponent<SpriteRenderer>().sprite.bounds;
                    float stretchToWorldScale = bounds.size.y;
                    // print(tempWall.transform.lossyScale);
                    tempWall.transform.localScale = new Vector3(0.05f, (wallLength.y / stretchToWorldScale), 1);
                    tempWall.transform.parent = wallHolder.transform;
                 
                }
                else
                {
                    tempWall = Instantiate(Spikeywall, myPos, Quaternion.identity) as GameObject;
                    Bounds bounds = tempWall.GetComponent<SpriteRenderer>().sprite.bounds;
                    float stretchToWorldScale = bounds.size.y;
                    // print(tempWall.transform.lossyScale);
                    tempWall.transform.localScale = new Vector3(0.05f, (wallLength.y / stretchToWorldScale), 1);
                    tempWall.transform.parent = wallHolder.transform;

                    
                }

            }
        }

        //for y axis
        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector2(initialPos.x + (j * wallLength.x), initialPos.y + (i * wallLength.y) - wallLength.y);
                if (Random.value > 0.03f)
                {
                    tempWall = Instantiate(Spikeywall, myPos, Quaternion.Euler(0, 0, 90.0f)) as GameObject;
                    Bounds bounds = tempWall.GetComponent<SpriteRenderer>().sprite.bounds;
                    float stretchToWorldScale = bounds.size.y;
                    tempWall.transform.localScale = new Vector3(0.05f, (wallLength.x / stretchToWorldScale), 1);
                    tempWall.transform.parent = wallHolder.transform;
                }



                else
                {
                    tempWall = Instantiate(wall, myPos, Quaternion.Euler(0, 0, 90.0f)) as GameObject;
                    Bounds bounds = tempWall.GetComponent<SpriteRenderer>().sprite.bounds;
                    float stretchToWorldScale = bounds.size.y;
                    tempWall.transform.localScale = new Vector3(0.05f, (wallLength.x / stretchToWorldScale), 1);
                    tempWall.transform.parent = wallHolder.transform;    
                }


            }
        }
        CreateCells();
    }
    void CreateCells()
    {
        lastCells = new List<int>();
        lastCells.Clear();
        totalCells = xSize * ySize;
        GameObject[] walls;
        int children = wallHolder.transform.childCount;
        walls = new GameObject[children];
        cells = new Cell[xSize * ySize];
        int eastWestProcess = 0;
        int childProcess = 0;
        int termCount = 0;
        //gertchildren
        for (int i = 0; i < children; i++)
        {
            walls[i] = wallHolder.transform.GetChild(i).gameObject;
        }
        //Assign walls to cells
        for (int cellprocess = 0; cellprocess < cells.Length; cellprocess++)
        {
            if (termCount == xSize)
            {
                eastWestProcess++;
                termCount = 0;
            }
            cells[cellprocess] = new Cell();
            cells[cellprocess].east = walls[eastWestProcess];
            cells[cellprocess].south = walls[childProcess + (xSize + 1) * ySize];



            eastWestProcess++;

            termCount++;
            childProcess++;
            cells[cellprocess].west = walls[eastWestProcess];
            cells[cellprocess].north = walls[(childProcess + (xSize + 1) * ySize) + xSize - 1];
        }
        CreateMaze();
    }
    
    void CreateMaze()
    {
        while (visitedCells < totalCells)
        {
            if (startedBuilding) {
                GetNeighbour();
                if (cells[currentNeighbour].visited == false && cells[currentCell].visited == true)
                {
                    float r = Random.value;
                    if (r > 0.05)
                    {
                        BreakWall();

                    }

                    else LockWall();

                    cells[currentNeighbour].visited = true;
                    visitedCells++;
                    lastCells.Add(currentCell);
                    currentCell = currentNeighbour;
                    if (lastCells.Count > 0)
                    {
                        backingUp = lastCells.Count - 1;
                    }
                }
            }
            else
            {
                currentCell = Random.Range(0, totalCells);
                cells[currentCell].visited = true;
                visitedCells++;
                startedBuilding = true;
            }
            //.Log("Finished");
        }
    }

    void BreakWall()
    {
        switch (wallToBreak)
        {
            case 1:
                Destroy(cells[currentCell].north);
                
                break;
            case 2:
                Destroy(cells[currentCell].east); break;
            case 3:
                Destroy(cells[currentCell].west); break;
            case 4:
                Destroy(cells[currentCell].south); break;
        }
    }
    void SpinWall()
    {
        //print("spinwalled");
        switch (wallToBreak)
        {
            case 1:
                StartCoroutine(Spin(cells[currentCell].north)); break;
            case 2:
                StartCoroutine(Spin(cells[currentCell].east)); break;
            case 3:
                StartCoroutine(Spin(cells[currentCell].west)); break;
            case 4:
                StartCoroutine(Spin(cells[currentCell].south)); break;
        }
    }
    void LockWall()
    {
        GameObject key;
        GameObject Lock;
        //print("spinwalled");
        switch (wallToBreak)
        {
            case 1:
                Lock = Instantiate(lockedWall, transform.position, transform.rotation);


                Lock.transform.parent = cells[currentCell].north.transform;
                Lock.transform.localPosition = new Vector2(0, 0);
                // cells[currentCell].north.GetComponent<GameObject>().Equals ( lockedWall); 
                break;
            case 2:
                Lock = Instantiate(lockedWall, transform.position, transform.rotation);
                Lock.transform.parent = cells[currentCell].east.transform;

                Lock.transform.localPosition = new Vector2(0, 0);
                break;
            case 3:
                Lock = Instantiate(lockedWall, transform.position, transform.rotation);
                Lock.transform.parent = cells[currentCell].west.transform;
                Lock.transform.localPosition = new Vector2(0, 0);
                break;
            case 4:
                Lock = Instantiate(lockedWall, transform.position, transform.rotation);
                Lock.transform.parent = cells[currentCell].south.transform;
                Lock.transform.localPosition = new Vector2(0, 0);
                break;
        }
        //key = Instantiate(GetComponent<GameManager>().key,
        //            GetComponent<GameManager>().gameLayout.RandomWalk(GetComponent<GameManager>().StartSector).centroid,
        //            transform.rotation);
    }
    //void restoreWalls(MazeContainer mazecontainr)
    //{
    //    complexity = 1 + (GameControl.control.LevelNumber / 10);
     
    //    //xSize = Maze.xSize;// / 6;
    //    //ySize = 2 + complexity;
    //    wallHolder = new GameObject
    //    {
    //        name = "Maze",
    //        //  tag = "maze"
    //    };
    //    // wallLength = Camera.main.ViewportToWorldPoint(new Vector2(wallLength.x, wallLength.y));
    //    wallLength = (new Vector2((ScreenSize.x / xSize) * 2f, (ScreenSize.y / ySize) * 2f));
    //    //wallLength = new Vector2(wallLength.x / xSize, wallLength.y / ySize);


    //    GameObject newWall;
    //    foreach (Wall wl in mazecontainr.walls)
    //    {
    //        if(wl.type ==1)
    //        {
    //            newWall=   Instantiate(wall, new Vector2(wl.position.x,wl.position.y), Quaternion.Euler(wl.rotation)) as GameObject;
    //            Bounds bounds = newWall.GetComponent<SpriteRenderer>().sprite.bounds;
    //            float stretchToWorldScale = bounds.size.x;
    //            newWall.transform.localScale = new Vector3(wl.scale.x, wl.scale.y, 1);
    //            newWall.transform.parent = wallHolder.transform;
    //        }
    //        if (wl.type == 2)
    //        {
    //            newWall = Instantiate(Spikeywall, new Vector2(wl.position.x, wl.position.y), Quaternion.Euler(wl.rotation)) as GameObject;
    //            Bounds bounds = newWall.GetComponent<SpriteRenderer>().sprite.bounds;
    //            float stretchToWorldScale = bounds.size.x;
    //            newWall.transform.localScale = new Vector3(wl.scale.x, wl.scale.y, 1);
    //            newWall.transform.parent = wallHolder.transform;
    //        }
    //        if (wl.type == 3)
    //        {
    //            newWall = Instantiate(lockedWall, new Vector2(wl.position.x, wl.position.y), Quaternion.Euler(wl.rotation)) as GameObject;
    //            Bounds bounds = newWall.GetComponent<SpriteRenderer>().sprite.bounds;
    //            float stretchToWorldScale = bounds.size.x;
    //            newWall.transform.localScale = new Vector3(wl.scale.x, wl.scale.y, 1);
    //            newWall.transform.parent = wallHolder.transform;
    //        }
    //    }

    //}
    IEnumerator Spin(GameObject Wall)
    {
        while (true)
        {
            //print("spining");
            Wall.transform.Rotate(0, 0, 0.5f);
            yield return new WaitForEndOfFrame();
        }
    }
    void GetNeighbour()
    {

        int length = 0;
        int[] neighbours = new int[4];
        int[] connectingWall = new int[4];
        int check = 0;
        check = ((currentCell + 1) / xSize);
        check -= 1;
        check *= xSize;
        check += xSize;
        //west
        if (currentCell + 1 < totalCells && (currentCell + 1) != check)
        {
            if (cells[currentCell + 1].visited == false)
            {
                neighbours[length] = currentCell + 1;
                connectingWall[length] = 3;
                length++;
            }
        }

        //east
        if (currentCell - 1 >= 0 && currentCell != check)
        {
            if (cells[currentCell - 1].visited == false)
            {
                neighbours[length] = currentCell - 1;
                connectingWall[length] = 2;
                length++;
            }
        }


        //north
        if (currentCell + xSize < totalCells)
        {
            if (cells[currentCell + xSize].visited == false)
            {
                neighbours[length] = currentCell + xSize;
                connectingWall[length] = 1;
                length++;
            }
        }
        // south
        if (currentCell - xSize >= 0)
        {
            if (cells[currentCell - xSize].visited == false)
            {
                neighbours[length] = currentCell - xSize;
                connectingWall[length] = 4;
                length++;
            }
        }
        if (length != 0)
        {
            int theChosenOne = Random.Range(0, length);
            currentNeighbour = neighbours[theChosenOne];

            wallToBreak = connectingWall[theChosenOne];
        }
        else {
            if (backingUp > 0)
            {
                currentCell = lastCells[backingUp];
                backingUp--;
            }
        }
    }
   //public  Wall GetWall(int type, Vector3 rotation, Vector3 position,int i)
   //     {
   //     Wall mc = new Wall();
   //     mc.position = position;
   //     mc.rotation = rotation;
   //     mc.type= type;
   //     return mc;
   //     }
    // Update is called once per frame
    void Update()
    {
        
    }
}
