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

    public bool enabled = false;

    public void ReduceCooldown()
    {
        cooldown = Math.Max(cooldown-1, 0);
    }

    public abstract void Use();
    public abstract Spells GetId();
}
public class Waterwalking : Spell
{
    public new string Name = "Waterwalking Spell";
    public new int BaseCooldown = 3;
    public override void Use()
    {
        this.cooldown = BaseCooldown;
    }
    
    public override Spells GetId() {
        return Spells.Waterwalking;
    }
}

public class Abundance : Spell
{
    public override void Use()
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
    public override void Use()
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
    public override void Use()
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

    public override void Use()
    {
        // enter UI state machine
        // disable all actions except click and ESC to exit machine
        // highlight locations you can travel to
        // if you click, teleport and end state machine
        this.cooldown = BaseCooldown;
        Player.Instance().currentLocation.ActivateForTeleport(Strength);
        UIManager.Instance().CloseLocalePopup();
        UIManager.Instance().CloseDialoguePopup();
        UIManager.Instance().CloseShopUI();
    }

        public override Spells GetId()
    {
        return Spells.Teleportation;
    }
}

public class Mindreading : Spell
{
    public override void Use()
    {
        // highlight locations with secrets to discover. Automatically succeed on Talk in this location.
        this.cooldown = BaseCooldown;
    }

    public override Spells GetId()
    {
        return Spells.Mindreading;
    }
}