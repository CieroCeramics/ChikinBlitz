using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chickie : MonoBehaviour
{
	 public Animator animator;
 public Partitions.Sector Home = new Partitions.Sector();
 bool reacheddest;
 Vector2 dest;
 Vector2 start;
 Vector2 dir  ;
    // Start is called before the first frame update
    void Start()
    {
        if (GameControl.control.Stage == 1)
        {
            transform.localScale = new Vector2(transform.localScale.x / 1.5f, transform.localScale.y / 1.5f);

        }
        if (GameControl.control.Stage == 2)
        {
            transform.localScale = new Vector2(transform.localScale.x / 2, transform.localScale.y / 2);

        }
        start = transform.position;
    	
        dir = dest-start;
    	reacheddest=false;
        animator = GetComponent<Animator>();
        SD();
    }

    // Update is called once per frame
    void Update()
    {
Vector2 curpos = new Vector2 (transform.position.x, transform.position.y);
      //  curpos.Normalize();
    	if(!reacheddest)
    	{
    		walking();
    	}
        else { transform.position = transform.position;}
        
    	if(Vector2.Distance(dest,curpos)<0.3f&&!reacheddest)
    	{
    		reacheddest =true;
    		animator.SetBool("Walk",false);
    		Invoke("SD",2);
    	}

      //  transform.Translate())
    }
    float startTime;
float journeyLength;
    void SD()
    {
    	dest=setDest();

        while (Vector2.Distance(dest,transform.position)<0.3f)
        {
            dest = setDest();
        }
        start = transform.position;
        dir = dest-start;
    	animator.SetBool("Walk",true);
    	reacheddest =false;
         startTime = Time.time;
         journeyLength = Vector2.Distance(dest,transform.position);
         if (dir.x > 0)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (dir.x <= 0)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }
    }
void walking()
{
	print("w");
    print(Home.north + " "+ Home.south+" "+ Home.east+" "+ Home.west );
	
    dir.Normalize();
	 
     float distCovered = (Time.time - startTime) * 0.5f;
     float fracJourney = distCovered / journeyLength;
     fracJourney = Easing.Sinusoidal.In(fracJourney);
     transform.position = Vector2.Lerp(start, dest, fracJourney);
    //transform.Translate(dir*0.1f);
}

    Vector2 setDest()
    {
         

    	float x= Random.Range(Home.west,Home.east);
    	float y = Random.Range(Home.south, Home.north);
    	
    	return new Vector2 (x,y);
    }
void OnTriggerEnter(Collider collider)
{
    if(collider.tag == "player")
    {
        Destroy(gameObject);
    }
}

}
