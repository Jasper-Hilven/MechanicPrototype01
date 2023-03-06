using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MechanicPrototype1 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject buttonCenter;
    public BuildSquare buildsquarePrefab;
    public Dictionary<Tuple<int, int>, BuildSquare> buildSquares = new Dictionary<Tuple<int, int>, BuildSquare>();
    // GUI
    public TMP_Text title;
    public TMP_Text description;
    public TMP_Dropdown dropdown;
    public GameObject Build;
    public GameObject Deconstruct;
    public GameObject ToggleConversion;
    public TMP_Text InputRandomness;
    //State
    public BuildSquare selected;

    void Start()
    {
        BuildSquare center = null;
        for (int i = -4; i <= 4; i++)
        {
            for (int j = -4; j <= 4; j++)
            {
                var clone = UnityEngine.Object.Instantiate(buildsquarePrefab);
                clone.setIndices(i, j,this);
                buildSquares.Add(Tuple.Create(i, j), clone);
                clone.transform.parent = buttonCenter.transform;
                clone.transform.localPosition = new Vector3(i, j, 0);
                if(i == 0 && j == 0)
                {
                    center = clone;
                }

            }
        }
        center.building = new Building(BuildingType.Towncenter);
        center.resources = ResourceType.gold.n(300).add(ResourceType.activeHuman.n(4));
    }
    
    public void SquareButtonClicked()
    {

    }
    public void GUIButtonClicked()
    {
         
    }
    public void GUIButton2Clicked()
    {

    }


    public void GUIDropdownSelected()
    {
    }
    // Update is called once per frame
    void Update()
    {
        renderGUI();
    }

    void renderGUI()
    {

    }
}
