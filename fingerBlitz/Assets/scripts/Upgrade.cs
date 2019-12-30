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
        if (GameControl.control.Stage == 1)
        {
            transform.localScale = new Vector2(transform.localScale.x / 1.5f, transform.localScale.y / 1.5f);

        }
        if (GameControl.control.Stage == 2)
        {
            transform.localScale = new Vector2(transform.localScale.x / 2, transform.localScale.y / 2);

        }
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
            case 5:
                gameObject.tag = "ScoreAdder";
                spriteR.sprite = Resources.Load<Sprite>("upscore");
                return;
            case 6:
                gameObject.tag = "key";
                spriteR.sprite = Resources.Load<Sprite>("key");
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
        if ((Destiny < 0.3f) && (Destiny >= 0f))
        {
            identity = 1;
        }
        else if (Destiny < 0.6f && Destiny >= 0.3f)
        {
            identity = 2;
        }
        else if (Destiny < 0.9f && Destiny >= 0.6f)
        {
            identity = 3;
        }
        else identity = 4;
        if (GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].levels[GameControl.control.LevelNumber%5].complete)
        {
            identity = 5;
        }
        if(GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].keyLevel == 
            GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].levels[GameControl.control.LevelNumber % 5].number%5)
        {
            identity = 6;
        }
        if (GameControl.control.stagedata[GameControl.control.Stage].phases[GameControl.control.phasenum].completed)
        {
            identity = 5;
        }
    }
    private void GetDressed()
    {

    }
}
