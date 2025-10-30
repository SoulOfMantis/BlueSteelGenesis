using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlueSteelGenesis.Character {
public class BasicCharacter : MonoBehaviour {
  public virtual void damage(int dmg) {
    currentHealth -= dmg;
    emitCallback(CallbackType.OnDamage, dmg);
    if(current_health_ == 0) emitCallback(CallbackType.OnDeath);
  }
  public virtual void heal(int hp) {
    currentHealth += hp;
    emitCallback(CallbackType.OnHeal, hp);
  }



  BasicCharacter() {
    for(int i = 0; i < callback_.Length; ++i)
      callback_[i] = new();
  }
  public int currentHealth {
    get => current_health_;
    protected set =>
      current_health_ = Math.Clamp(value, 0, max_health_);
  }
  public int maxHealth {
    get => max_health_;
    protected set => max_health_ = value;
  }
  public Vector2Int position {
    get => position_;
    protected set {
      // TODO
    }
  }

  private int current_health_ = 100;
  private int max_health_ = 100;
  private Vector2Int position_;
  private List<Action<BasicCharacter, object>>[] callback_
      = new List<Action<BasicCharacter, object>>[(int)CallbackType.CALLBACK_SIZE];

  public enum CallbackType {
    OnDamage,
    OnDeath,
    OnHeal,
    // etc.
    CALLBACK_SIZE
  }

  public void subscribe(CallbackType event_id,
                        Action<BasicCharacter, object> callback) {
    callback_[(int)event_id].Add(callback);
  }
  private void emitCallback(CallbackType event_id, object info = null) {
    foreach (var cb in callback_[(int)event_id])
      cb(this, info);
  }
}
}
