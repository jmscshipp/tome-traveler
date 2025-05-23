using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this list MUST match the AllSpells list
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
    // this list MUST match the Spells enum
    public static readonly List<Spell> AllSpells = new List<Spell>() {
        new Abundance(),
        new Sleepless(),
        new Waterwalking(),
        new Mindreading(),
        new Clairvoyance(),
        new Teleportation(),
};
    public int cooldown = 0;
    public int BaseCooldown = 3;
    public virtual string Name => "Default Spell Name";
    protected virtual int Strength => 2;

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
    public void ReduceCooldown()
    {
        cooldown = Math.Max(cooldown - 1, 0);
    }
    public abstract Items ItemId { get; }
    public abstract void Cast();
    public abstract Spells GetId();
}

public class Abundance : Spell
{
    public override string Name => "Abundance Spell";
    public override void Cast()
    {
        Player.Instance().GetComponent<PlayerResources>().AddHunger(-Strength);
        this.cooldown = BaseCooldown;
    }

    public override Spells GetId()
    {
        return Spells.Abundance;
    }

    public override Items ItemId { get => Items.AbundanceTome;  }
}

public class Sleepless : Spell
{
    public override string Name => "Sleepless Spell";
    public override void Cast()
    {
        Player.Instance().resources.AddExhaustion(-Strength);
        this.cooldown = BaseCooldown;
    }

    public override Spells GetId()
    {
        return Spells.Sleepless;
    }
    public override Items ItemId { get => Items.SleeplessTome;  }

}

public class Clairvoyance : TimedSpell
{
    public override string Name => "Clairvoyance Spell";
    private static float _defaultFogOfWarRadius = -1f;
    protected override int Strength => 5;
    public override void Cast()
    {
        if (_defaultFogOfWarRadius <= 0)
        {
            _defaultFogOfWarRadius = GameManager.GetFogOfWarRadius();
        }
        // Change Fog of War size (lerp it?) by Strength
        this.cooldown = BaseCooldown;
        GameManager.UpdateFogOfWarRadius(_defaultFogOfWarRadius + Strength);
    }

    public override bool Use()
    {
        bool res = base.Use();
        // If no uses left, cleanup
        if (!res)
        {
            Cleanup();
        }
        return res;
    }

    private static void Cleanup()
    {
        if (_defaultFogOfWarRadius > 0f)
        {
            GameManager.UpdateFogOfWarRadius(_defaultFogOfWarRadius);
        }
    }
    
    public override Spells GetId()
    {
        return Spells.Clairvoyance;
    }
    public override Items ItemId { get => Items.ClairvoyanceTome;  }
}

public class Teleportation : Spell
{

    public override string Name => "Teleportation Spell";
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
        MapLocation.activeTeleportLocations.ForEach(x => x.DeactivateForTeleport(isTraversable: false));
        MapLocation.activeTeleportLocations.Clear();
    }

    public override Spells GetId()
    {
        return Spells.Teleportation;
    }
    public override Items ItemId { get => Items.TeleportationTome;  }
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
    
    public bool HasCharges()
    {
        return UsesLeft > 0;
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


    public override string Name => "Mindreading Spell";
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
        // This check is necessary because Use is called whenever time passes
        // We have to use check result of .Use() to see if we have uses left
        bool res = base.Use();
        if (res)
            Player.Instance().currentLocation.ActivateForMindreading(Strength);
        return res;
    }
    public override Items ItemId { get => Items.MindreadingTome;  }
}

public class Waterwalking : TimedSpell
{
    public override string Name => "Waterwalking Spell";
    public new int BaseCooldown = 3;
    public static void Cleanup()
    {

    }

    public override Spells GetId()
    {
        return Spells.Waterwalking;
    }

    public override void Cast()
    {
        base.Cast();
    }

    public override bool Use()
    {
        return base.Use();
    }
    public override Items ItemId { get => Items.WaterwalkingTome;  }
}