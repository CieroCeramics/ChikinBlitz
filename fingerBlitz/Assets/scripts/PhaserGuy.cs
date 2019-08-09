using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaserGuy : GuyBehavior
{
    public float spin;
    // Start is called before the first frame update
    void Start()
    {
        //gameSpeed = 1;
     //   StartCoroutine(Phasers1(90f));
        StartCoroutine(Phasers1(0f));
        StartCoroutine(Phasers1(180f));
        fireRate = 20f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, 0, spin*GameManager.gameSpeed);
        
    }



    IEnumerator Phasers1(float dir)
    {

        while (true)
        {
            spin = 2*GameManager.gameSpeed;
            playa = GameObject.FindGameObjectWithTag("Player");
            Bullet bulletCopy;
            
            if ((playa.GetComponent<DragMove>().firsttouch == true) && GameManager.gameSpeed > 0.1f)
            {
                
                    bulletCopy = Instantiate(bulletPrefab, transform.position, transform.rotation * (Quaternion.Euler(0, 0, dir)));
                    bulletCopy.speed = 0.02f;
                
            }
            yield return WaitFor.Frames((int)((1 / GameManager.gameSpeed )*fireRate ));
        }

    }
    IEnumerator Phasers2()
    {

        while (true)
        {
            spin = 0.5f;
            playa = GameObject.FindGameObjectWithTag("Player");
            Bullet bulletCopy;
            yield return new WaitForSeconds(fireRate/ GameManager.gameSpeed);
            if ((playa.GetComponent<DragMove>().firsttouch == true)&& GameManager.gameSpeed > 0)
            {

                bulletCopy = Instantiate(bulletPrefab, transform.position, transform.rotation * (Quaternion.Euler(0, 0, 90)));
                bulletCopy.speed = 0.005f;
            }

        }

    }
}
