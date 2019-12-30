using UnityEngine;
using UnityEngine.UI;
public class CameraFollow : MonoBehaviour
{
    private Camera cam;
    public Transform target;
    public Slider slider;
    public GameManager gameManager;
    public float smoothSpeed = 20f;
    public Vector3 offset;
    DragMove playr;
    float k;
    // Update is called once per frame
    private void Start()
    {
        playr = target.GetComponent<DragMove>();
        offset = new Vector3(0, 0, -10);
        cam = GetComponent<Camera>();
        //slider.onValueChanged.AddListener(sliderCallBack);
    }

    Vector3 lerpDest()
    {
        Vector3 retrnVector = new Vector3(playr.curSec.centroid.x, playr.curSec.centroid.y, -10);
 

            int total = gameManager.gameLayout.xPartitions*gameManager.gameLayout.yPartitions;
            int xpartitions = gameManager.gameLayout.xPartitions;
            int check = 0;
            check = ((playr.curSec.number + 1) / gameManager.gameLayout.xPartitions);
            check -= 1;
            check *= xpartitions;
            check += xpartitions;
            //   float dist = Vector2.Distance(gameManager.gameLayout.sectors[playr.curSec.number + gameManager.gameLayout.xPartitions].centroid, gameManager.gameLayout.sectors[playr.curSec.number].centroid);
           
            float d = (playr.curSec.north - playr.curSec.south) / 4;
        //north
        k = 0;
            if (playr.curSec.north - target.position.y < d && playr.curSec.number + xpartitions < total)
            {
                retrnVector = gameManager.gameLayout.sectors[playr.curSec.number + gameManager.gameLayout.xPartitions].centroid; //new Vector3(playr.curSec.centroid.x, playr.curSec.centroid.y+ylen, -10);
                k = ((playr.curSec.north + d)-target.position.y) / (d + d);
            }
            //south
            if (target.position.y - playr.curSec.south < d && playr.curSec.number - xpartitions >= 0)
            {
                retrnVector = gameManager.gameLayout.sectors[playr.curSec.number - gameManager.gameLayout.xPartitions].centroid;
                k = (target.position.y - (playr.curSec.south - d)) / (d + d);
            }
            //east
            if (playr.curSec.east - target.position.x < d && playr.curSec.number + 1 < total && (playr.curSec.number + 1) != check)
            {
                retrnVector = gameManager.gameLayout.sectors[playr.curSec.number + 1].centroid; //new Vector3(playr.curSec.centroid.x, playr.curSec.centroid.y+ylen, -10);
                k = ((playr.curSec.east + d)-target.position.x) / (d + d);
            }
            //west
            if (target.position.x - playr.curSec.west < d && playr.curSec.number != check)
            {
                retrnVector = gameManager.gameLayout.sectors[playr.curSec.number - 1].centroid;
                k = (target.position.x - (playr.curSec.west - d)) / (d + d);
            }
        
        return new Vector3 (retrnVector.x,retrnVector.y,-10);
    }

    void LateUpdate()
    {
     
        Vector3 start = new Vector3(0, 0, -10);


        // k =(( k * -1) +1);


        Vector3 dest = Vector3.Lerp(new Vector3(playr.curSec.centroid.x,playr.curSec.centroid.y,-10), lerpDest(), Easing.Quadratic.InOut((k*-1)+1));//new Vector3(playr.curSec.centroid.x, playr.curSec.centroid.y);
        cam.orthographicSize =  5/(slider.value+1);
        transform.position = Vector3.Lerp(start, dest, slider.value);
    }

    //void sliderCallBack(float value)
    //{
    //    Vector3 start = new Vector3(0,0,-10);
    //    Vector3 dest = new Vector3(target.transform.position.x, target.transform.position.y, -10);
    //    cam.orthographicSize = Mathf.Clamp(value * 5, ;
    //    transform.position = Vector3.Lerp(start, dest, (value*-1)+1);
    //    // GameControl.control.circleSize = value;

    //}
}
