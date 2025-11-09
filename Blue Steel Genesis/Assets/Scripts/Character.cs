using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class Character : MonoBehaviour {
  public bool myTurn = false;
  public static InitiativeTracker Tracker;
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
        Tracker.StartNextTurn();

    // TODO: trigger modules
  }



  public void move(int x, int y, int z) => move(new Vector3Int(x, y, z));
  public void move(Vector3Int pos) {
    // TODO
  }
  public void strike(int x, int y, int z, int dmg) => strike(new Vector3Int(x, y, z), dmg);
  public void strike(Vector3Int pos, int dmg) {
    // TODO
  }

  // TODO: public void addModule(/* smth */)
  // TODO: protected void triggerModule(/* smth */)
  // TODO: protected void triggerModules(TriggerType)



  public int currentHealth {
    get => current_health_;
    protected set =>
      current_health_ = Math.Clamp(value, 0, maxHealth);
  }
  public int maxHealth { get; protected set; }

  public int currentEnergy {
    get => current_energy_;
    protected set =>
      current_energy_ = Math.Clamp(value, 0, maxEnergy);
  }
  public int maxEnergy { get; protected set; }


   // protected bool myTurn { get; private set; }


    private int current_health_;
  private int current_energy_;
  // private List<Module> modules_;
}

