using UnityEngine;

public class PowerupHealth : PowerupBase {
  [SerializeField] private float healAmt = 50f;

  private void Awake() {
    powerupType = PowerupType.Health;
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
      other.gameObject.GetComponent<Player>().Heal(healAmt);
      // TODO: PLAY SFX
      Destroy(gameObject, .01f);
    }
  }
  
}