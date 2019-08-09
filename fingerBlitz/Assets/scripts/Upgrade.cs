using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public int identity;
    private SpriteRenderer spriteR;
    // Start is called before the first frame update
    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        DecideWhoYouAre();
        switch (identity)
        {
            case 1:
                gameObject.tag = "timeStop";
                spriteR.sprite = Resources.Load<Sprite>("timeStop");
                return;
            case 2:
                gameObject.tag = "zoom";
                spriteR.sprite = Resources.Load<Sprite>("Zoom");
                return;
            case 3:
                gameObject.tag = "fly";
                spriteR.sprite = Resources.Load<Sprite>("fly");
                return;
            case 4:
                gameObject.tag = "life";
                spriteR.sprite = Resources.Load<Sprite>("Life");
                return;
        }

    }
     
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //print("hi2 " + collision.gameObject.tag);
        if ((collision.gameObject.tag == "Respawn") || 
            (collision.gameObject.tag == "Finish")  || 
            (collision.gameObject.tag == "guy")     || 
            (collision.gameObject.tag == "spikes")  || 
            (collision.gameObject.tag == "wall"))
        {
           Vector2 ScreenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            transform.position = new Vector2(Random.Range(0, ScreenSize.x), Random.Range(0, ScreenSize.y));

            //Destroy(gameObject);
        }
    }
    private void DecideWhoYouAre()
    {
        float Destiny = Random.value;
       // print(Destiny);
        if ((Destiny<0.3f)&&(Destiny>=0f))
        {
            identity = 1;
        }
        else if (Destiny < 0.6f&& Destiny>=0.3f)
        {
            identity = 2;
        }
        else if (Destiny < 0.9f && Destiny >= 0.6f)
        {
            identity = 3;
        }
        else identity = 4;
    }
    private void GetDressed()
    {

    }
}
