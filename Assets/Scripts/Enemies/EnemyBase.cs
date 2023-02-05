using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
  [SerializeField] protected float maxHealth = 100f;
  protected float health;
  public float Health => health;

  protected void Start() {
    health = maxHealth;
  }


  public void TakeDamage(float amt) {
    Debug.Log(this.name + " taking damage! " + amt);
    health -= amt;
    PlayHurtSFX();
    if (health <= 0)
      Die();
  }

  public void Heal(float amt) {
    health += amt;
    if (health > maxHealth)
      health = maxHealth;
  }

  protected abstract void PlayHurtSFX();
  protected abstract void PlayDieSFX();
  protected abstract void Die();
  
}
