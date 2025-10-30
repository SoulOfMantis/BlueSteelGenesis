using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlueSteelGenesis.Character {
public abstract class Character : MonoBehaviour {
  public virtual void damage(int dmg) {
    currentHealth -= Math.Max(dmg, 1);
    if (current_health_ == 0) die();
  }
  public virtual void heal(int hp) {
    currentHealth += Math.Max(hp, 1);
  }

  abstract protected void die();

  public int currentHealth {
    get => current_health_;
    protected set =>
      current_health_ = Math.Clamp(value, 0, max_health_);
  }
  public int maxHealth {
    get => max_health_;
    protected set => max_health_ = value;
  }

  private int current_health_;
  private int max_health_;
}
}
