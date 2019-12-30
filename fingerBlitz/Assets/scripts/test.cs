using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
 public Partitions.Sector StartSector = new Partitions.Sector();
    public GameObject start;
    public GameObject end;
	 public enum LevelState
    {
        LOADING, LOADED
    };
      LevelState levelstate = LevelState.LOADING;
     public Partitions gameLayout;
    // Start is called before the first frame update
    void Start()
    {
    	 Vector2 screenSize;
        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)));// * 0.5; //Grab the world-space position values of the start and end positions of the screen, then calculate the distance between them and store it as half, since we only need half that value for distance away from the camera to the edge
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)));// * 0.5f;

    	gameLayout = new Partitions(Maze.xSize, Maze.ySize, screenSize);
        gameLayout.createSectors();
         StartCoroutine(Loadlevel());
    }

    // Update is called once per frame
    void Update()
    {
        
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
IEnumerator Loadlevel()
    {
        while (levelstate == LevelState.LOADING)
        {
        	if(Input.GetMouseButtonDown(0))
         {
         	print("hi");
            if (new Vector2(end.transform.position.x, end.transform.position.y) == Vector2.zero)
            {
                //end.GetComponent<Finish>().sectors.Enqueue(StartSector);
                CreateStartAndFinish();
               
                yield return new WaitForSeconds(1f);
            }
            else
            { 
              
                levelstate = LevelState.LOADED;
                yield return null;
            }
        }
        yield return null;
        }
         
    }

 public void CreateStartAndFinish()
    {
    	 end.GetComponent<Finish>().ResetPartitions(gameLayout.sectors);
        StartSector = checknplace(start);
       
        end.GetComponent<Finish>().sectors.Enqueue(StartSector);
        
        start.transform.position = StartSector.centroid;
      // end.transform.position = end.GetComponent<Finish>().createPath(0, end.GetComponent<Finish>().sectors);
        
       // player.transform.position = start.transform.position;
       

    }
}
