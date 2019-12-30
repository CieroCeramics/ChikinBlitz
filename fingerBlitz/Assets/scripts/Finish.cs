using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{

    public GameManager manager;
    Partitions partitions = new Partitions(Maze.xSize, Maze.ySize,new Vector2(Screen.width,Screen.height));
    public Queue<Partitions.Sector> sectors = new Queue<Partitions.Sector>();
    int minDistance = 5;
    int layerMask = 1 << 13; 
    public bool hasHome;
    int[] values;// = new int[4] { 1, 2, 3, 4 };
    // Start is called before the first frame update
    void Start()
    {
        // values = new int[4]{ currentSector.number + manager.gameLayout.xPartitions }
        //manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        if (GameControl.control.Stage == 1)
        {
            transform.localScale = new Vector2(transform.localScale.x / 1.5f, transform.localScale.y / 1.5f);
            minDistance = 10;
        }
        if (GameControl.control.Stage == 2)
        {
            transform.localScale = new Vector2(transform.localScale.x / 2, transform.localScale.y / 2);
           minDistance = 10;
        }
        partitions = new Partitions(manager.gameLayout.xPartitions, manager.gameLayout.yPartitions, manager.gameLayout.Dimensions);
        partitions = manager.gameLayout;

        
    }

    public void PlaceFinish()
    {
        while (new Vector2(transform.position.x, transform.position.y) == Vector2.zero)
                {
                     manager.CreateStartAndFinish();
            

                }

    }
    public Partitions.Sector createPath(int distance, Queue<Partitions.Sector> current)
    {
        int j = 0;
        bool allclear;
        Partitions.Sector rval =new Partitions.Sector();
        foreach (Partitions.Sector sector in current.ToArray())
        {
            //set the distance value of the sector

            sector.distance = distance;
            // Partitions.Sector[] neighbours = getNeighbours(sector);


            foreach (Partitions.Sector neighbour in getNeighbours(sector))
            {
                //check if the neighbour is blocked by a wall
                allclear = CheckForPath(sector, neighbour);

                //if the path is clear add the neighbour to the queue
                if (allclear)
                {
                    sectors.Enqueue(neighbour);
                }

            }
            //remove the current sector from the queue
            if (sectors.Count != 0)
            {
                //set it as our return value
                rval = sectors.Peek();

                sectors.Dequeue().visited = true;
            }

        }

        //make sure the queue isnt empty
        if (sectors.Count > 0 && distance<minDistance)
        {
            //increment the number steps we have taken from the beginning
            distance++;
           
               rval= createPath(distance, sectors);
               
            
           
 //sectors.Peek().inhabitants.Add(this.gameObject);
            //recursively check the next place in the queue
              // hasHome=true;
            return rval;// sectors.Peek().centroid;

        }
        else if (distance>=minDistance)
        {
            rval.partOfPath =true;
            
            return rval;
        }
        else if (sectors.Count==0&&distance<minDistance)
        {
            manager.gameLayout = partitions;
            return null;
        }
        else
        {
            return null;
        }
        manager.gameLayout = partitions;
        
        //set the main partitions to have all the new things we assigned in this algorithm
        
    }
    public void ResetPartitions(Partitions.Sector[] slist)
    {
        foreach (Partitions.Sector s in slist)
        {
            s.visited = false;
            s.neighbours.Clear();
            s.inhabitants.Clear();

            
        }
    }
    List <Partitions.Sector> getNeighbours(Partitions.Sector Base)
    {
        //set the partitions to be exactly what it is in the game
        partitions.sectors = manager.gameLayout.sectors;

        
        
       
        //set the partitions to be the size of the maze.
        partitions.xPartitions = Maze.xSize;
        partitions.yPartitions = Maze.ySize;
        partitions.totalPartitions = Maze.xSize * Maze.ySize;
        //do some wild math for border detection
        int check = 0;
        check = ((Base.number + 1) / partitions.xPartitions);
        check -= 1;
        check *= partitions.xPartitions;
        check += partitions.xPartitions;

        //check north neighbour
        if (Base.number+partitions.xPartitions<partitions.totalPartitions)
        {
            Vector2 dir = partitions.sectors[Base.number + manager.gameLayout.xPartitions].centroid - Base.centroid;
            float dist = Vector2.Distance(partitions.sectors[Base.number + manager.gameLayout.xPartitions].centroid, Base.centroid);
            RaycastHit2D hit = Physics2D.Raycast(Base.centroid, dir, 2, layerMask);
            
            if (hit.collider == null)
            {
                Base.neighbours.Add(partitions.sectors[Base.number + partitions.xPartitions]);
            }
            else
            {
               //  print("hit " +hit.collider.tag + " at:" + hit.point+" from Sector" + Base.number +" at:" + Base.centroid+" aiming north");
                Vector2 dir2 = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                Debug.DrawRay(Base.centroid, dir2, Color.red, Mathf.Infinity);
                Debug.DrawRay(Base.centroid, dir, Color.green, Mathf.Infinity);

            }
        }
        //check south neighbour
        if (Base.number - partitions.xPartitions >= 0)
        {

            Vector2 dir = partitions.sectors[Base.number - partitions.xPartitions].centroid - Base.centroid;
            float dist = Vector2.Distance(partitions.sectors[Base.number - manager.gameLayout.xPartitions].centroid, Base.centroid);
            RaycastHit2D hit = Physics2D.Raycast(Base.centroid, dir, dist, layerMask) ;

            if (hit.collider == null)
            {
                Base.neighbours.Add(partitions.sectors[Base.number - manager.gameLayout.xPartitions]);
            }
            else
            {
              //  print("hit " + hit.collider.tag + " at:" + hit.point + " from Sector" + Base.number + " at:" + Base.centroid + "aiming south");
                Vector2 dir2 = hit.point - Base.centroid;//hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                Debug.DrawRay(Base.centroid, dir2, Color.red, Mathf.Infinity);
                Debug.DrawRay(Base.centroid, dir, Color.green, Mathf.Infinity);
            }
        }
        //check east neighbour
        if (Base.number + 1 < partitions.totalPartitions && (Base.number + 1) != check)
        {
            Vector2 dir = partitions.sectors[Base.number+1].centroid - Base.centroid;

            float dist = Vector2.Distance(partitions.sectors[Base.number +1].centroid, Base.centroid);
            RaycastHit2D hit = Physics2D.Raycast(Base.centroid, dir, dist, layerMask);
            //Ray2D ray = new Ray2D(Base.centroid, dir);

            //if(Physics2D.Raycast (Base.centroi)           {

            //}
            if (hit.collider == null)
            {
                Base.neighbours.Add(partitions.sectors[Base.number + 1]);
            }
            else
            {
             //   print("hit " + hit.collider.tag + " at:" + hit.point + " from Sector" + Base.number + " at:" + Base.centroid + "aiming east");                //return false;//hit a collider? yes? then return false, no? then return true.
                Vector2 dir2 = hit.point - Base.centroid;// hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                Debug.DrawRay(Base.centroid, dir2, Color.red, Mathf.Infinity);
               // Debug.DrawRay(Base.centroid, dir , Color.green, Mathf.Infinity);
            }
        }
        //check west neighbour
        if (Base.number-1>= 0 &&Base.number != check)
        {
            Vector2 dir = partitions.sectors[Base.number -1].centroid - Base.centroid;
            float dist = Vector2.Distance(partitions.sectors[Base.number - 1].centroid, Base.centroid);
            RaycastHit2D hit = Physics2D.Raycast(Base.centroid, dir, dist, layerMask);

            if (hit.collider == null)
            {
                Base.neighbours.Add(partitions.sectors[Base.number - 1]);

            }
            else
            {
              //  print("hit " + hit.collider.tag + " at:" + hit.point + " from Sector" + Base.number + " at:" + Base.centroid + "aiming west");
                //return false;//hit a collider? yes? then return false, no? then return true.
                Vector2 dir2 = hit.point - Base.centroid;// hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                Debug.DrawRay(Base.centroid, dir2, Color.red, Mathf.Infinity);
                Debug.DrawRay(Base.centroid, Vector2.left * dist, Color.green, Mathf.Infinity);
                //return false;//hit a collider? yes? then return false, no? then return true.
            }
        }

        return Base.neighbours;
    }
    int[] getpoints(int source)
    {
        int[] p = new int[4] {
            source + manager.gameLayout.xPartitions,
            source - manager.gameLayout.xPartitions,
            source +1,
            source -1};
        return p;
    }

    bool CheckForPath(Partitions.Sector currentSector, Partitions.Sector neighbour)
    {
        // assign visited to true if we have visited the neighbour
        bool visited = neighbour.visited;
        //get the direction vector and distance from the current partition to the neighbour
        Vector2 dir = neighbour.centroid- currentSector.centroid.normalized;  
        

        //if the neighbour hasn't been visited, cast a ray to see if it's traverasable
        if (!visited)
        {
            return true;
        }
        else { return false; }//has been visisted false
    }
}
