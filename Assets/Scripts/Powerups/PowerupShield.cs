using UnityEngine;
using EcxUtilities;

public class PowerupShield : PowerupBase {
  [SerializeField] private float shieldAmt = 25f;

  private void Awake() {
    powerupType = PowerupType.Health;
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
      other.gameObject.GetComponent<Player>().ShieldChange(shieldAmt);
      PlayPowerupSFX();
      Destroy(gameObject, .01f);
    }
  }

  protected override void PlayPowerupSFX() {
    AudioManager.Instance.PlayClip(AudioManager.Instance.SfxManager.Shield, AudioCategory.Sfx);
  }
  
}