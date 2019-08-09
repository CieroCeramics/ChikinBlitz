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

    
    public void createPath(int distance, Queue<Partitions.Sector> current)
    {
        int j = 0;
        bool allclear;
        foreach (Partitions.Sector sector in current.ToArray())
        {
            sector.distance = distance;
           // Partitions.Sector[] neighbours = getNeighbours(sector);
            foreach (Partitions.Sector neighbour in getNeighbours(sector))
            {
                allclear = CheckForPath(sector, neighbour);
               
                if(allclear)
                {
                    sectors.Enqueue(neighbour);
                }
                //check if its open
                
                //if it is add it to the queue
            }

            sectors.Dequeue();

        }
        
        if (sectors.Count > 0)
        {
            distance++;
            createPath(distance, sectors);
        }
        manager.gameLayout = partitions;
    }

    List <Partitions.Sector> getNeighbours(Partitions.Sector Base)
    {
        partitions.sectors = manager.gameLayout.sectors;
        int check = 0;
        //layerMask = 
        partitions.xPartitions = Maze.xSize;
        partitions.yPartitions = Maze.ySize;
        check = ((Base.number + 1) / partitions.xPartitions);
        check -= 1;
        check *= partitions.xPartitions;
        check += partitions.xPartitions;
        if (Base.number+partitions.xPartitions<partitions.totalPartitions)
        {
            Base.neighbours.Add(partitions.sectors[Base.number + manager.gameLayout.xPartitions]);
        }
        if (Base.number - partitions.xPartitions >= 0)
        {
            Base.neighbours.Add(partitions.sectors[Base.number - manager.gameLayout.xPartitions]);
        }
        if (Base.number + 1 < partitions.totalPartitions && (Base.number + 1) != check)
        {
            Base.neighbours.Add(partitions.sectors[Base.number + 1]);
        }
        if (Base.number != check)
        {
            Base.neighbours.Add(partitions.sectors[Base.number -1]);
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
        bool visited = neighbour.visited;
        Vector2 dir = currentSector.centroid - neighbour.centroid;
        float dist = Vector2.Distance(currentSector.centroid, neighbour.centroid);

        if (!visited)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentSector.centroid, dir, dist, layerMask);
            return !hit.collider;//didn hit a wall return true
        }
        else { return false; }//has been visisted false
    }
}
