using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell
{
    public SpellData sd;
    public int cooldown = 0;
    public int BaseCooldown = 1;

    public void SetSpellData (SpellData sd)
    {
        this.sd = sd;
        this.BaseCooldown = sd.Cooldown;
    }

    public abstract void Use();
}
public class Waterwalking : Spell
{
    public override void Use()
    {
        SetSpellData(GameManager.Instance().Waterwalking);
        this.cooldown = BaseCooldown;
    }
}

public class Abundance : Spell
{
    public override void Use()
    {
        SetSpellData(GameManager.Instance().Abundance);
        Player.Instance().GetComponent<PlayerResources>().AddHunger(-sd.Strength);
        this.cooldown = BaseCooldown;
    }
}

public class Sleepless : Spell
{
    public override void Use()
    {
        SetSpellData(GameManager.Instance().Sleepless);
        Player.Instance().GetComponent<PlayerResources>().AddExhaustion(-sd.Strength);
        this.cooldown = BaseCooldown;
    }
}

public class Clairvoyance : Spell
{
    public override void Use()
    {
        SetSpellData(GameManager.Instance().Clairvoyance);
        // Change Fog of War size (lerp it?) by Strength
        this.cooldown = BaseCooldown;
    }
}

public class Teleportation : Spell
{
    public override void Use()
    {
        SetSpellData(GameManager.Instance().Teleportation);
        // enter UI state machine
        // disable all actions except click and ESC to exit machine
        // highlight locations you can travel to
        // if you click, teleport and end state machine
        this.cooldown = BaseCooldown;
    }
}

public class Mindreading : Spell
{
    public override void Use()
    {
        SetSpellData(GameManager.Instance().Mindreading);
        // highlight locations with secrets to discover. Automatically succeed on Talk in this location.
        this.cooldown = BaseCooldown;
    }
}