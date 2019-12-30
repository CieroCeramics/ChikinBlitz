using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGuy : GuyBehavior
{

    public GameManager gm;
   // Color linecolor = new Color(0, 1, 1, 0);
    int status = 1;

   // private LineRenderer lineRenderer;
    public Transform LaserHit;
    // Start is called before the first frame update

    Animator Anim;//= GetComponentInChildren<Animator>();
    //gameSpeed =1;
    void Awake()
    {
        gm= GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        playa = GameObject.FindGameObjectWithTag("Player");
        Anim = GetComponentInChildren<Animator>();
        StartCoroutine(Lasers2());
         fireRate = 20f;
    }

    // void OnBecameVisible()
    // {
    //     StartCoroutine(Lasers2());
    // }
    private void OnBecameInvisible()
    {
        //StopCoroutine(Lasers());
    }
    // Update is called once per frame
    void Update()
    {
        if(Anim)
         Anim.speed = GameManager.gameSpeed;
        //gameSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<DragMove>().gameSpeed;
    }

    int k = 0;
    IEnumerator Lasers()
    {
        
        //gameSpeed = playa.GetComponent<DragMove>().gameSpeed;        gameSpeed = playa.GetComponent<DragMove>().gameSpeed;

        
        float t = 0;
        // yield WaitforSeconds(1);
        //ParticleSystem.
        //ParticleSystem.EmissionModule em = particles.emission;
        while (true)
        {
    
            if (status >= 4) { status = 1; }
           
            //print (Anim.speed);
            // em.rate ;
            //  lineRenderer.startColor = linecolor;
            //  lineRenderer.endColor = linecolor;
            

                // yield return new WaitForSeconds(0.5f);
                int screenlayer = LayerMask.NameToLayer("Screen");
                int playerlayer = LayerMask.NameToLayer("Player");
                int lasermask = LayerMask.NameToLayer("Laser");
                int wallLayer = LayerMask.NameToLayer("Ignore Raycast");
                int layerMask = ~((1 << playerlayer) | (1 << wallLayer));

                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 500f, layerMask);

                
                LaserHit.position = hit.point;

                switch (status)
                {

                      case 1://charging

                      Anim.gameObject.GetComponent<Animator>().enabled = true;
                        // get the angle
                      //  Anim.Play("laser");
                        //particles.Simulate(t);
                       // particles2.Simulate(t);
                        Vector2 dir = playa.transform.position - transform.position;
                        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.Euler(0, 0, angle);
                        t += 0.08f;

                        Vector3 res = LaserHit.position - transform.position;
                
                        if (Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !Anim.IsInTransition(0))
                        {
                             Anim.SetBool("Fire", true);
                            // Anim.Play("cooldown", 0, 0.0f);
                            status=2;
                        }

                        break;

                        case 2://fire
                        
                    k++;
                        //Anim.speed = 0;
                        t = 0;
                            Bullet bulletCopy;
                    int WT = (int)(1 / (GameManager.gameSpeed));
                    if (k >= WT||GameManager.gameSpeed==gameManager.maxGameSpeed) // GameManager.gameSpeed >= 0.1f)
                        {
                        k = 0;
                            bulletCopy = Instantiate(bulletPrefab, transform.position, transform.rotation);
                        bulletCopy.type = 2;
                            bullets.Add(bulletCopy);
                           //bulletCopy.speed = 0.1f;
                        }
                        if (bullets.Count >= 50)
                        {
                            //Anim.speed = 1;
                            status=3;
                            bullets = new List<Bullet>();
                        }
                       // 
                        break;
                         case 3://cooldown
                       // Anim.StopPlayback();
                        // particles2.Stop();
                      
                        //Anim.speed = 0;
                          if (Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !Anim.IsInTransition(0))
                       {
                            Anim.SetBool("Fire", false);
                            // Anim.speed = 1;
                            Anim.Play("Laser", 0, 0.0f);
                            status = 1;
                            Anim.gameObject.GetComponent<Animator>().enabled = false;
                             //eld return new WaitForSeconds(4f);
                        }
                       
                        break;
                        

                }




                //if ((hit.collider.tag == "Player") && (status == 3))
                //{
                //    playa.GetComponent<DragMove>().finished("DEAD");
                //}

                //   ishitting = false;

                //lineRenderer.startColor = linecolor;
                // lineRenderer.endColor = linecolor;
                //int waittime = (int)(1 / (GameManager.gameSpeed + 1));
                
                //yield return WaitFor.Frames (waittime);
                //print("waotot" + waittime);
            


            
            // yield return new WaitForSeconds(WT);

            yield return null;// WaitFor.Frames(WT);
            //print("waotot" + WT);
          

        }
        yield return null;
    }
    IEnumerator Lasers2()
    {
        int k = 0;
       
        while(true)
        {
            k++;
            Bullet bulletCopy;
             int WT = (int)(1 / (GameManager.gameSpeed) * fireRate);
             if (k >= WT && GameManager.gameSpeed>0)
            {
                k = 0;
                bulletCopy = Instantiate(bulletPrefab, transform.position, transform.rotation);
                bulletCopy.type = 2;
                bulletCopy.dims = gm.screenSize;
                //   bulletCopy.speed = 0.02f;

            }
            Vector2 dir = playa.transform.position - transform.position;
                        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
         //  Rotation a = new Rotation(0,0,angle);
                        Quaternion a = Quaternion.Euler(0,0,angle);
                        Quaternion b =Quaternion.Euler(0,0,Mathf.Sin(Time.time*4f));
                        Quaternion c =a*b;
           //Rotation  b = new Rotation(0,0,Mathf.Sin(Time.time*4f));
                   //     
           transform.rotation=Quaternion.Euler(0,0,angle)*Quaternion.Euler(0,0,Mathf.Sin(Time.time*2f)*30f);
           //transform.Rotate(0,0,Mathf.Sin(Time.time*4f));
             yield return new WaitForFixedUpdate();

        }
    }
}

