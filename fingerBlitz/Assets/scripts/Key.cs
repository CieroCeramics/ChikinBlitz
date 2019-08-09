using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    GameManager manager;
    Partitions partitions;//=new Partitions(0,0, Vector2.zero);
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        partitions = manager.gameLayout;
        transform.position = RandomWalk(manager.StartSector).centroid;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Partitions.Sector RandomWalk(Partitions.Sector start)
    {
        int totalPartitions = 25;
        Partitions.Sector currentSector = start;
        int layerMask = 1 << 13;
        //layerMask = ~layerMask;
        int check = 0;
        //layerMask = 
        check = ((currentSector.number + 1) / partitions.xPartitions);
        check -= 1;
        check *= partitions.xPartitions;
        check += partitions.xPartitions;

        for (int i = 0; i < 1000; i++)
        {
            // MonoBehaviour.print(currentSector.number);
            int num =Random.Range(2,3);
            float xLength = partitions.Dimensions.x / partitions.xPartitions;
            float yLength = partitions.Dimensions.y / partitions.yPartitions;
            //print(currentSector.centroid.x + " " + currentSector.centroid.y);
            //MonoBehaviour.print(num);

            switch (num)
            {

                case 0://north
                       //   MonoBehaviour.print("this the math: " + currentSector.number + xPartitions + " < " + totalPartitions);
                    if (currentSector.number + partitions.xPartitions < totalPartitions)
                    {

                        Vector2 dir = partitions.sectors[currentSector.number + partitions.xPartitions].centroid - currentSector.centroid;
                        print("diir" + dir);
                        float dist = Vector2.Distance(partitions.sectors[currentSector.number + partitions.xPartitions].centroid, currentSector.centroid);
                        print("hopefully going to:" + partitions.sectors[currentSector.number + partitions.xPartitions].centroid);

                        RaycastHit2D hit = Physics2D.Raycast(currentSector.centroid, Vector2.up, dist, layerMask);

                        //Debug.DrawRay()
                        if (hit.collider != null)
                        {
                            Vector2 dir2 = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                            Debug.DrawRay(currentSector.centroid, dir2, Color.red, Mathf.Infinity);
                            Debug.DrawRay(currentSector.centroid, Vector2.up, Color.green, Mathf.Infinity);
                            print(hit.collider.tag);
                            print("Did Hit");
                        }

                        else if (hit.collider == null)
                        {
                            MonoBehaviour.print("north");
                            currentSector = partitions.sectors[currentSector.number + partitions.xPartitions];
                        }

                    }
                    break;
                case 1://south
                    if (currentSector.number - partitions.xPartitions >= 0)
                    {
                        float dist = Vector2.Distance(partitions.sectors[currentSector.number - partitions.xPartitions].centroid, currentSector.centroid);
                        RaycastHit2D hit = Physics2D.Raycast(currentSector.centroid, Vector2.down, dist, layerMask);
                        
                        if (hit.collider != null)
                        {
                            Vector2 dir2 = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                            Debug.DrawRay(currentSector.centroid, dir2, Color.red, Mathf.Infinity);
                            Debug.DrawRay(currentSector.centroid, Vector2.down, Color.green, Mathf.Infinity);
                            print(hit.collider.tag);
                            print("Did Hit");
                        }

                        else
                        {
                            MonoBehaviour.print("south");
                            currentSector = partitions.sectors[currentSector.number - partitions.xPartitions];
                        }
                        //if (Physics2D.Raycast
                        //    (currentSector.centroid, Vector2.down,  yLength, layerMask))
                        //{
                        //    MonoBehaviour.print("Did Hit");
                        //}
                    }
                    break;
                case 2://east
                    if (currentSector.number+1<partitions.totalPartitions && (currentSector.number+1) != check)
                    {
                        float dist = Vector2.Distance(partitions.sectors[currentSector.number +1].centroid, currentSector.centroid);
                        RaycastHit2D hit = Physics2D.Raycast(currentSector.centroid, Vector2.right, dist, layerMask);
                        if (hit.collider != null)
                        {
                            Vector2 dir2 = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                            Debug.DrawRay(currentSector.centroid, dir2, Color.red, Mathf.Infinity);
                            Debug.DrawRay(currentSector.centroid, Vector2.right, Color.green, Mathf.Infinity);
                            print(hit.collider.tag);
                            print("Did Hit");
                        }

                        else
                        {
                            MonoBehaviour.print("east");
                            currentSector = partitions.sectors[currentSector.number + 1];
                        }
                        //if (Physics2D.Raycast
                        //   (currentSector.centroid, Vector2.right,  xLength, layerMask))
                        //{
                        //    MonoBehaviour.print("Did Hit");
                        //}
                    }
                    break;
                case 3://west
                    if ( currentSector.number != check)
                    {
                        float dist = Vector2.Distance(partitions.sectors[currentSector.number - 1].centroid, currentSector.centroid);
                        RaycastHit2D hit = Physics2D.Raycast(currentSector.centroid, Vector2.left, dist, layerMask);
                        if (hit.collider != null)
                        {
                            Vector2 dir2 = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                            Debug.DrawRay(currentSector.centroid, dir2, Color.red, Mathf.Infinity);
                            Debug.DrawRay(currentSector.centroid, Vector2.left, Color.green, Mathf.Infinity);
                            print(hit.collider.tag);
                            print("Did Hit");
                        }

                        else
                        {
                            MonoBehaviour.print("west");
                            currentSector = partitions.sectors[currentSector.number - 1];
                        }
                        //if (Physics2D.Raycast
                        //   (currentSector.centroid, Vector2.left,  yLength, layerMask))
                        //{
                        //    MonoBehaviour.print("Did Hit");
                        //}
                    }
                    break;

            }
            //MonoBehaviour.print(currentSector.centroid.x + " " + currentSector.centroid.y);
            if (i == 999 && start.centroid == currentSector.centroid)
            {
                //i -= 1;
                MonoBehaviour.print("same");
            }
        }
        return currentSector;

    }
}
