using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int type;
  //  public Texture2D[] bulletTex;

    private bool outOfBoundsKill;
        private Transform OOB;
    Plane[] planes;
     float speed =0.01f;
    public float acceleration =0.00002f;
    //float gameSpeed = 1f;
    public Vector2 dims;
    // Start is called before the first frame update
    void Start()
    {
        if (type ==2)
        {
            GetComponent<Animator>().Play("bullet3", 0, 0);
        }
        if (GameControl.control.Stage == 1)
        {
            transform.localScale = new Vector2(transform.localScale.x / 1.5f, transform.localScale.y / 1.5f);

        }
        if (GameControl.control.Stage == 2)
        {
            transform.localScale = new Vector2(transform.localScale.x / 2, transform.localScale.y / 2);

        }
        //gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        //dewey = transform.Translate(transform.TransformDirection(-transform.up) * speed);
        planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       speed = speed - acceleration *Time.deltaTime;
        //if(gameManager.destroyAllBullets)
        //{
        //    Destroy(gameObject);
        //}
        //gameSpeed = gameManager.gameSpeed;
        transform.Translate(new Vector3(1,0,0)*speed*GameManager.gameSpeed);
        eat(CheckWorldBounds());
    }

    private bool CheckWorldBounds()
    {
                  if(transform.position.x<dims.x/2
            && transform.position.x > -dims.x / 2
            && transform.position.y < dims.y / 2
            && transform.position.y > -dims.y / 2)
        //if (GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider2D>().bounds))
        {
            return false;
        }
        else return true;
    }
       public void BAWK()
    {
        Destroy(gameObject);
    }
    void eat(bool OOB)
    {
        if(OOB)
        Destroy(gameObject);
    }
    private void kill()
    {
        
    }
}
