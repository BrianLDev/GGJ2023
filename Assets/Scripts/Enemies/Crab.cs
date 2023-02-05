using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : EnemyBase {
  [SerializeField] private float damage;

  protected new void Start() {
      base.Start();
  }

  protected void Update() {
    // TODO: MOVEMENT LOGIC AND SIMPLE AI
  }

  protected override void Die() {
    // TODO: DIE ANIMATION (IF THERE IS ONE)
    // TODO: PLAY SFX
    // TODO: GENERATE LOOT OR POWERUP
    Destroy(transform.gameObject, 0.3f);
  }

  public void OnCollisionEnter2D(Collision2D coll) {
    if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) {
      Player player = coll.gameObject.GetComponent<Player>();
      player.TakeDamage(damage);
      player.KnockBack((coll.transform.position - transform.position).normalized);
    }
  }
}
