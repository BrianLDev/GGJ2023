using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EcxUtilities;

public class Jumper : EnemyBase {
  public enum JumperState { Idle, JumpLeft, JumpRight }

  [SerializeField] private float damage = 20f;
  [SerializeField] private float jumpForce = 4f;
  [SerializeField] private float stateDuration = 1f;
  private JumperState jumperState = JumperState.Idle;
  private float stateTimer = 0;
  private Rigidbody2D rb;
  private Animator animator;
  private SpriteRenderer spriteRenderer;

  protected void Awake() {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponentInChildren<Animator>();
    spriteRenderer = GetComponentInChildren<SpriteRenderer>();
  }

  protected new void Start() {
      base.Start();
  }

  protected void Update() {
    // choose state
    stateTimer -= Time.deltaTime;
    if (stateTimer <= 0) {
      stateTimer = stateDuration * Random.Range(0.5f, 1.5f);
      int random = Random.Range(0, 3);
      ChangeStates((JumperState)random);
    }
    // flip facing up/down
    if (rb.velocity.y > 0.1f)
      spriteRenderer.flipY = true;
    else
      spriteRenderer.flipY = false;
  }

  protected void ChangeStates(JumperState newState) {
    jumperState = newState;
    if (jumperState == JumperState.Idle) {
      rb.velocity = Vector3.zero;
    }
    else if (jumperState == JumperState.JumpLeft) {
      rb.AddForce((Vector2.up + Vector2.left) * jumpForce, ForceMode2D.Impulse);
      animator.SetTrigger("jump");
    }
    else if (jumperState == JumperState.JumpRight) {
      rb.AddForce((Vector2.up + Vector2.right) * jumpForce, ForceMode2D.Impulse);
      animator.SetTrigger("jump");
    }
  }

  protected override void Die() {
    // TODO: DIE ANIMATION (IF THERE IS ONE)
    // TODO: GENERATE LOOT OR POWERUP
    PlayHurtSFX();
    Destroy(transform.gameObject, 0.3f);
  }

  // Note - EnemyBase calls PlayHurtSFX
  protected override void PlayHurtSFX() {
    AudioManager.Instance.PlayClip(AudioManager.Instance.SfxManager.JumperHit, AudioCategory.Sfx);
  }

  protected override void PlayDieSFX() {
    AudioManager.Instance.PlayClip(AudioManager.Instance.SfxManager.JumperDie, AudioCategory.Sfx);
  }

  public void OnCollisionEnter2D(Collision2D coll) {
    if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) {
      Player player = coll.gameObject.GetComponent<Player>();
      player.TakeDamage(damage);
      player.KnockBack((coll.transform.position - transform.position).normalized);
    }
  }
}
