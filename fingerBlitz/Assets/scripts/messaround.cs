using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class messaround : MonoBehaviour
{
    private Partitions gameLayout;
    Vector2 sptw,vptw;
    // Start is called before the first frame update
    void Start()
    {
       // gameLayout = new Partitions();
        //gameLayout.createSectors();
    }
    void debugMaze()
    {
        sptw = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        vptw = Camera.main.ScreenToViewportPoint(new Vector2(Screen.width, Screen.height));
        print("Screem Dimensions: " + Screen.width + ", " + Screen.height);
        print("world Dimensions" + sptw.x + ", " + sptw.y);
        print("View Dimensions" + vptw.x + ", " + vptw.y);
        Bounds bounds = GetComponent<SpriteRenderer>().sprite.bounds;
        float stretchToWorldScale = bounds.size.y;
        transform.localScale = new Vector3(1, (sptw.y * 2 / stretchToWorldScale), 1);
    }
    // Update is called once per frame
    void Update()
    {
        //transform.position =new Vector3(sptw.x,sptw.y, 0f);
    }
}
