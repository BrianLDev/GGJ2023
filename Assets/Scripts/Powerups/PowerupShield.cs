using UnityEngine;

public class PowerupShield : PowerupBase {
  [SerializeField] private float shieldAmt = 25f;

  private void Awake() {
    powerupType = PowerupType.Health;
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
      other.gameObject.GetComponent<Player>().ShieldChange(shieldAmt);
      // TODO: PLAY SFX
      Destroy(gameObject, .01f);
    }
  }
  
}