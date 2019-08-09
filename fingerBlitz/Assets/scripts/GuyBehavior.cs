using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuyBehavior : MonoBehaviour
{

    /// <summary>
    /// ////get ready for the new stuff
    /// were talking using this guy with particles for all our crazy needs;
    /// so maybe that means using particles as lasers
    /// but I should run some tests to see if the line renderer is better;
    /// maybe I can just use a few particles as well as the line renderer;
    /// </summary>
    public GameManager gameManager;
    public List<Bullet>bullets;

    public Bullet bulletPrefab;
    public int spread = 0;

   
   
    public float fireRate = 0.5f;
    public float bulletSpeed;
    public int weaponCase=0;

    public GameObject playa;
    //public float gameSpeed=1;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = playa.GetComponent<DragMove>().gameManager;
        playa = GameObject.FindGameObjectWithTag("Player");

        // Random.InitState(2);
       
        bullets = new List<Bullet>();
    }
       
        //StartCoroutine(looknshoot());
    


    // Update is called once per frame
    void Update()
    {
       // gameSpeed = gameManager.gameSpeed;
    }

    public static class WaitFor
    {
        public static IEnumerator Frames(int frameCount)
        {
            while (frameCount > 0)
            {
                frameCount--;
                yield return null;
            }
        }
    }
        public enum FacingDirection
        {
            UP = 270,
            DOWN = 90,
            LEFT = 180,
            RIGHT = 0
        }
    
    public static Quaternion FaceObject(Vector2 startingPosition, Vector2 targetPosition, FacingDirection facing)
    {
        Vector2 direction = targetPosition - startingPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= (float)facing;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void getScreenCoord(Vector2 origin, float angle)
    {
        float x = origin.x;
        float y = origin.y;
        float height = Screen.height;
        float width = Screen.width;

        // get the max diagonal
        float d = Mathf.Sqrt((width / 2) * (width / 2) + (height / 2) * (height / 2));

        //calculate point on circle
        

        //clip the vector from the origin point to each side of the screen
        if(x>width/2)
        {
            float clipFraction = (width / 2) / x;
            x *= clipFraction;
            y *= clipFraction;
        }
        else if (x < -width / 2)
        {
            float clipFraction = (-width / 2) / x; // amount to shorten the vector
            x *= clipFraction;
            y *= clipFraction;
        }
        if (y > height / 2)
        {
            float clipFraction = (height / 2) / y; // amount to shorten the vector
            x *= clipFraction;
            y *= clipFraction;
        }
        else if (y < -height / 2)
        {
            float clipFraction = (-height / 2) / y; // amount to shorten the vector
            x *= clipFraction;
            y *= clipFraction;
        }
        x += width / 2;
        y += height / 2;
    }
}