using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell
{
    public SpellData sd;
    public int cooldown = 0;
    public int BaseCooldown = 1;

    public Spell(SpellData sd)
    {
        this.BaseCooldown = sd.Cooldown;
    }
    public abstract void Use();
}
public class Waterwalking : Spell
{
    public Waterwalking() : base(sd: GameManager.Instance().Waterwalking) { }
    public override void Use()
    {
        this.cooldown = BaseCooldown;
    }
}

public class Abundance : Spell
{
    public SpellData sd = GameManager.Instance().Abundance;
    public Abundance() : base(sd: GameManager.Instance().Abundance) { }
    public override void Use()
    {
        Player.Instance().GetComponent<PlayerResources>().AddHunger(-sd.Strength);
        this.cooldown = BaseCooldown;
    }
}

public class Sleepless : Spell
{
    public Sleepless() : base(sd: GameManager.Instance().Sleepless) { }
    public override void Use()
    {
        Player.Instance().GetComponent<PlayerResources>().AddExhaustion(-sd.Strength);
        this.cooldown = BaseCooldown;
    }
}

public class Clairvoyance : Spell
{
    SpellData sd = GameManager.Instance().Clairvoyance;
    public Clairvoyance() : base(sd: GameManager.Instance().Clairvoyance) { }
    public override void Use()
    {
        // Change Fog of War size (lerp it?) by Strength
        this.cooldown = BaseCooldown;
    }
}

public class Teleportation : Spell
{
    SpellData sd = GameManager.Instance().Teleportation;
    public Teleportation() : base(sd: GameManager.Instance().Teleportation) { }
    public override void Use()
    {
        // enter UI state machine
        // disable all actions except click and ESC to exit machine
        // highlight locations you can travel to
        // if you click, teleport and end state machine
        this.cooldown = BaseCooldown;
    }
}

public class Mindreading : Spell
{
    SpellData sd = GameManager.Instance().Mindreading;
    public Mindreading() : base(sd:GameManager.Instance().Mindreading) {}
    public override void Use()
    {
        // highlight locations with secrets to discover. Automatically succeed on Talk in this location.
        this.cooldown = BaseCooldown;
    }
}