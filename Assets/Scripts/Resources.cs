using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Resources
{
    private Dictionary<ResourceType, long> amounts;

    public Resources()
    {
        this.amounts = new Dictionary<ResourceType, long>();
    }
    public Resources(Dictionary<ResourceType, long> amounts)
    {
        this.amounts = amounts;
    }
    public static Resources orEmpty(Resources r)
    {
        if (r == null)
        {
            return Resources.r();
        }
        return r;
    }

    public bool isSame(Resources other)
    {
        if(other.amounts.Count != amounts.Count)
        {
            return false;
        }
        foreach (KeyValuePair<ResourceType, long> entry in amounts)
        {
            long valueOther = long.MinValue;
            other.amounts.TryGetValue(entry.Key, out valueOther);
            if (valueOther != entry.Value) return false;
        }
        return true;
    }
    public Resources multiply(float amount)
    {
        var ret = new Dictionary<ResourceType, long>();
        foreach (var entry in amounts)
            ret.Add(entry.Key, (long)Mathf.Round(entry.Value * amount));
        return new Resources(ret);
    }
    public List<ResourceType> types()
    {
        return amounts.Keys.ToList();
    }
    public List<ResourceType> typesAlphaBetically()
    {
        return new List<ResourceType>(amounts.Keys.ToList().OrderBy((e) => e.name));
    }
    public Resources EachResourceTypeOnce()
    {
        var ret = new Dictionary<ResourceType, long>();
        foreach (var entry in amounts)
            ret.Add(entry.Key, 1);
        return new Resources(ret);
    }
    public Resources subtract(Resources toSubtract)
    {
        var ret = new Resources(new Dictionary<ResourceType, long>());
        foreach (var entry in types())
            ret.setAmount(entry, getAmountOf(entry));
        foreach (var entry in toSubtract.types())
            ret.setAmount(entry, getAmountOf(entry) - toSubtract.getAmountOf(entry));
        return ret;
    }
    public Resources common(Resources other)
    {
        var ret = new Resources(new Dictionary<ResourceType, long>());
        foreach (var entry in types())
            ret.setAmount(entry, Math.Min(getAmountOf(entry), other.getAmountOf(entry)));
        return ret;
    }

    public Resources addForEach(int amount)
    {
        var ret = new Resources(new Dictionary<ResourceType, long>());
        foreach (var entry in types())
            ret.setAmount(entry, getAmountOf(entry) + amount);
        return ret;
    }
    public Resources add(Resources toAdd)
    {
        var ret = new Resources(new Dictionary<ResourceType, long>());
        foreach (var entry in types())
            ret.setAmount(entry, getAmountOf(entry));
        foreach (var entry in toAdd.types())
            ret.setAmount(entry, getAmountOf(entry) + toAdd.getAmountOf(entry));
        return ret;
    }
    private void setAmount(ResourceType t, long amount)
    {
        if (amount == 0) amounts.Remove(t); else amounts[t] = amount;
    }

    public string ToReadableString()
    {
        string ret = "";

        var myTypes = types();
        for (int i = 0; i < myTypes.Count; i++)
        {
            var type = myTypes[i];
            ret = ret + getAmountOfTypeInText(type) + " " + type.name;
            if (i == myTypes.Count - 2)
            {
                ret = ret + " and ";
            }
            if (i < myTypes.Count - 2)
            {
                ret = ret + ", ";
            }
        }
        return ret;
    }

    public bool isEmpty()
    {
        return new Resources().contains(this);
    }
    public bool contains(Resources toContain)
    {
        foreach (var rT in toContain.types())
        {
            if (getAmountOf(rT) < toContain.getAmountOf(rT))
                return false;
        }
        return true;
    }
    public long getAmountOf(ResourceType rT)
    {
        return amounts.ContainsKey(rT) ? amounts[rT] : 0;
    }
    public string getAmountOfT(ResourceType rT)
    {
        return rT.name + ": " + getAmountOfTypeInText(rT);
    }
    public string getAmountOfTypeInText(ResourceType rT)
    {
        long amount = getAmountOf(rT);
        return getAmountInText(amount);
    }
    public static string getAmountInText(long amount)
    {
        if (amount < 1000L)
            return "" + amount;
        int logNumber = Mathf.FloorToInt((Mathf.Log10(amount) - 1) / 3);
        logNumber = logNumber >= 0 ? logNumber : 0;
        string suffix = new string[] { "", "K", "M", "G", "T", "E" }[logNumber];
        while (logNumber > 0)
        {
            amount = amount / 1000;
            logNumber--;
        }
        return amount + suffix;
    }
    public static Resources r()
    {
        return new Resources(new Dictionary<ResourceType, long>());
    }
    public static Resources r(ResourceType t0)
    {
        return r(t0, 1);
    }
    public static Resources r(ResourceType t0, long amount0)
    {
        return new Resources(new Dictionary<ResourceType, long>() { { t0, amount0 } });
    }
    public static Resources r(ResourceType t0, long amount0, ResourceType t1, long amount1)
    {
        return new Resources(new Dictionary<ResourceType, long>() { { t0, amount0 }, { t1, amount1 } });
    }
    public static Resources r(ResourceType t0, long amount0, ResourceType t1, long amount1, ResourceType t2, long amount2)
    {
        return new Resources(new Dictionary<ResourceType, long>() { { t0, amount0 }, { t1, amount1 }, { t2, amount2 } });
    }
    public static Resources r(ResourceType t0, long amount0, ResourceType t1, long amount1, ResourceType t2, long amount2, ResourceType t3, long amount3)
    {
        return new Resources(new Dictionary<ResourceType, long>() { { t0, amount0 }, { t1, amount1 }, { t2, amount2 }, { t3, amount3 } });
    }
    public static Resources r(ResourceType t0, long amount0, ResourceType t1, long amount1, ResourceType t2, long amount2, ResourceType t3, long amount3, ResourceType t4, long amount4)
    {
        return new Resources(new Dictionary<ResourceType, long>() { { t0, amount0 }, { t1, amount1 }, { t2, amount2 }, { t3, amount3 }, { t4, amount4 } });
    }
    public static Resources r(ResourceType t0, long amount0, ResourceType t1, long amount1, ResourceType t2, long amount2, ResourceType t3, long amount3, ResourceType t4, long amount4, ResourceType t5, long amount5)
    {
        return new Resources(new Dictionary<ResourceType, long>() { { t0, amount0 }, { t1, amount1 }, { t2, amount2 }, { t3, amount3 }, { t4, amount4 } , { t5, amount5 } });
    }

}
public class ResourceType
{
    public string name;
    public static ResourceType gold = new ResourceType() { name = "gold" };
    public static ResourceType activeHuman = new ResourceType() { name = "activeHuman" };
    public static ResourceType tiredHuman = new ResourceType() { name = "tiredHuman" };
    public static ResourceType dish = new ResourceType() { name = "dish" };
    public static ResourceType meal = new ResourceType() { name = "meal" };
    public static ResourceType meat = new ResourceType() { name = "meat" };
    public Resources n(int amount)
    {
        return Resources.r(this, amount);
    }
}
