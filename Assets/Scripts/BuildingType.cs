using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingType
{
    public Conversion mainConversion;
    public string name;
    public Resources constructionCost;
    public bool canDeconstruct;

    public Resources getDeconstructionDrop()
    {
        return constructionCost.multiply(.5f);
    }
    public BuildingType(Conversion mainConversion, string name, Resources constructionCost, bool canDeconstruct)
    {
        this.mainConversion = mainConversion;
        this.name = name;
        this.constructionCost = constructionCost;
        this.canDeconstruct = canDeconstruct;
    }
    public static BuildingType ReproductionCenter = new BuildingType(Conversion.reproduce, "Reproduction center", ResourceType.gold.n(10),true);
    public static BuildingType NormalHouse = new BuildingType(Conversion.sleep, "Normal house", ResourceType.gold.n(10), true);
    public static BuildingType FancyHouse = new BuildingType(Conversion.eat, "Fancy house", ResourceType.gold.n(10).add(ResourceType.dish.n(5)), true);
    public static BuildingType HuntingHouse = new BuildingType(Conversion.hunt, "Hunting hut", ResourceType.gold.n(30), true);
    public static BuildingType Granary = new BuildingType(Conversion.prepMeals, "Granary", ResourceType.gold.n(30), true);
    public static BuildingType Bazaar = new BuildingType(Conversion.prepDishes, "Bazaar", ResourceType.gold.n(30), true);
    public static BuildingType Towncenter = new BuildingType(Conversion.idle, "Town center", ResourceType.gold.n(30), false);
    public static List<BuildingType> all = new List<BuildingType>() { ReproductionCenter, NormalHouse, FancyHouse, HuntingHouse, Granary, Bazaar, Towncenter };
}
