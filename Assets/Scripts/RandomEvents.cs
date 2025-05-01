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
        int amount = Random.Range(1, Player.Instance().GetComponent<PlayerResources>().GetCoins());
        Player.Instance().GetComponent<PlayerResources>().AddCoins(-amount);
        UIManager.Instance().OpenDialoguePopup("You were robbed. You lost " + amount + " gold. Be happy you kept your life.");
        //Debug.LogError("You got robbed but it wasn't implemented");
    }
}

public class GetConcussed : RandomEvent
{
    public GetConcussed(int likelihood) : base(likelihood) { }

    public override void Activate()
    {

        List<Spell> pspells = Player.Instance().GetSpells();
        if (pspells.Count == 0)
        {
            UIManager.Instance().OpenDialoguePopup("Bandits fall upon you, but you fight back. In the ensuing chaos, you are struck and fall unconscious. When you awake, you can't remember something important.");
        }
        else
        {
            Spell forgottenSpell = Player.Instance().LoseRandomSpell();
            UIManager.Instance().OpenDialoguePopup("Bandits fall upon you, but you fight back. In the ensuing chaos, you are struck and fall unconscious. When you awake, you can't remember " + forgottenSpell.Name);
        }
    }
}

class FindItem : RandomEvent
    {
        Item item;

        public FindItem(int likelihood, Item item) : base(likelihood) {
            this.item = item;
        }

        public override void Activate()
        {

            UIManager.Instance().OpenDialoguePopup($"It's your lucky day-- you found {item.IndefiniteArticle()}!");

        // Read spells immediately
        // TODO: should we do this?
        if (item is Tome t)
        {
            t.Spell.enabled = true;
        }
        else
        {
            Player.Instance().PlayerInventory.AddItem(item);
        }
    }
}