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

  public virtual void startTurn() {
    myTurn = true;
    currentEnergy = maxEnergy;
    // TODO: trigger modules
  }
  public virtual void endTurn() {
    myTurn = false;
    // TODO: trigger modules
  }



  public void move(int x, int y) => move(new Vector2Int(x, y));
  public void move(Vector2Int pos) {
    // TODO
  }
  public void strike(int x, int y, int dmg) => strike(new Vector2Int(x, y), dmg);
  public void strike(Vector2Int pos, int dmg) {
    // TODO
  }

  // TODO: public void addModule(/* smth */)
  // TODO: protected void triggerModule(/* smth */)
  // TODO: protected void triggerModules(TriggerType)



  public int currentHealth {
    get => current_health_;
    protected set =>
      current_health_ = Math.Clamp(value, 0, max_health_);
  }
  public int maxHealth {
    get => max_health_;
    protected set => max_health_ = value;
  }

  public int currentEnergy {
    get => current_energy_;
    protected set =>
      current_energy_ = Math.Clamp(value, 0, maxEnergy);
  }
  public int maxEnergy { get; protected set; }

  protected bool myTurn { get; private set; }



  private int current_health_;
  private int max_health_;

  private int current_energy_;
  // private List<Module> modules_;
}
}
