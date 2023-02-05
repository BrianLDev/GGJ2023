using UnityEngine;
using EcxUtilities;

public class PowerupHealth : PowerupBase {
  [SerializeField] private float healAmt = 50f;

  private void Awake() {
    powerupType = PowerupType.Health;
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
      other.gameObject.GetComponent<Player>().Heal(healAmt);
      PlayPowerupSFX();
      Destroy(gameObject, .01f);
    }
  }

  protected override void PlayPowerupSFX() {
    AudioManager.Instance.PlayClip(AudioManager.Instance.SfxManager.Health, AudioCategory.Sfx, 7.0f);
  }
  
}