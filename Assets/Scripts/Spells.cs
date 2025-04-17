using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell
{
    public int cooldown = 0;
    public int BaseCooldown = 1;

    public Spell(int cooldown)
    {
        this.BaseCooldown = cooldown;
    }
    public abstract void Use();
}
public class Waterwalking : Spell
{
    SpellData sd = GameManager.Instance().Waterwalking;
    public Waterwalking() : base(cooldown: GameManager.Instance().Waterwalking.Cooldown) { }
    public override void Use()
    {
        this.cooldown = BaseCooldown;
    }
}

public class Abundance : Spell
{
    SpellData sd = GameManager.Instance().Abundance;
    public Abundance() : base(cooldown: GameManager.Instance().Abundance.Cooldown) { }
    public override void Use()
    {
        Player.Instance().GetComponent<PlayerResources>().AddHunger(-sd.Strength);
        this.cooldown = BaseCooldown;
    }
}

public class Sleepless : Spell
{
    SpellData sd = GameManager.Instance().Sleepless;
    public Sleepless() : base(cooldown: GameManager.Instance().Sleepless.Cooldown) { }
    public override void Use()
    {
        Player.Instance().GetComponent<PlayerResources>().AddExhaustion(-sd.Strength);
        this.cooldown = BaseCooldown;
    }
}

public class Clairvoyance : Spell
{
    SpellData sd = GameManager.Instance().Clairvoyance;
    public Clairvoyance() : base(cooldown: GameManager.Instance().Clairvoyance.Cooldown) { }
    public override void Use()
    {
        // Change Fog of War size (lerp it?) by Strength
        this.cooldown = BaseCooldown;
    }
}

public class Teleportation : Spell
{
    SpellData sd = GameManager.Instance().Teleportation;
    public Teleportation() : base(cooldown: GameManager.Instance().Teleportation.Cooldown) { }
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
    public Mindreading() : base(cooldown:GameManager.Instance().Mindreading.Cooldown) {}
    public override void Use()
    {
        // highlight locations with secrets to discover. Automatically succeed on Talk in this location.
        this.cooldown = BaseCooldown;
    }
}