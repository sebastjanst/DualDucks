using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckID : MonoBehaviour
{
    int xPosition;
    int yPosition;
    GridScript gridScript;

    // Start is called before the first frame update
    void Start()
    {
        //finds the main grid handling script, so it can report clicks back to it
        gridScript = GetComponentInParent<GridScript>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            quack();
        }
    }

    public void setPositionIngrid(int x, int y)
    {
        xPosition = x;
        yPosition = y;
    }

    public void duckClicked()
    {
        gridScript.duckClicked(xPosition, yPosition);
    }

    public void quack()
    {
        GetComponent<Animator>().Play("DuckJump");
    }
}
