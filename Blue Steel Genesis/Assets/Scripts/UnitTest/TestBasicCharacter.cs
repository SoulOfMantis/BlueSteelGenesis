using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BlueSteelGenesis.Character;

public class TestBasicCharacter {
  [Test]
  public void BasicCallback() {
    GameObject go_ = new();
    BasicCharacter ch = go_.AddComponent<BasicCharacter>();

    Assert.AreEqual(100, ch.currentHealth);
    Assert.AreEqual(100, ch.maxHealth);

    int dmg_amount = 0, heal_amount = 0;
    bool dead = false;
    ch.subscribe(BasicCharacter.CallbackType.OnDamage, (ch_, dmg) => dmg_amount += (int)dmg);
    ch.subscribe(BasicCharacter.CallbackType.OnHeal, (ch_, hp) => heal_amount += (int)hp);
    ch.subscribe(BasicCharacter.CallbackType.OnDeath, (ch_, _) => dead = true);

    ch.heal(115);
    Assert.AreEqual(100, ch.currentHealth);
    Assert.AreEqual(0, dmg_amount);
    Assert.AreEqual(115, heal_amount);
    Assert.AreEqual(false, dead);

    ch.damage(99);
    Assert.AreEqual(1, ch.currentHealth);
    Assert.AreEqual(99, dmg_amount);
    Assert.AreEqual(115, heal_amount);
    Assert.AreEqual(false, dead);

    ch.heal(97);
    Assert.AreEqual(98, ch.currentHealth);
    Assert.AreEqual(99, dmg_amount);
    Assert.AreEqual(212, heal_amount);
    Assert.AreEqual(false, dead);

    ch.damage(98);
    Assert.AreEqual(0, ch.currentHealth);
    Assert.AreEqual(197, dmg_amount);
    Assert.AreEqual(212, heal_amount);
    Assert.AreEqual(true, dead);
  }
}
