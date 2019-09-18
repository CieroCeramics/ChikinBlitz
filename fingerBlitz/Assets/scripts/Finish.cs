using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{

    public GameManager manager;
    Partitions partitions = new Partitions(Maze.xSize, Maze.ySize,new Vector2(Screen.width,Screen.height));
    public Queue<Partitions.Sector> sectors = new Queue<Partitions.Sector>();
    int minDistance = 3;
    int layerMask = 1 << 13;
    int[] values;// = new int[4] { 1, 2, 3, 4 };
    // Start is called before the first frame update
    void Start()
    {
        // values = new int[4]{ currentSector.number + manager.gameLayout.xPartitions }
        //manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        partitions = new Partitions(manager.gameLayout.xPartitions, manager.gameLayout.yPartitions, manager.gameLayout.Dimensions);
        partitions = manager.gameLayout;

        
    }

    public void PlaceFinish()
    {
        transform.position = createPath(0, sectors);
    }
    public Vector2 createPath(int distance, Queue<Partitions.Sector> current)
    {
        int j = 0;
        bool allclear;

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
            sectors.Dequeue().visited = true;

        }

        //make sure the queue isnt empty
        if (sectors.Count > 0)
        {
            //increment the number steps we have taken from the beginning
            distance++;
            if (distance > minDistance)
            {
                manager.gameLayout = partitions;
                return sectors.Peek().centroid;
            }
            else { createPath(distance, sectors); }

            //recursively check the next place in the queue
            return sectors.Peek().centroid;
        }
        else
        {
            return Vector2.zero;
                }
        manager.gameLayout = partitions;
        
        //set the main partitions to have all the new things we assigned in this algorithm
        
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
           
            float dist = Vector2.Distance(partitions.sectors[Base.number + manager.gameLayout.xPartitions].centroid, Base.centroid);
            RaycastHit2D hit = Physics2D.Raycast(Base.centroid, Vector2.up, 2, layerMask);

            if (hit.collider == null)
            {
                Base.neighbours.Add(partitions.sectors[Base.number + manager.gameLayout.xPartitions]);
            }
            else
            {
                print("hit " +hit.collider.tag +"from Sector"+ Base.number +"aiming north");
                Vector2 dir2 = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                Debug.DrawRay(Base.centroid, dir2, Color.red);
                Debug.DrawRay(Base.centroid, Vector2.up * dist, Color.green, Mathf.Infinity);

            }
        }
        //check south neighbour
        if (Base.number - partitions.xPartitions >= 0)
        {
           

            float dist = Vector2.Distance(partitions.sectors[Base.number - manager.gameLayout.xPartitions].centroid, Base.centroid);
            RaycastHit2D hit = Physics2D.Raycast(Base.centroid, Vector2.down, 2, layerMask);

            if (hit.collider == null)
            {
                Base.neighbours.Add(partitions.sectors[Base.number - manager.gameLayout.xPartitions]);
            }
            else
            {
                print("hit " + hit.collider.tag + "from Sector" + Base.number + "aiming south");
                Vector2 dir2 = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                Debug.DrawRay(Base.centroid, dir2, Color.red, Mathf.Infinity);
                Debug.DrawRay(Base.centroid, Vector2.down*dist, Color.green, Mathf.Infinity);
            }
        }
        //check east neighbour
        if (Base.number + 1 < partitions.totalPartitions && (Base.number + 1) != check)
        {


            float dist = Vector2.Distance(partitions.sectors[Base.number +1].centroid, Base.centroid);
            RaycastHit2D hit = Physics2D.Raycast(Base.centroid, Vector2.right, 2, layerMask);

            if (hit.collider == null)
            {
                Base.neighbours.Add(partitions.sectors[Base.number + 1]);
            }
            else
            {
                print("hit " + hit.collider.tag + "from Sector" + Base.number + "aiming east");                //return false;//hit a collider? yes? then return false, no? then return true.
                Vector2 dir2 = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                Debug.DrawRay(Base.centroid, dir2, Color.red, Mathf.Infinity);
                Debug.DrawRay(Base.centroid, Vector2.right * dist, Color.green, Mathf.Infinity);
            }
        }
        //check west neighbour
        if (Base.number-1>= 0 &&Base.number != check)
        {
         
            float dist = Vector2.Distance(partitions.sectors[Base.number - 1].centroid, Base.centroid);
            RaycastHit2D hit = Physics2D.Raycast(Base.centroid, Vector2.left, 2, layerMask);

            if (hit.collider == null)
            {
                Base.neighbours.Add(partitions.sectors[Base.number - 1]);

            }
            else
            {
                print("hit " + hit.collider.tag + "from Sector" + Base.number + "aiming west");
                //return false;//hit a collider? yes? then return false, no? then return true.
                Vector2 dir2 = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
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
