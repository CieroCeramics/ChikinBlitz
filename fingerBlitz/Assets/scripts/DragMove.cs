using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
public class DragMove : MonoBehaviour//, IPointerDownHandler
{
    public GameObject radial;
    public GameManager gameManager;
    public GameObject thumbStop;
    public SpriteRenderer FullSreenEffect;
    public GameObject linearprojection;
    //public bool firsttouch = false;
    private Vector3 touchPosition;
    private Rigidbody2D rb;
    private Vector2 direction;
    private float moveSpeed; //= 20f;
    private LineRenderer lineRenderer;
   
    Queue<Vector2> Path = new Queue<Vector2>();
    bool zoomz = false;
    bool flyin = false;
    public float gameSpeed;
    Vector2 StoredPosition;
    public Animator animator;
    private TrailRenderer trailrenderer;
    int stack = 0;
    void Start()
    {
       
        trailrenderer = GetComponent<TrailRenderer>();
        radial.SetActive(false);
        animator = GetComponent<Animator>();
        //gameSpeed = gameManager.gameSpeed;
        StoredPosition = transform.position;
        Time.timeScale = 1f;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        //firsttouch = false;
        addPhysics2DRaycaster();
        rb = GetComponent<Rigidbody2D>();

        ///Physics2D.IgnoreRaycastLayer()
    }

    public bool playerclicked = false;
    bool slowdown = false;
    private void Update()
    {

    }
    Vector2 mouseRelativeOrigin;
    int c = 0;
    Vector2 tempOrigin;
    private void FixedUpdate()
    {

        //Time.timeScale
        Vector2 dir;
        float speed = Vector2.Distance(StoredPosition, transform.position) / Time.deltaTime * 100000000f;

        thumbStop.transform.position = transform.position;
        //if (Input.GetMouseButtonDown(0))
        //{
        //    radial.SetActive(true);
        //    radial.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        //    playerclicked = true;
        //    // tempOrigin = Input.mousePosition;
        //    mouseRelativeOrigin = Input.mousePosition;
        //    // print("click");
        //    Path.Enqueue(Input.mousePosition);
        //    lineRenderer.SetPosition(0, new Vector2(Camera.main.ScreenToWorldPoint(mouseRelativeOrigin).x, Camera.main.ScreenToWorldPoint(mouseRelativeOrigin).y));
        //}
        //if (Input.GetMouseButton(0))
        //{
        //    // print("clickING!!!!!");

        //    c++;
        //    if (c > 5)
        //    {
        //        if (Vector2.Distance(mouseRelativeOrigin, Input.mousePosition) > 250)
        //        {
        //            Path.Enqueue(tempOrigin);
        //            //if (Path.Count > 1)
        //            //{
        //                mouseRelativeOrigin = Input.mousePosition;//Path.Dequeue();
        //                radial.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y,0);
        //                //radial.transform.position.z = 0;
        //                lineRenderer.SetPosition(0, new Vector2(Camera.main.ScreenToWorldPoint(mouseRelativeOrigin).x, Camera.main.ScreenToWorldPoint(mouseRelativeOrigin).y));
        //            //}
        //            c = 0;
        //        }
        //    }
        //    if (!gameManager.itsoverman)
        //    {
        //        GameManager.gameSpeed = 1;
        //    }
        //    StopCoroutine(SlowDown());
        //    //mouseRelativeOrigin = Input.mousePosition;
        //    // origin = Input.mousePosition;
        //    moveSpeed = Vector2.Distance(mouseRelativeOrigin, Input.mousePosition);
        //    dir = (new Vector2(Input.mousePosition.x, Input.mousePosition.y) - mouseRelativeOrigin).normalized;
        //    lineRenderer.SetPosition(1, new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
        //    transform.Translate(dir * Time.deltaTime * moveSpeed*0.008f);

        //    tempOrigin = Input.mousePosition;
        //}

        //if (Input.GetMouseButtonUp(0))
        //{
        //    radial.SetActive(false);
        //    Path.Clear();
        //    c = 0;
        //    slowdown = true;
        //    StartCoroutine(SlowDown());

        //}
        if (Input.touchCount > 0 && !zoomz && !IsPointerOverUIObject())
        {
            if (!gameManager.itsoverman)
            {
                GameManager.gameSpeed = gameManager.maxGameSpeed;
            }

            if (flyin)
            {
                print(flytime);
                flytime--;
                float mft = stack * 100;
                FullSreenEffect.color -= new Color(0f, 0f, 0f, 0.5f / (mft * 2));
                if (flytime == 0)
                {
                    gameObject.layer = LayerMask.NameToLayer("player");
                    flyin = false;
                    stack = 0;
                    FullSreenEffect.gameObject.SetActive(false);
                }


            }


            StopCoroutine(SlowDown());
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                mouseRelativeOrigin = touch.position;
                radial.SetActive(true);
                radial.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
                lineRenderer.SetPosition(0, new Vector2(Camera.main.ScreenToWorldPoint(mouseRelativeOrigin).x, Camera.main.ScreenToWorldPoint(mouseRelativeOrigin).y));
                lineRenderer.startColor = Color.black;
                lineRenderer.startWidth = 0.012f;
                playerclicked = true;
                //StartCoroutine(TraceFinger());
                
               
                //    print("click");
                //Path.Enqueue(touch.position);
            }

            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;
            Vector2 touchPos = new Vector2(touchPosition.x, touchPosition.y);
            animator.SetFloat("Speed", 1);
            //  print("speed " + speed + " distance " + Vector2.Distance(transform.position, StoredPosition));


            if (playerclicked)
            {
                slowdown = false;


                StopCoroutine(ZoomTo());
                lineRenderer.enabled = true;
                trailrenderer.enabled = false;
                c++;
                Vector2 MRE = new Vector2(Camera.main.ScreenToWorldPoint(mouseRelativeOrigin).x, Camera.main.ScreenToWorldPoint(mouseRelativeOrigin).y);
                dir = (touchPos - MRE).normalized;
                if (c > 2)
                {
                    if (Vector2.Distance(mouseRelativeOrigin, touch.position) > 250)
                    {
                        //Path.Enqueue(touch.position);
                        // if (Path.Count > 1)
                        // {
                        mouseRelativeOrigin =new Vector2(Camera.main.WorldToScreenPoint(radial.transform.position).x,Camera.main.WorldToScreenPoint(radial.transform.position).y);// Path.Dequeue();
                        radial.transform.Translate(dir*Time.deltaTime* Vector2.Distance(mouseRelativeOrigin, touch.position)*0.05f);
                        lineRenderer.SetPosition(0, radial.transform.position );
                        //  }
                        c = 0;
                    }
                }

               
                moveSpeed =Mathf.Clamp(Vector2.Distance(mouseRelativeOrigin, touch.position),0,250);
                //  print(moveSpeed);
                lineRenderer.SetPosition(1, touchPosition);
                transform.Translate(dir * Time.deltaTime * moveSpeed * 0.008f);

                tempOrigin = touch.position;
                //firsttouch = true;


                //;lgameManager.maxGameSpeed;
                //    if (Path.Count > 0)
                //    {

                direction = (new Vector2(touch.position.x, touch.position.y) - mouseRelativeOrigin);


                if (direction.x > 0)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (direction.x <= 0)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }

                //    Vector2 smoothLerp = Vector2.Lerp(transform.position, Path.Peek(), moveSpeed * Time.deltaTime);
                //    transform.position = smoothLerp;
                //    //transform.Translate(new Vector2(direction.x, direction.y) * moveSpeed);
                //    if(Vector2.Distance(transform.position,Path.Peek())<=0.1f)
                //    {
                //        Path.Dequeue();
                //    }
                //    //if (Vector2.Distance(Path.Peek(),touchPosition)>0.01)
                //    //    {
                //    //        Path.Enqueue(touchPosition);
                //    //    }
                //    lineRenderer.SetPosition(0, transform.position);
                //    lineRenderer.SetPosition(1, touchPos);
                //}
            }
            if (touch.phase == TouchPhase.Ended)
            {
                slowdown = true;
                //up = down = left = right = false;
                //print(gameSpeed);
                playerclicked = false;
                StartCoroutine(SlowDown());
                GetComponent<Animator>().SetFloat("Speed", 0);
                //GetComponentInChildren<ParticleSystem>().Play();
                Path.Clear();
                lineRenderer.enabled = false;
                c = 0;
                // StopCoroutine(TraceFinger());
            }
            // rb.velocity = Vector2.zero;
            // StoredPosition = transform.position;

        }



    }

    public struct lerpInfo
    {
        float start;
        float length;

    }

    float startTime = 0;
    int count = 0;
    float journeyLength = 0;
    Queue<Vector2> zoomqueue = new Queue<Vector2>();
    bool start = false;
    int layerMask = 1 << 13;
    Vector2 oldtouch;
    bool clearPath = false;
    Recycling.RecycleBin linebin = new Recycling.RecycleBin();
  

    GameObject curLine;
    IEnumerator ZoomTo()
    {

        while (zoomz)
        {
     
            
            print(start);
            if (FullSreenEffect.color.a < 0.5f)
            {
                FullSreenEffect.color += new Color(0, 0, 0, 0.05f);
            }
     
            if (!IsPointerOverUIObject() && Input.touchCount > 0)
            {
                
                print( "not touching UI ");
                Touch touch = Input.GetTouch(0);
                touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                Vector2 touchPos = new Vector2(touchPosition.x, touchPosition.y);

                if (touch.phase == TouchPhase.Began)
                {
                   Vector2 dir = touchPos - oldtouch;
                    GameObject lineclone;
                    if (linebin.Grab(out lineclone))
                    {
                       
                        lineclone.transform.position = oldtouch;
                        lineclone.transform.rotation = Quaternion.Euler(dir);
                        lineclone.SetActive(true);
                    }
                    else
                    {

                        lineclone = Instantiate(linearprojection, oldtouch, Quaternion.Euler(dir));
                    }
                    curLine = lineclone;
                    start = false;
                }

               
               
                if (!IsPointerOverUIObject())
                {
                    Ray2D ray = new Ray2D(oldtouch, touchPos - oldtouch);
                    float dist = Vector2.Distance(oldtouch, touchPos);
                    Vector2 dir = touchPos - oldtouch;// new Vector2(touchPos.x - transform.position.x , touchPos.y- transform.position.y );
                    RaycastHit2D hit = Physics2D.Raycast(oldtouch, dir, dist, layerMask);

                    float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg)-90f;

                    curLine.transform.rotation = Quaternion.Euler(0, 0, angle);

                    curLine.GetComponent<SpriteRenderer>().size = new Vector2(18.38f, dist*30f);
                    if (hit.collider != null)//.tag ==  "wall" || hit.collider.tag == "spikeyWall")
                    {
                        curLine.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0.009137154f);
                        Vector2 dir2 = new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y) - hit.point;
                        print(hit.collider.tag);

                       
                        Debug.DrawRay(oldtouch, dir2, Color.red);
                        Debug.DrawRay(oldtouch, dir, Color.green);

                       
                      
                        clearPath = false;

                    }
                    else
                    {
                        curLine.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

                        clearPath = true;

                        Debug.DrawRay(oldtouch, dir, Color.blue);
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    yield return new WaitForEndOfFrame();
                    print(count + " " + stack);
                    
                        if (clearPath)
                        {
                            zoomqueue.Enqueue(touchPos);
                        count++;
                        if (zoomqueue.Count >= stack)
                            {
                       
                                startTime = Time.time;
                                journeyLength = Vector2.Distance(transform.position, touchPosition);
                                Vector2 dirr = new Vector2(transform.position.x, transform.position.y) - new Vector2(touchPosition.x, touchPosition.y);
                                start = true;
                            }
                            oldtouch = touchPos;
                        }
                    
                    else
                    {
                        linebin.Store(curLine);
                       
                    }
                    

                }


            }
          
            if (start)
            {
               
                if (FullSreenEffect.color.a >= 0.0f)
                {
                    FullSreenEffect.color -= new Color(0, 0, 0, 0.01f);
                }
                else { FullSreenEffect.gameObject.SetActive(false); }
                float distCovered = (Time.time - startTime) * 20;
                float fracJourney = distCovered / journeyLength;
                transform.position = Vector2.Lerp(transform.position, zoomqueue.Peek(), fracJourney);
                //print(fracJourney);
                if (fracJourney >= 1)
                {
                    zoomqueue.Dequeue();
                    if (zoomqueue.Count == 0)
                    {
                        foreach (GameObject line in GameObject.FindGameObjectsWithTag("line"))
                        {
                           
                            linebin.Store(line);
                        }
                        count = 0; zoomz = false; FullSreenEffect.gameObject.SetActive(false);
                        start = false;
                        zoomqueue.Clear();
                        stack = 0;
                        yield break;
                    }
                    else
                    {
                        startTime = Time.time;
                        journeyLength = Vector2.Distance(transform.position, zoomqueue.Peek());
                    }
                }
            }
           
            yield return new WaitForFixedUpdate();
        }
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
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
            if (GameManager.gameSpeed < 0.001f)
            {
                GameManager.gameSpeed = 0.0001f;
            }

            else { yield return null; }
        }
    }
    public bool up, down, left, right;
    //void OnPointerDown(PointerEventData eventData)
    //{
    //    if (up) { transform.Translate(new Vector2(0, 1) * moveSpeed); }
    //    if (down) { transform.Translate(new Vector2(0, -1) * moveSpeed); }
    //    if (left) { transform.Translate(new Vector2(-1, 0) * moveSpeed); }
    //    if (right) { transform.Translate(new Vector2(1, 0) * moveSpeed); }
    //    if (eventData.pointerCurrentRaycast.gameObject.name == "trysmt")
    //    {
    //        playerclicked = true;
    //        GetComponentInChildren<ParticleSystem>().Stop();
    //    }
    //    else playerclicked = false;
    //    //  Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    //}

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
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Ended)
        {
            if (GameControl.control.zooms > 0)
            {
                if (zoomz) { stack++; }
                if (!zoomz)
                {
                    zoomz = true;
                    stack++;
                    oldtouch = new Vector2(transform.position.x, transform.position.y);
                    StartCoroutine(ZoomTo());
                    FullSreenEffect.gameObject.SetActive(true);
                    trailrenderer.enabled = true;
                    FullSreenEffect.color = new Color(0.3176471f, 0.6705883f, 1, 0.5f);
                }

                GameControl.control.zooms--;
            }
        }
    }
    int flytime = 0;
    public void Fly()
    {
        if (GameControl.control.flys > 0)
        {
            if (!flyin)
            {
                flyin = true;
                stack++;
                flytime += 100;
                FullSreenEffect.gameObject.SetActive(true);
                FullSreenEffect.color = new Color(0.984f, 0.149f, 0.023f, 0.5f);
            }
            if (flyin)
            {
                stack++;
                flytime += 100;
            }
            print("fkying");

            gameObject.layer = LayerMask.NameToLayer("FlyingPlayer");
            GameControl.control.flys--;
        }
    }


    public void goUP()
    {
        up = true; down = false;
    }
    public void goDOWN()
    {
        down = true; up = false;
    }
    public void goLeft()
    {
        left = true; right = false;
    }
    public void goRight()
    {
        right = true; left = false;
    }
    bool isdamaged = false;
    public void OnCollisionEnter2D(Collision2D other)
    {
      //  print(other.gameObject.tag);
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
        if (other.gameObject.tag == "wall" && !flyin)
        {

            playerclicked = false;
            transform.Translate(normalizeToDirection(transform.position, other.collider) * 0.2f);


        }

        if (other.gameObject.tag == ("Finish"))
        {
            // print("hit");
            //Time.timeScale = 0f;
            //StartCoroutine(gameManager.screenPulse());
            slowdown = false;
            GameManager.gameSpeed = 0;
            gameManager.gobackyn = false;
            if (other.contacts[0].otherCollider.transform.gameObject.name == "trysmt")
            {
                Physics2D.IgnoreCollision(thumbStop.GetComponentInChildren<Collider2D>(), other.gameObject.GetComponent<Collider2D>());

                gameManager.Finished(GameManager.FinishState.ALIVE, gameManager.Win, gameManager.nextLevl);
            }

        }


        if (other.gameObject.tag == "laser" || (other.gameObject.tag == "spikes" && !flyin))
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
                    //Time.timeScale = (0);
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
                        Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>());

                        GameControl.control.lives = 3;
                    }
                    else
                    {
                        Invoke("dotheInvoke2", 1);
                        
                        GameControl.control.lives--;

                        Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
                        gameManager.gobackyn = true;
                        //GameControl.control.seedToLoad = gameManager.thisSeed;


                        // reloadLevel();
                    }
                }
            }
        }
        else { GetComponent<Animator>().SetBool("Frantic", false); }
    }
    public void dotheInvoke()
    {
        gameManager.Finished(GameManager.FinishState.GAMEOVER, gameManager.GameOver, null);
    }
    void dotheInvoke2()
    {
        gameManager.Finished(GameManager.FinishState.DEAD, gameManager.Lose, gameManager.restart);
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
public static class hasComponent
{
    public static bool HasComponent<T>(this GameObject flag) where T : Component
    {
        return flag.GetComponent<T>() != null;
    }
}
