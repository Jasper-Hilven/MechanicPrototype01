using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSquare : MonoBehaviour
{
    public int i;
    public int j;
    private MechanicPrototype1 grid;
    public Building building; // Can be null
    public Resources resources = new Resources();
    internal void setIndices(int i, int j, MechanicPrototype1 prototype)
    {
        this.i = i;
        this.j = j;
        this.grid = prototype;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Canvas>().worldCamera = FindObjectOfType<Camera>();
    }
    public void Update()
    {
        if(canStartConversionOfBuilding())
        {
            takeResourcesAround(3, building.type.mainConversion.from);
            building.isDoingConversion = true;
            building.conversionState = 0f;
        }
        if(building.isDoingConversion)
        {
            building.conversionState+= Time.deltaTime / building.type.mainConversion.duration;
            if(building.conversionState >= 1f)
            {
                dropResources(building.type.mainConversion.to);
                building.isDoingConversion = false;
                building.conversionState = 0f;
            }
        }
    }

    public List<BuildSquare> getSurrounding(int range)
    {
        var ret = new List<BuildSquare>();
        for (int k = 0; k <= 2 * range; k++)
        {

            for (int i = -range; i <= range; i++)
            {
                for (int j = -range; j <= range; j++)
                {
                    if (Math.Abs(k) != Mathf.Abs(i) + Math.Abs(j)) continue;
                    if (i * i + j * j > range * range) continue;
                    Tuple<int, int> checkPos = new Tuple<int, int>(this.i + i, this.j + j);
                    if (grid.buildSquares.ContainsKey(checkPos))
                    {
                        ret.Add(grid.buildSquares[checkPos]);
                    }
                }

            }
        }
        return ret;
    }
    public bool hasResourcesAround(int range, Resources resources)
    {
        Resources total = Resources.r();
        var surrounding = getSurrounding(range);
        surrounding.ForEach(s => total = total.add(s.resources));
        return total.contains(resources);
    } 
    public void takeResourcesAround(int range, Resources resources)
    {
        Resources totalToTake = resources;
        var surrounding = getSurrounding(range);
        for (int i = 0; i < surrounding.Count; i++)
        {
            var toTakeFrom = surrounding[i];
            var common = totalToTake.common(toTakeFrom.resources);
            totalToTake = totalToTake.subtract(common);
            toTakeFrom.resources = toTakeFrom.resources.subtract(common);
        }
        if(!totalToTake.isEmpty())
        {
            throw new Exception();
        }
    }
    public bool canStartConversionOfBuilding()
    {
        if (building == null) return false;
        if (building.isDoingConversion) return false;
        if(!building.intendDoingConversion) return false;
        return hasResourcesAround(3, building.type.mainConversion.from);
    }
    public bool canBuild(BuildingType buildingType)
    {
        if (building != null) return false;
        return hasResourcesAround(6, buildingType.constructionCost);
    }
    public bool canDowngrade()
    {
        if (building == null) return false;
        if (!building.type.canDeconstruct) return false;
        if (building.isDoingConversion) return false;
        return true;
    }
    public void build(BuildingType buildingType)
    {
        if (!canBuild(buildingType)) throw new Exception();
        takeResourcesAround(6, buildingType.constructionCost);
        building = new Building(buildingType);
    }
    public void downgrade()
    {
        if(!canDowngrade()) throw new Exception();
        dropResources(building.type.getDeconstructionDrop());
        building = null;

    }
    public void dropResources(Resources r)
    {
        this.resources = this.resources.add(r);
    }
}
public class Building
{
    public BuildingType type;
    public BuildSquare onwhich;
    public bool isDoingConversion;
    public bool intendDoingConversion;
    public float conversionState;

    public Building(BuildingType type)
    {
        this.type = type;

    }

}
public class Conversion
{
    public Resources from;
    public Resources to;
    public string name;
    public float duration;
    public Conversion(Resources from, Resources to, string name, float duration)
    {
        this.from = from;
        this.to = to;
        this.name = name;
        this.duration = duration;
    }
    public static Conversion idle = new Conversion(Resources.r(), Resources.r(), "idle", 20);
    public static Conversion reproduce = new Conversion(ResourceType.activeHuman.n(2), ResourceType.tiredHuman.n(3), "reproduce", 20);
    public static Conversion sleep = new Conversion(ResourceType.tiredHuman.n(1).add(ResourceType.gold.n(1)),
        ResourceType.activeHuman.n(1), "sleep", 5);
    public static Conversion eat = new Conversion(
        ResourceType.tiredHuman.n(2).add(ResourceType.gold.n(1)).add(ResourceType.dish.n(2)),
        ResourceType.activeHuman.n(2), "eat", 8);
    public static Conversion hunt = new Conversion(
        ResourceType.activeHuman.n(1),
        ResourceType.tiredHuman.n(1).add(ResourceType.meat.n(1)), "hunt", 10);
    public static Conversion prepMeals = new Conversion(
        ResourceType.activeHuman.n(1).add(ResourceType.meat.n(1)),
        ResourceType.tiredHuman.n(1).add(ResourceType.meal.n(4)), "prepare meals", 10);
    public static Conversion prepDishes = new Conversion(
        ResourceType.activeHuman.n(1).add(ResourceType.meal.n(1)),
        ResourceType.tiredHuman.n(1).add(ResourceType.dish.n(4)), "prepare dishes", 10);

}