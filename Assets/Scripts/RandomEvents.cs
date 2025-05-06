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
    private bool ShowDialogue = false;
    public NothingHappens(int likelihood, bool ShowDialogue = false) : base(likelihood) {
        this.ShowDialogue = ShowDialogue;
    }

    public override void Activate()
    {
        if (ShowDialogue)
            UIManager.Instance().OpenDialoguePopup("You search to the point of exhaustion, but there is nothing to find.");
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

public class FindItem : RandomEvent
{
    Item item;

    public FindItem(int likelihood, Item item) : base(likelihood)
    {
        this.item = item;
    }

    public override void Activate()
    {

        UIManager.Instance().OpenDialoguePopup($"It's your lucky day-- you found {item.IndefiniteArticle()}!");

        Player.Instance().PlayerInventory.AddItem(item);
    }
}

public class GiveSupplies : RandomEvent
{
    public GiveSupplies(int likelihood) : base(likelihood)
    {
    }

    public override void Activate()
    {
        int num_tents;
        int num_food;
        do
        {
            num_tents = Random.Range(0, 2);
            num_food = Random.Range(0, 3);
        } while (num_food + num_tents == 0);
        string tents_plural = (num_tents > 1) ? "s" : "";
        string dialogue = "You shouldn't ever see this dialogue option!";
        if (num_food > 0 && num_tents > 0)
        {
            dialogue = $"It's your lucky day-- you got {num_food} food and {num_tents} tent{tents_plural}!"; ;
        }
        else if (num_food > 0)
        {
            dialogue = $"It's your lucky day-- you got {num_tents} tent{tents_plural}!"; ;
        }
        else if (num_tents > 0)
        {
            dialogue = $"It's your lucky day-- you got {num_food} food!";
        }
        for (int i = 0; i < num_tents; i++)
        {
            Player.Instance().PlayerInventory.AddItem(new Tent());
        }
        for (int i = 0; i < num_food; i++)
        {
            Player.Instance().PlayerInventory.AddItem(new Food());
        }
        UIManager.Instance().OpenDialoguePopup(dialogue);
    }
}
public class GiveRandomSecret : RandomEvent
{
    public GiveRandomSecret(int likelihood) : base(likelihood)
    {
    }

    public override void Activate()
    {
        UIManager.Instance().OpenDialoguePopup("The NPC teaches you a random secret!");
        UIManager.Instance().QueueActionAfterPopup(() => Secret.ActivateRandomSecretLocale());
    }

}