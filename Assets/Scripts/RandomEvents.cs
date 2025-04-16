using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RandomEvent
{
    public int likelihood = 1;
    public abstract void Activate();

    public RandomEvent (int likelihood)
    {
        this.likelihood = likelihood;
    }
}

public class RandomTable
{
    List<RandomEvent> events;

    public RandomTable (List<RandomEvent> events)
    {
        this.events = events;
    }

    public RandomEvent ChooseRandom()
    {
        List<RandomEvent> choices = new List<RandomEvent>();
        foreach (RandomEvent r in events)
        {
            for (int i=0;i<r.likelihood;i++)
            {
                choices.Add(r);
            }
        }
        return choices[Random.Range(0, choices.Count)];
    }
}

public class NothingHappens : RandomEvent
{
    public NothingHappens(int likelihood) : base(likelihood) { }

    public override void Activate()
    {
        // Nothing Happens
    }
}
public class GetRobbed : RandomEvent
{
    public GetRobbed (int likelihood) : base(likelihood) {}

    public override void Activate()
    {
        Debug.LogError("You got robbed but it wasn't implemented");
    }
}

public class GetConcussed : RandomEvent
{
    public GetConcussed(int likelihood) : base(likelihood) { }

    public override void Activate()
    {
        Debug.LogError("You got concussed but it wasn't implemented");
    }
}