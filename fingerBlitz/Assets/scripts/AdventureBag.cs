using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AdventureBag : MonoBehaviour
{
    public static AdventureBag aBag;
    public Button inBag;
    public Button onHand;
    public GameObject Bag;
    public GameObject Hand;
    public int flys, zooms, times, bawks;

    private Image img;
    public void setupBag()
    {

        if (GameControl.control.flys > 0)
        {
            Button fBagCopy;
            fBagCopy = Instantiate(inBag);
            fBagCopy.transform.SetParent(Bag.transform);
            fBagCopy.gameObject.GetComponent<Button>().onClick.AddListener(() => TakeWith(fBagCopy, 1));
            fBagCopy.tag ="fbag";
            
            //flys--;
        }
       
        if (GameControl.control.zooms > 0)
        {
            Button zBagCopy;
            zBagCopy = Instantiate(inBag);
            zBagCopy.transform.SetParent(Bag.transform);
            zBagCopy.gameObject.GetComponent<Button>().onClick.AddListener(() => TakeWith(zBagCopy, 2));
            zBagCopy.tag = "zbag";
        }
        if (GameControl.control.times > 0)
        {
            Button tBagCopy;
            tBagCopy = Instantiate(inBag);
            tBagCopy.transform.SetParent(Bag.transform);
            tBagCopy.gameObject.GetComponent<Button>().onClick.AddListener(() => TakeWith(tBagCopy, 3));
            tBagCopy.tag = "tbag";
        }

        displayInformation(Bag);





        if (flys > 0)
        {
            Button fBagCopy;
            fBagCopy = Instantiate(onHand);
            fBagCopy.transform.SetParent(Hand.transform);
            fBagCopy.gameObject.GetComponent<Button>().onClick.AddListener(() => LeaveBehind(fBagCopy, 1));
            fBagCopy.tag = "fbag";

            //flys--;
        }

        if (zooms > 0)
        {
            Button zBagCopy;
            zBagCopy = Instantiate(onHand);
            zBagCopy.transform.SetParent(Hand.transform);
            zBagCopy.gameObject.GetComponent<Button>().onClick.AddListener(() => LeaveBehind (zBagCopy, 2));
            zBagCopy.tag = "zbag";
        }
        if (times > 0)
        {
            Button tBagCopy;
            tBagCopy = Instantiate(onHand);
            tBagCopy.transform.SetParent(Hand.transform);
            tBagCopy.gameObject.GetComponent<Button>().onClick.AddListener(() => LeaveBehind(tBagCopy, 3));
            tBagCopy.tag = "tbag";
        }

        displayInformation(Hand);
    }
    void displayInformation(GameObject Bagaroo)
    {
        if (Bagaroo == Bag)
        {
            foreach (Button b in Bagaroo.GetComponentsInChildren<Button>())
            {
                if (b.tag == "fbag")
                {
                    b.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("fly");
                    b.GetComponentInChildren<Text>().text = GameControl.control.flys.ToString();
                }
                if (b.tag == "zbag")
                {
                    b.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("Zoom");
                    b.GetComponentInChildren<Text>().text = GameControl.control.zooms.ToString();
                }
                if (b.tag == "tbag")
                {
                    b.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("timeStop");
                    b.GetComponentInChildren<Text>().text = GameControl.control.times.ToString();
                }
            }

        }


        if (Bagaroo == Hand)
        {
            foreach (Button b in Bagaroo.GetComponentsInChildren<Button>())
            {
                if (b.tag == "fbag")
                {
                    b.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("fly");
                    b.GetComponentInChildren<Text>().text = flys.ToString();
                }
                if (b.tag == "zbag")
                {
                    b.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("Zoom");
                    b.GetComponentInChildren<Text>().text = zooms.ToString();
                }
                if (b.tag == "tbag")
                {
                    b.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("timeStop");
                    b.GetComponentInChildren<Text>().text = times.ToString();
                }
            }
        }

        }
    public void TakeWith(Button self, int type)           //function to run when the BUTTON in the Bag is clicked,
    {
       // Button onHandCopy;
        switch (type)
        {
            case 1:    //flys
                GameControl.control.flys--;       //subtract from how many we have in the bag (saved to file)

                if (flys <= 0)              //if we have 0 onhand, create a button in the hand. make that button run the LEAVE BEHIND script
                {
                    Button fBagCopy;
                    fBagCopy = Instantiate(onHand);     //create a button for the ONHAND buttons
                   fBagCopy.transform.SetParent(Hand.transform);          //parent it to the ON HAND grid
                    fBagCopy.gameObject.GetComponent<Button>().onClick.AddListener(() => LeaveBehind(fBagCopy, 1));      //make it LEAVE BEHIND on click
                    fBagCopy.tag = "fbag";
                }
                flys++;
                if (GameControl.control.flys == 0)
                {
                    Destroy(self.gameObject);
                }
                break;



            case 2:    //zoomz
                GameControl.control.zooms--;

                if (zooms <= 0)
                {
                    Button zBagCopy;
                    zBagCopy = Instantiate(onHand);
                    zBagCopy.transform.SetParent(Hand.transform);
                    zBagCopy.gameObject.GetComponent<Button>().onClick.AddListener(() => LeaveBehind(zBagCopy, 2));
                    zBagCopy.tag = "zbag";

                }
                zooms++;
                if (GameControl.control.zooms == 0)
                {
                    Destroy(self.gameObject);
                }
                break;



            case 3:    //times
                GameControl.control.times--;

                if (times <= 0)
                {
                    Button tBagCopy;
                    tBagCopy = Instantiate(onHand);
                    tBagCopy.transform.SetParent(Hand.transform);
                    tBagCopy.gameObject.GetComponent<Button>().onClick.AddListener(() => LeaveBehind(tBagCopy, 3));
                    tBagCopy.tag = "tbag";
                }
                times++;
                if (GameControl.control.times == 0)
                {
                    Destroy(self.gameObject);
                }
                break;
        }

        displayInformation(Hand);
        displayInformation(Bag);
    }
    public void LeaveBehind(Button self, int type)
    {
        switch (type)
        {
            case 1:    //flys
                flys--;

                if (GameControl.control.flys <= 0)
                {
                    Button fBagCopy;
                    fBagCopy = Instantiate(inBag);
                    fBagCopy.transform.SetParent(Bag.transform);
                    fBagCopy.gameObject.GetComponent<Button>().onClick.AddListener(() => TakeWith(fBagCopy, 1));
                    fBagCopy.tag = "fbag";
                }
                GameControl.control.flys++;
                if (flys == 0)
                {
                    Destroy(self.gameObject);
                }
                break;


            case 2:    //zooms
                zooms--;

                if (GameControl.control.zooms <= 0)
                {
                    Button zBagCopy;
                    zBagCopy = Instantiate(inBag);
                    zBagCopy.transform.SetParent(Bag.transform);
                    zBagCopy.gameObject.GetComponent<Button>().onClick.AddListener(() => TakeWith(zBagCopy, 2));
                    zBagCopy.tag = "zbag";
                }
                GameControl.control.zooms++;
                if (zooms == 0)
                {
                    Destroy(self.gameObject);
                }
                break;


            case 3:    //times
                times--;

                if (GameControl.control.times <= 0)
                {
                    Button tBagCopy;
                    tBagCopy = Instantiate(inBag);
                    tBagCopy.transform.SetParent(Bag.transform);
                    tBagCopy.gameObject.GetComponent<Button>().onClick.AddListener(() => TakeWith(tBagCopy, 3));
                    tBagCopy.tag = "tbag";
                }
                GameControl.control.times++;
                if (times == 0)
                {
                    Destroy(self.gameObject);
                }
                break;
        }
        displayInformation(Hand);
        displayInformation(Bag);
    }

    public void closeBag()
    {
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Awake()
        {
        if (aBag == null)
        {
            DontDestroyOnLoad(gameObject);
            aBag = this;
        }
        else if (aBag != this)
        {
            Destroy(gameObject);
        }
    }

        // Update is called once per frame
        void Update()
        {

        }
    
}
