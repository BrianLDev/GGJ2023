using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EcxUtilities;

public class BioMech : EnemyBase {
  public enum BioMechState { Idle, WalkLeft, WalkRight, AttackLeft, AttackRight, Dead }

  [SerializeField] private float damage = 25;
  [SerializeField] private float walkSpeed = 0.5f;
  [SerializeField] private float stateDuration = 1.5f;
  private BioMechState bioMechState = BioMechState.Idle;
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
    if (GameManager.Instance.CurrentState == GameManager.GameState.Game && bioMechState != BioMechState.Dead) {
      // choose state
      stateTimer -= Time.deltaTime;
      if (stateTimer <= 0) {
        stateTimer = stateDuration * Random.Range(0.5f, 1.5f);
        int random = Random.Range(0, 5);
        ChangeStates((BioMechState)random);
      }
    }
  }

  protected void FixedUpdate() {
    if (GameManager.Instance.CurrentState == GameManager.GameState.Game && bioMechState != BioMechState.Dead) {
      // handle movement
      if (bioMechState == BioMechState.WalkLeft)
        transform.position += Vector3.left * walkSpeed * Time.deltaTime;
      else if (bioMechState == BioMechState.WalkRight)
        transform.position += Vector3.right * walkSpeed * Time.deltaTime;
      // Attacking
      // TODO: ATTACK LEFT, RIGHT
    }
  }

  protected void ChangeStates(BioMechState newState) {
    bioMechState = newState;
    if (bioMechState == BioMechState.Idle) {
      rb.velocity = Vector3.zero;
      animator.SetBool("isWalking", false);
    }
    else if (bioMechState == BioMechState.WalkLeft) {
      spriteRenderer.flipX = true;
      animator.SetBool("isWalking", true);
    }
    else if (bioMechState == BioMechState.WalkRight) {
      spriteRenderer.flipX = false;
      animator.SetBool("isWalking", true);
    }
    else if (bioMechState == BioMechState.AttackLeft) {
      spriteRenderer.flipX = true;
      animator.SetBool("isWalking", false);
    }
    else if (bioMechState == BioMechState.AttackRight) {
      spriteRenderer.flipX = false;
      animator.SetBool("isWalking", false);
    }
  }

  protected override void Die() {
    if (bioMechState != BioMechState.Dead) {
      // TODO: GENERATE LOOT OR POWERUP
      bioMechState = BioMechState.Dead;
      PlayDieSFX();
      animator.SetTrigger("die");
      spriteRenderer.flipX = true;
      // Destroy(transform.gameObject, 5.0f);
      GameManager.Instance.Victory();
    }
  }

  // Note - EnemyBase calls PlayHurtSFX
  protected override void PlayHurtSFX() {
    AudioManager.Instance.PlayClip(AudioManager.Instance.SfxManager.BioMechHit, AudioCategory.Sfx, 1.3f);
  }

  protected override void PlayDieSFX() {
    AudioManager.Instance.PlayClip(AudioManager.Instance.SfxManager.BioMechDie, AudioCategory.Sfx, 2.0f);
  }

  public void OnCollisionEnter2D(Collision2D coll) {
    if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) {
      Player player = coll.gameObject.GetComponent<Player>();
      player.TakeDamage(damage);
      player.KnockBack((coll.transform.position - transform.position).normalized);
    }
  }
}
