using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
public class DragMove : MonoBehaviour, IPointerDownHandler
{

    public GameManager gameManager;
    public GameObject thumbStop;
    
    //public bool firsttouch = false;
    private Vector3 touchPosition;
    private Rigidbody2D rb;
    private Vector2 direction;
    private float moveSpeed = 20f;
    private LineRenderer lineRenderer;

    Queue<Vector2> Path = new Queue<Vector2>();
    bool zoomz = false;
    public float gameSpeed;
    Vector2 StoredPosition;
    void Start()
    {
      
        //gameSpeed = gameManager.gameSpeed;
        StoredPosition = transform.position;
        Time.timeScale = 1f;
        
        lineRenderer = GetComponent<LineRenderer>();
        //firsttouch = false;
        addPhysics2DRaycaster();
        rb = GetComponent<Rigidbody2D>();
        
        ///Physics2D.IgnoreRaycastLayer()
    }
   
    public bool playerclicked = false;
    bool slowdown = true;
    private void LateUpdate()
    {
        
    }
    private void FixedUpdate()
    {
        float speed = Vector2.Distance(StoredPosition,transform.position) / Time.deltaTime*100000000f;
        
        thumbStop.transform.position = transform.position;
        if ((Input.touchCount > 0) || (Input.GetMouseButtonDown(1)))
        {
            GameManager.gameSpeed = 1;
            StopCoroutine(SlowDown());
            Touch touch = Input.GetTouch(0);
            if(touch.phase==TouchPhase.Began)
            {
                StartCoroutine(TraceFinger());
            }
            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;
            Vector2 touchPos = new Vector2(touchPosition.x, touchPosition.y);
            GetComponent<Animator>().SetFloat("Speed", 1);
          //  print("speed " + speed + " distance " + Vector2.Distance(transform.position, StoredPosition));

            if (zoomz)
            {
                StartCoroutine(ZoomTo());
                zoomz = false;
            }
            if (playerclicked)
            {
                slowdown = false;
                
                
                StopCoroutine(ZoomTo());
                //firsttouch = true;
               
                 
                //;lgameManager.maxGameSpeed;
                if (Path.Count > 0)
                {

                    direction = (Path.Peek() - new Vector2(transform.position.x, transform.position.y));
                

                    if (direction.x > 0)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (direction.x <= 0)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }

                Vector2 smoothLerp = Vector2.Lerp(transform.position, Path.Peek(), moveSpeed * Time.deltaTime);
                transform.position = smoothLerp;
                //transform.Translate(new Vector2(direction.x, direction.y) * moveSpeed);
                if(Vector2.Distance(transform.position,Path.Peek())<=0.1f)
                {
                    Path.Dequeue();
                }
                //if (Vector2.Distance(Path.Peek(),touchPosition)>0.01)
                //    {
                //        Path.Enqueue(touchPosition);
                //    }
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, touchPos);
            }
                }
            if (touch.phase == TouchPhase.Ended)
            {
                slowdown = true;
                up = down = left = right = false;
                //print(gameSpeed);
                playerclicked = false;
                StartCoroutine(SlowDown());
                GetComponent<Animator>().SetFloat("Speed", 0);
                GetComponentInChildren<ParticleSystem>().Play();
                Path.Clear();
               // StopCoroutine(TraceFinger());
            }
            // rb.velocity = Vector2.zero;
            StoredPosition = transform.position;
            
        }


         
    }
    IEnumerator TraceFinger()
    {
        while (true)
        {
            if (playerclicked)
            {

                Path.Enqueue(touchPosition);
                yield return new WaitForFixedUpdate();
            }
            yield return null;
        }
    }

    IEnumerator ZoomTo()
    {
        while (true)
        {
            direction = (touchPosition - transform.position);
            transform.Translate(new Vector2(direction.x, direction.y) * moveSpeed * 2);
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator SlowDown()
    {
        
        while (slowdown)
        {
            //  print(gameSpeed);
            //EasingFunction ease;
            float v = 0.5f;
            if (GameManager.gameSpeed > 0)
            {
                GameManager.gameSpeed *= (-Mathf.Pow(2, -10 * v) + 1);
                // gameSpeed -= 0.05f;
                yield return new WaitForFixedUpdate();
            }
            if(GameManager.gameSpeed<0.001f)
            {
                GameManager.gameSpeed = 0.0001f;
            }

            else { yield return null; }
        }
    }
    bool up, down, left, right;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (up) { transform.Translate(new Vector2(0, 1) * moveSpeed); }
        if (down) { transform.Translate(new Vector2(0, -1) * moveSpeed); }
        if (left) { transform.Translate(new Vector2(-1, 0) * moveSpeed); }
        if (right) { transform.Translate(new Vector2(1, 0) * moveSpeed); }
        if (eventData.pointerCurrentRaycast.gameObject.name == "trysmt")
        {
            playerclicked = true;
            GetComponentInChildren<ParticleSystem>().Stop();
        }
        else playerclicked = false;
        //  Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

    Vector2 normalizeToDirection(Vector2 inputDir, Collider2D WallBounds)
    {
        Vector2 backwards = -inputDir;
        if ((WallBounds.bounds.extents.x > WallBounds.bounds.extents.y) && (WallBounds.transform.position.y > inputDir.y))
        {
            return new Vector2(0, -1);//south
        }
        if ((WallBounds.bounds.extents.x > WallBounds.bounds.extents.y) && (WallBounds.transform.position.y < inputDir.y))
        {
            return new Vector2(0, 1);//north
        }
        if ((WallBounds.bounds.extents.x < WallBounds.bounds.extents.y) && (WallBounds.transform.position.x > inputDir.x))
        {
            return new Vector2(-1, 0);//west
        }
        if ((WallBounds.bounds.extents.x < WallBounds.bounds.extents.y) && (WallBounds.transform.position.x < inputDir.x))
        {
            return new Vector2(1, 0);//east
        }
        return new Vector2(0, 0);
    }

    void addPhysics2DRaycaster()
    {
        Physics2DRaycaster physicsRaycaster = FindObjectOfType<Physics2DRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }

    


    public void Zoom()
    {
        if (GameControl.control.zooms > 0)
        {
            zoomz = true;
            GameControl.control.zooms--;
        }

    }

    public void Fly()
    {
        if (GameControl.control.flys > 0)
        {
            print("fkying");
            //int lasermask = LayerMask.NameToLayer("Laser");
            //  int wallLayer = LayerMask.NameToLayer("Ignore Raycast");
            //  int layerMask = ~((1 << lasermask) | (1 << wallLayer));
            gameObject.layer = LayerMask.NameToLayer("FlyingPlayer");
            GameControl.control.flys--;
        }
    }


    public void goUP()
    {
        up = true;down = false;
    }
    public void goDOWN()
    {
        down = true;up = false;
    }
    public void goLeft()
    {
        left = true;right = false;
    }
    public void goRight()
    {
        right = true;left = false;
    }
    bool isdamaged = false;
    public void OnCollisionEnter2D(Collision2D other)
    {
        //Physics2D.IgnoreCollision(thumbStop.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        if (other.gameObject.tag == "key")
        {
            GameControl.control.keys++;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "timeStop")
        {
            GameControl.control.times++;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "life")
        {
            GameControl.control.lives++;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "zoom")
        {
            GameControl.control.zooms++;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "fly")
        {
            GameControl.control.flys++;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "lock")
        {

            if (GameControl.control.keys > 0)
            {
                Destroy(other.gameObject.transform.parent.gameObject);
            }


        }
        if (other.gameObject.tag == "wall")
        {
            
                playerclicked = false;
                transform.Translate(normalizeToDirection(transform.position, other.collider) * 0.2f);
       

        }
  
        if (other.gameObject.tag == ("Finish"))
        {
            // print("hit");
            //Time.timeScale = 0f;
            //StartCoroutine(gameManager.screenPulse());
           
            gameManager.gobackyn = false;
            if (other.contacts[0].otherCollider.transform.gameObject.name == "trysmt")
            {
                Physics2D.IgnoreCollision(thumbStop.GetComponentInChildren<Collider2D>(), other.gameObject.GetComponent<Collider2D>());

                gameManager.Finished("alive", gameManager.Win, gameManager.nextLevl);
            }
            
        }


        if (other.gameObject.tag == "laser" || other.gameObject.tag == "spikes")
        {

            if (other.contacts[0].otherCollider.transform.gameObject.name == "trysmt")
            {


                Physics2D.IgnoreCollision(thumbStop.GetComponentInChildren<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
            //    print("Frantic");
                GetComponent<Animator>().SetBool("Frantic", true);


            }
            else 
            {
                playerclicked = false;
                if (!isdamaged)
                {
                   
                    isdamaged = true;
                    GameManager.gameSpeed = 0;
                    //Time.timeScale = 0f;
                    GetComponent<Animator>().SetBool("Dead", true);
                    
                    // StartCoroutine(gameManager.screenShake());
                    
                    if (GameControl.control.lives <= 0)
                    {

                        GameControl.control.LevelNumber = 0;
                       // Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
                        Invoke("dotheInvoke", 1);

                        GameControl.control.lives = 3;
                    }
                    else
                    {
                        Invoke("dotheInvoke2", 1);
                        GameControl.control.lives--;
                        
                        //Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
                        gameManager.gobackyn = true;
                        
                       

                        // reloadLevel();
                    }
                }
            }
        }
        else { GetComponent<Animator>().SetBool("Frantic", false); }
    }
    public void dotheInvoke()
    {
        gameManager.Finished("GO", gameManager.GameOver, null);
    }
    void dotheInvoke2()
    {
        gameManager.Finished("dead", gameManager.Lose, gameManager.restart);
    }
    public void OnCollisionExit2D(Collision2D other)
    {

        //if (other.contacts[0].otherCollider.transform.gameObject.name == "trysmt")
        //{
         //   print("not Frantic");
            GetComponent<Animator>().SetBool("Frantic", false);
            isdamaged = false;
        //}
    }
        }

