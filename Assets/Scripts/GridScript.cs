using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts;//our script folder, so we can access our classes

[System.Serializable]
public class GridScript : MonoBehaviour
{
    public AudioSource Quack;//our quack sound
    public Sprite[] UpgradeImages;//drop in new duck upgrade sprites here (in the inspector)
    public TextMeshProUGUI[] UpgradePricesTxts;//drop all the price texts in here (see instructions for DuckObjects)
    public GameObject[] UpgradeButtons;//drop all the upgrade buttons in here (see instructions for DuckObjects)
    public GameObject[] DuckObjects;
    //if you changed the grid size in the Unity editor:
    //lock the DuckGrid gameobject in the top-right of the inspector, set the array elements to 0, then select and drag all the ducks onto the array's name

    private GameObject[,] Ducks;//we'll save the precise locations of our ducks in the grid using this 2D array
    private DuckUpgrade[] DuckUpgrades;

    //set size of our duck grid here
    private int DuckColumns = 8;
    private int DuckRows = 7;

    public TextMeshProUGUI Dbtxt;//text showing our dB currency
    private int Decibels = 0;//our dB currency

    private int ActiveDucks = 0;//current ducks on screen
    private int CurrentUpgradeLvl = 0;//current duck upgrade level
    private int HonkPower = 1;//upgrades can increase this honk power multiplier

    public GameObject WinnerPanel;

    void Awake()//Awake happens before Start
    {
        //change upgrade prices here and make new upgrades
        DuckUpgrades = new DuckUpgrade[]
        {
            new DuckUpgrade("DoubleDucks", 1000, UpgradePricesTxts[0], UpgradeImages[1]),//image 0 is regular duck sprite, so we start the first upgrade with 1
            new DuckUpgrade("BigHonkers", 2000, UpgradePricesTxts[1], UpgradeImages[2]),
            new DuckUpgrade("Coolducks", 3000, UpgradePricesTxts[2], UpgradeImages[3]),
            new DuckUpgrade("Duckroll", 5000, UpgradePricesTxts[3], UpgradeImages[4]),
            new DuckUpgrade("Duck tanks", 10000, UpgradePricesTxts[4], UpgradeImages[5]),
            new DuckUpgrade("Cyberducks", 77000, UpgradePricesTxts[5], UpgradeImages[6])
        };

        CreateDuckGrid();//only use once to set our 2D array and give the ducks their IDs
        disableAllDucks();//makes them invisible and not clickable
        disableAllButtons();//hide future upgrades
        WinnerPanel.SetActive(false);//hide the winner panel just in case we left it open
        setPriceTexts();
        toggleDuck(DuckColumns / 2, DuckRows / 2);//enables a duck in the middle
        addDBs(0);//refreshes the text
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            mouseClicked();
        }

        //debug buttons
        if (Input.GetKeyDown("9"))
        {
            addDBs(200000);
        }
    }

    private void mouseClicked()
    {
        Quack.volume = 0.3f + (ActiveDucks / 100f)*2;//more volue for more ducks
        Quack.Play();
        addDBs(ActiveDucks * HonkPower);//gives one decibel for each active duck and multiplies with honk power
    }

    private void CreateDuckGrid()
    {
        Ducks = new GameObject[DuckColumns, DuckRows];//set the size of our array
        int DucksLength = DuckObjects.Length;
        int DuckPointer = 0;

        //sort the ducks into the array
        for (int Yducks = 0; Yducks < DuckRows; Yducks++) 
        {
            for (int Xducks = 0; Xducks < DuckColumns; Xducks++)
            {
                //saves our duck into the 2D array, so we know where it is in the grid
                if (DuckPointer < DucksLength)
                {
                    Ducks[Xducks, Yducks] = DuckObjects[DuckPointer];//saves the gameobject in our 2D array
                    Ducks[Xducks, Yducks].GetComponent<DuckID>().setPositionIngrid(Xducks, Yducks);//give it an ID so it can tell where the click came from
                    DuckPointer++;
                }
            }
        }
    }

    private void disableAllDucks()
    {
        for (int Yducks = 0; Yducks < DuckRows; Yducks++) 
        {
            for (int Xducks = 0; Xducks < DuckColumns; Xducks++)
            {
                Ducks[Xducks, Yducks].GetComponent<Image>().enabled = false;
                ActiveDucks = 0;
            }
        }
    }

    private void disableAllButtons()
    {
        for(int i=0; i < UpgradeButtons.Length; i++)
        {
            UpgradeButtons[i].SetActive(false);
        }
        setButtonVisibility();
    }

    private void setPriceTexts()
    {
        for(int i=0; i<UpgradePricesTxts.Length; i++)
        {
            UpgradePricesTxts[i].text = DuckUpgrades[i].getPrice().ToString() + "dB";
        }
    }

    private void setButtonVisibility()
    {
        if (CurrentUpgradeLvl < UpgradeButtons.Length)//in case the button is the last one, this makes sure the array isn't out of bounds
            UpgradeButtons[CurrentUpgradeLvl].SetActive(true);
        else//if it's the last button the game is done, we can cover the buttons with the winner panel
            WinnerPanel.SetActive(true);
    }

    private void addDBs(int ammountToAdd)//can add negative to take away
    {
        Decibels += ammountToAdd;
        Dbtxt.text = "dB:\n" + Decibels.ToString();
    }

    //enable OR disable a single duck
    private void toggleDuck(int Xduck, int Yduck)
    {
        if (Xduck < DuckColumns && Xduck >= 0 && Yduck < DuckRows && Yduck >= 0)//check in case the coordinates are out of range
        {
            //checks current state and toggles it
            bool newState = !Ducks[Xduck, Yduck].GetComponent<Image>().enabled;
            Ducks[Xduck, Yduck].GetComponent<Image>().enabled = newState;

            //update how many ducks are active now
            if (newState) ActiveDucks++;
            if (newState == false) ActiveDucks--;
        }
    }

    public void duckClicked(int x, int y)
    {
        toggleDuck(x, y);//the duck clicked should disappear

        //the 4 directly adjacent ducks get toggled
        toggleDuck(x - 1, y);
        toggleDuck(x + 1, y);
        toggleDuck(x, y - 1);
        toggleDuck(x, y + 1);
    }

    public void BuyUpgrade(int buttonNumber)
    {
        if (buttonNumber == CurrentUpgradeLvl+1)
        {
            int Upgradeprice = DuckUpgrades[CurrentUpgradeLvl].getPrice();//get the next upgrade's price
            if (Decibels >= Upgradeprice)//if the player can afford the upgrade
            {
                addDBs(-Upgradeprice);//negative value takes away DBs
                CurrentUpgradeLvl++;//player bought the next level
                HonkPower *= 2;//upgrading increases our honk power
                setButtonVisibility();//enables the next button based on our current upgrade level
                updateAllDuckSprites();//give our ducks the new look
            }
        }
    }

    private void updateAllDuckSprites()
    {
        for(int i=0; i<DuckObjects.Length; i++)
        {
            DuckObjects[i].GetComponent<Image>().sprite = UpgradeImages[CurrentUpgradeLvl];
        }
    }
}
