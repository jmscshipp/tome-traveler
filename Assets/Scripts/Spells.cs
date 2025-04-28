using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Spells {
    Abundance = 0,
    Sleepless,
    Waterwalking,
    Mindreading,
    Clairvoyance,
    Teleportation
}

public abstract class Spell
{
    public int cooldown = 0;
    public int BaseCooldown = 3;
    public string Name = "Default Spell Name";
    public int Strength = 2;

    public static void PassTime()
    {
        foreach (Spell s in AllSpells)
        {
            s.ReduceCooldown();
            if (s is TimedSpell t)
                t.Use();
        }
    }

    public bool enabled = false;

    int MaxUses = 3;
    int UsesLeft = 0;

    protected static List<Spell> AllSpells = new List<Spell>();

    protected Spell()
    {
        AllSpells.Add(this);
    }

    public void ReduceCooldown()
    {
        cooldown = Math.Max(cooldown - 1, 0);
    }

    public abstract void Cast();
    public abstract Spells GetId();
}
public class Waterwalking : Spell
{
    public new string Name = "Waterwalking Spell";
    public new int BaseCooldown = 3;
    public override void Cast()
    {
        this.cooldown = BaseCooldown;
    }
    
    public override Spells GetId() {
        return Spells.Waterwalking;
    }
}

public class Abundance : Spell
{
    public override void Cast()
    {
        Player.Instance().GetComponent<PlayerResources>().AddHunger(-Strength);
        this.cooldown = BaseCooldown;
    }
    
    public override Spells GetId() {
        return Spells.Abundance;
    }
}

public class Sleepless : Spell
{
    public override void Cast()
    {
        Player.Instance().GetComponent<PlayerResources>().AddExhaustion(-Strength);
        this.cooldown = BaseCooldown;
    }

    public override Spells GetId()
    {
        return Spells.Sleepless;
    }

}

public class Clairvoyance : Spell
{
    public override void Cast()
    {
        // Change Fog of War size (lerp it?) by Strength
        this.cooldown = BaseCooldown;
    }

    public override Spells GetId()
    {
        return Spells.Clairvoyance;
    }
}

public class Teleportation : Spell
{
    public override void Cast()
    {
        // enter UI state machine
        // disable all actions except click and ESC to exit machine
        // highlight locations you can travel to
        // if you click, teleport and end state machine
        cooldown = BaseCooldown;
        Player.Instance().currentLocation.ActivateForTeleport(Strength);
        UIManager.Instance().CloseLocalePopup();
        UIManager.Instance().CloseDialoguePopup();
        UIManager.Instance().CloseShopUI();
    }
    public static void Cleanup()
    {
        foreach (MapLocation m in MapLocation.activeMindreadingLocations)
        {
            m.DeactivateForTeleport(isTraversable: false);
        }
        MapLocation.activeMindreadingLocations.Clear();
    }

    public override Spells GetId()
    {
        return Spells.Teleportation;
    }
}

public abstract class UsableSpell : Spell
{
    protected int UsesLeft = 0;
    protected int MaxUses = 3;

    public override void Cast()
    {
        cooldown = BaseCooldown;
        UsesLeft = MaxUses;
    }

    public virtual bool Use()
    {
        if (UsesLeft > 0)
        {
            UsesLeft -= 1;
            return true;
        }
        else
        {
            return false;
        }
    }
    
}

public abstract class TimedSpell : UsableSpell
{
    // This stub class exists for the PassTime function
    // Usable and Timed spells both have a limited number of charges
    // Usable spells don't spend a charge when time passes, but TimedSpells do
    // Examples of TimedSpells are Waterwalking, Mindreading, and Clairvoyance
}

public class Mindreading : TimedSpell
{
    // highlight locations with secrets to discover. Automatically succeed on Talk while spell lasts
    public static void Cleanup()
    {
        foreach (MapLocation m in MapLocation.activeMindreadingLocations)
        {
            m.DeactivateForMindreading();
        }
        MapLocation.activeMindreadingLocations.Clear();
    }

    public override Spells GetId()
    {
        return Spells.Mindreading;
    }

    public override void Cast()
    {
        base.Cast();
    }

    public override bool Use()
    {
        return base.Use();
    }
}