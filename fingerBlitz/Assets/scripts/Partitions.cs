using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partitions // int xp, int yp, Vector2 p 
{
    public int xPartitions;// =2;
    public int yPartitions;//=3;
   public Vector2 Dimensions;

    public Partitions(int xParts, int yParts, Vector2 dimensions)
    {

        xPartitions = xParts;
        yPartitions = yParts;
        Dimensions = dimensions;
        totalPartitions = xPartitions * yPartitions;
    }



    public int totalPartitions ;
    public class Sector
    {
        public List<GameObject> inhabitants= new List<GameObject>();
        public List<Sector> neighbours = new List<Sector>();
        public  float north, east, south, west;
        public Vector2 centroid ;
        public int number;
        public int distance;
        public bool visited = false;
        public bool partOfPath;  
    }
    public GameObject t;
    public Sector[] sectors;
    // Start is called before the first frame update
    void Start()
    {
        //int complexity = 1 + (GameControl.control.LevelNumber / 10);
        xPartitions = Maze.xSize;// 1 + complexity;// / 6;
        yPartitions = Maze.xSize;// 2 + complexity;
       
        //if (GameControl.control.LevelNumber<5)
        //{
        //    xPartitions = 2;
        //    yPartitions = 3;
        //}
        //else if(GameControl.control.LevelNumber > 5)
        //{
        //    xPartitions = 3;
        //    yPartitions = 4;
        //}
        // createSectors();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    private Vector2 screenSize;
   public void createSectors()
    {
        //    t = new GameObject()
        //    {
        //        name = ("sector")
        //};

                //print(screenSize.x + ", " + screenSize.y);
        sectors = new Sector[xPartitions * yPartitions];
        float width = Dimensions.x;// screenSize.x;// Screen.width / 2;
        float height = Dimensions.y;//screenSize.y;// Screen.height / 2;
        int count = 0;
        for(int i=0; i<yPartitions;i++)
            for(int j=0; j<xPartitions;j++)
            {
                sectors[count] = new Sector
                {
                    north = (height / yPartitions) * (i + 1) - height / 2,
                    east = (width / xPartitions) * (j + 1) - width / 2,
                    south = (height / yPartitions) * i - height / 2,
                    west = (width / xPartitions) * j - width / 2
                };
                sectors[count].centroid = new Vector2((sectors[count].east + sectors[count].west) / 2, (sectors[count].north + sectors[count].south) / 2);
                ///sectors[count].constraints = Instantiate(t, sectors[count].centroid, transform.rotation);
                sectors[count].inhabitants = new List<GameObject>();
                sectors[count].number = count;
                count++;
            }
        //print("sectorsCreated");
    }
    public void Dijkstra(Sector Start)
    {

    }
    public void CheckPath(Sector one, Sector two)
    {

        List<Sector> AvailablePaths = new List<Sector>();

        AvailablePaths.Add(one);
        //if(one)
        Sector currentSector = one;
        //north
        //while (currentSector != null)
          

    }


    public Sector RandomWalk2(Sector start, int minDist)
    {
        int currentDistanceTravelled = 0;

        totalPartitions = xPartitions * yPartitions;
        Sector currentSector = start;
        int layerMask = 1 << 13;

        int check = 0;
        layerMask =
        check = ((currentSector.number + 1) / xPartitions);
        check -= 1;
        check *= xPartitions;
        check += xPartitions;

        while(currentDistanceTravelled <minDist)
        {
            // MonoBehaviour.print(currentSector.number);
            int num = Random.Range(0, 3);
            //MonoBehaviour.print(num);
            switch (num)
            {

                case 0://north
                       //   MonoBehaviour.print("this the math: " + currentSector.number + xPartitions + " < " + totalPartitions);
                    if (currentSector.number + xPartitions < totalPartitions)
                    {
                        RaycastHit hit;

                        if (!Physics2D.Raycast
                            (currentSector.centroid, Vector2.up, yPartitions, layerMask))
                        {
                            MonoBehaviour.print("north");
                            currentSector = sectors[currentSector.number + xPartitions];
                        }
                        if (Physics2D.Raycast
                           (currentSector.centroid, Vector2.up, yPartitions, layerMask))
                        {
                            MonoBehaviour.print("Did Hit");
                        }
                    }
                    break;
                case 1://south
                    if (currentSector.number - xPartitions >= 0)
                    {
                        RaycastHit hit;

                        if (!Physics2D.Raycast
                            (currentSector.centroid, Vector2.down, yPartitions, layerMask))
                        {
                            MonoBehaviour.print("south");
                            currentSector = sectors[currentSector.number - xPartitions];
                        }
                        if (Physics2D.Raycast
                            (currentSector.centroid, Vector2.up, yPartitions, layerMask))
                        {
                            MonoBehaviour.print("Did Hit");
                        }
                    }
                    break;
                case 2://east
                    if (currentSector.number + 1 < xPartitions && currentSector.number != check)
                    {
                        RaycastHit hit;

                        if (!Physics2D.Raycast
                            (currentSector.centroid, Vector2.right, xPartitions, layerMask))
                        {
                            MonoBehaviour.print("east");
                            currentSector = sectors[currentSector.number + 1];
                        }
                        if (Physics2D.Raycast
                           (currentSector.centroid, Vector2.up, yPartitions, layerMask))
                        {
                            MonoBehaviour.print("Did Hit");
                        }
                    }
                    break;
                case 3://west
                    if (currentSector.number - 1 >= 0 && currentSector.number != check)
                    {
                        RaycastHit hit;

                        if (!Physics2D.Raycast
                            (currentSector.centroid, Vector2.left, xPartitions, layerMask))
                        {
                            MonoBehaviour.print("west");
                            currentSector = sectors[currentSector.number - 1];
                        }
                        if (Physics2D.Raycast
                           (currentSector.centroid, Vector2.up, yPartitions, layerMask))
                        {
                            MonoBehaviour.print("Did Hit");
                        }
                    }
                    break;

            }


        }
        return currentSector;

    }
    public bool inSectorBounds(Sector sector, Vector2 obj)
    {
        if ((obj.x <= sector.east)
            && (obj.x > sector.west)
            && (obj.y <= sector.north)
            && (obj.y > sector.south))
            return true;

        else return false;
    }
    public Sector getSectorFromVector(Vector2 v, Sector[] p)
    {
        Sector rsec; 
        foreach(Sector s in p )
        {
            if (s.west <= v.x &&
                s.east >= v.x   &&
                 s.south <= v.y   &&
                 s.north >= v.y
                )                          
            {
            return s; 
            }
            
        }
        return null;
    }
}
