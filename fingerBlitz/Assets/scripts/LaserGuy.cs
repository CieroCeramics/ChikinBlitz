using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGuy : GuyBehavior
{

   // Color linecolor = new Color(0, 1, 1, 0);
    int status = 1;

   // private LineRenderer lineRenderer;
    public Transform LaserHit;
    // Start is called before the first frame update
    public ParticleSystem particles;
    public ParticleSystem particles2;
    Animator Anim;//= GetComponentInChildren<Animator>();
    //gameSpeed =1;
    void Start()
    {
        playa = GameObject.FindGameObjectWithTag("Player");
        Anim = GetComponentInChildren<Animator>();
        StartCoroutine(Lasers());

    }

    void OnBecameVisible()
    {
        StartCoroutine(Lasers());
    }
    private void OnBecameInvisible()
    {
        StopCoroutine(Lasers());
    }
    // Update is called once per frame
    void Update()
    {
        //gameSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<DragMove>().gameSpeed;
    }


    IEnumerator Lasers()
    {
        
        //gameSpeed = playa.GetComponent<DragMove>().gameSpeed;        gameSpeed = playa.GetComponent<DragMove>().gameSpeed;

        
        float t = 0;
        // yield WaitforSeconds(1);
        //ParticleSystem.
        ParticleSystem.EmissionModule em = particles.emission;
        while (GameManager.gameSpeed>0)
        {

            if (status >= 4) { status = 1; }
            Anim.speed = GameManager.gameSpeed;
            // em.rate ;
            //  lineRenderer.startColor = linecolor;
            //  lineRenderer.endColor = linecolor;
            if (playa.GetComponent<DragMove>().firsttouch == true)
            {

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

                      
                        // get the angle
                      //  Anim.Play("laser");
                        //particles.Simulate(t);
                       // particles2.Simulate(t);
                        Vector2 dir = playa.transform.position - transform.position;
                        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.Euler(0, 0, angle);
                        t += 0.08f;

                        Vector3 res = LaserHit.position - transform.position;
                
                        if (Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !Anim.IsInTransition(0))
                        {
                            Anim.Play("cooldown", 0, 0.0f);
                            status++;
                        }

                        break;

                        case 2://fire
                        Anim.SetBool("Fire", true);
                        
                        //Anim.speed = 0;
                        t = 0;
                            Bullet bulletCopy;
                        if (GameManager.gameSpeed >= 0.1f)
                        {
                            bulletCopy = Instantiate(bulletPrefab, transform.position, transform.rotation);
                            bullets.Add(bulletCopy);
                            bulletCopy.speed = 0.1f;
                        }
                        if (bullets.Count >= 100)
                        {
                            Anim.speed = 1;
                            status++;
                            bullets = new List<Bullet>();
                        }

                        break;
                         case 3://cooldown
                       // Anim.StopPlayback();
                        // particles2.Stop();
                      
                        //Anim.speed = 0;
                        if (bullets.Count <10)
                        {
                            Anim.SetBool("Fire", false);
                            // Anim.speed = 1;
                            Anim.Play("Laser", 0, 0.0f);
                            status = 1;
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
            }


            int WT = (int)(1 / (GameManager.gameSpeed + 1));
           // yield return new WaitForSeconds(WT);
            yield return WaitFor.Frames(WT);
            print("waotot" + WT);
          

        }
        yield return null;
    }
}

