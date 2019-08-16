using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

  //  public Texture2D[] bulletTex;

    private bool outOfBoundsKill;
        private Transform OOB;
    Plane[] planes;
    public float speed =0.1f;
    float gameSpeed = 1f;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        //dewey = transform.Translate(transform.TransformDirection(-transform.up) * speed);
        planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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

        if (GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider2D>().bounds))
        {
            return false;
        }
        else return true;
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
