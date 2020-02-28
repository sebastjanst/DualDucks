using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class GridScript : MonoBehaviour
{
    public GameObject[] DuckObjects;
    //if you changed the grid size in the Unity editor:
    //lock the duckgrid in the top-right of the inspector, set the array elements to 0, then select and drag all the ducks onto the array name

    private GameObject[,] Ducks;//we'll save the precise locations of our ducks in the grid using this 2D array

    //set size of our duck grid here
    private int DuckColumns = 8;
    private int DuckRows = 7;

    public TextMeshProUGUI Dbtxt;
    private int Decibels = 0;

    private int ActiveDucks = 0;

    void Awake()//Awake happens before Start
    {
        CreateDuckGrid();//only use once to set our 2D array and give the ducks their IDs
        disableAllDucks();//makes them invisible and not clickable
        toggleDuck(DuckColumns / 2, DuckRows / 2);//enables a duck in the middle
        addDBs(0);//refreshes the text
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
        toggleDuck(x, y);

        toggleDuck(x - 1, y);
        toggleDuck(x + 1, y);
        toggleDuck(x, y - 1);
        toggleDuck(x, y + 1);

        addDBs(1*ActiveDucks);//gives one decibel for each active duck
    }
}
