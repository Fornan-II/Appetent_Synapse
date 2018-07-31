using System.Collections.Generic;

//A modifier is effectively a KeyValuePair intended to be exclusively used by the ModifierTable class.
public class Modifier
{
    public Modifier(float v, uint k)
    {
        value = v;
        key = k;
    }

    //The value is whatever value we want to store in this Modifier
    public float value;

    //The key is for tracking specific modifiers. Useful for removing them later.
    public uint key;
}

//This class is intended to make dealing with multiple modifiers easier. Tracking where the modifiers are from, and what they are.
//Also has some useful functions for getting values out of the table.
public class ModifierTable
{    
    //The table of modifiers. Also tracks what actors are responsible for applying the modifiers.
    private List<KeyValuePair<Actor, Modifier>> modifierTable;

    //By using keyCounter, we ensure all modifiers are given unique keys.
    protected uint keyCounter;

    //Initialization
    public ModifierTable()
    {
        modifierTable = new List<KeyValuePair<Actor, Modifier>>();
        keyCounter = 0;
    }

    //Add a new modifier to the ModifierTable.
    public virtual int Add(float newValue, Actor source)
    {
        modifierTable.Add(new KeyValuePair<Actor, Modifier>(source, new Modifier(newValue, keyCounter)));
        keyCounter++;
        return (int)keyCounter - 1;
    }

    //Remove all modifiers given by a particular source.
    public virtual void Remove(Actor source)
    {
        for(int i = 0; i < modifierTable.Count; i++)
        {
            if(modifierTable[i].Key == source)
            {
                modifierTable.RemoveAt(i);
            }
        }
    }

    //Remove a specific modifier from a particular source.
    public virtual void Remove(Actor source, int key)
    {
        for (int i = 0; i < modifierTable.Count; i++)
        {
            if (modifierTable[i].Key == source && modifierTable[i].Value.key == key)
            {
                modifierTable.RemoveAt(i);
            }
        }
    }
    
    //Checks to see if the specified key is still in the modifierTable
    public virtual bool KeyIsActive(int key)
    {
        if(key < 0 || key > keyCounter) //Ignore trivial cases (keys that can't be used and keys that haven't been used yet)
        {
            return false;
        }

        for(int i = 0; i < modifierTable.Count; i++)
        {
            if(modifierTable[i].Value.key == key)
            {
                return true;
            }
        }

        return false;
    }

    //Get a list of all actors being tracked by the table.
    public virtual Actor[] GetModifyingActors()
    {
        List<Actor> l = new List<Actor>();

        for(int i = 0; i < modifierTable.Count; i++)
        {
            l.Add(modifierTable[i].Key);
        }

        return l.ToArray();
    }

    //Return the sum of all modifiers in the table
    public virtual float Sum()
    {
        float sum = 0.0f;

        for (int i = 0; i < modifierTable.Count; i++)
        {
            sum += modifierTable[i].Value.value;
        }

        return sum;
    }

    //Return the product of all modifiers in the table
    public virtual float Product()
    {
        float product = 1.0f;

        for (int i = 0; i < modifierTable.Count; i++)
        {
            product *= modifierTable[i].Value.value;
        }

        return product;
    }

    //Return the highest value present in the table.
    public virtual float Max()
    {
        if(modifierTable.Count == 0)
        {
            return float.NaN;
        }

        float max = modifierTable[0].Value.value;

        for (int i = 1; i < modifierTable.Count; i++)
        {
            float temp = modifierTable[i].Value.value;
            if (temp > max)
            {
                max = temp;
            }
        }

        return max;
    }

    //Return the lowest value present in the table.
    public virtual float Min()
    {
        if (modifierTable.Count == 0)
        {
            return float.NaN;
        }

        float min = modifierTable[0].Value.value;

        for (int i = 1; i < modifierTable.Count; i++)
        {
            float temp = modifierTable[i].Value.value;
            if (temp < min)
            {
                min = temp;
            }
        }

        return min;
    }

    //Return the average value of the table.
    public virtual float Mean()
    {
        return Sum() / modifierTable.Count;
    }
}
