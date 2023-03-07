using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public TMP_Dropdown dropdownz;
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
                    selected = center;
                }
            }
        }
        center.building = new Building(BuildingType.Towncenter);
        center.resources = ResourceType.gold.n(300).add(ResourceType.activeHuman.n(4));
        dropdownz.ClearOptions();
        dropdownz.AddOptions(BuildingType.all.Select(buildingType => new TMP_Dropdown.OptionData() { text = buildingType.name}).ToList());
    }
    
    public void SquareButtonClicked(BuildSquare clicker)
    {
        selected = clicker;
    }

    public void BuildButtonClicked()
    {
        selected.build(BuildingType.all[dropdownz.value]);
    }

    public void ToggleActionButtonClicked()
    {
        selected.building.intendDoingConversion = !selected.building.intendDoingConversion;
    }

    public void DeconstructButtonClicked()
    {
        selected.downgrade();
    }

    // Update is called once per frame
    void Update()
    {
        renderGUI();
    }

    void renderGUI()
    {
        if(selected.building == null)
        {
            title.text = "Buildground " + selected.i + "," + selected.j;
            description.text = "";
        }
        else
        {
            title.text = selected.building.type.name +" "+ selected.i + "," + selected.j;
            description.text = "intend: " + selected.building.intendDoingConversion;
            description.text += "\r\n isdoing: " + selected.building.isDoingConversion;
            description.text += "\r\n conversionstate: " + selected.building.conversionState;
            description.text += "\r\n main: " + selected.building.type.mainConversion.name;
        }
        description.text += "\r\n" + selected.resources.ToReadableString();
        Deconstruct.SetActive(selected.canDowngrade());
        Build.SetActive(selected.canBuild(BuildingType.all[dropdownz.value]));
        ToggleConversion.SetActive(selected.building != null);
        dropdownz.gameObject.SetActive(selected.building == null);
    }
}
