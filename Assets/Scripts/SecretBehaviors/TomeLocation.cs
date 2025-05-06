using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomeLocation : SecretLocale
{
    [SerializeField]
    Items SpellTome = Items.None;

    public override void Activate()
    {
        Spell s = (SpellTome == Items.None) ? Player.RandomUnusedSpell() : Spell.AllSpells.Find(x => x.ItemId == SpellTome);
        // this is a hack. likelihood isn't used here
        new FindItem(likelihood: 0, new Tome(s)).Activate();
    }
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
