using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : EnemyBase {
  public enum CrabState { Idle, WalkLeft, WalkRight }

  [SerializeField] private float damage;
  [SerializeField] private float walkSpeed = 0.5f;
  [SerializeField] private float stateDuration = 3f;
  private CrabState crabState = CrabState.Idle;
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
      stateTimer = stateDuration * Random.Range(0f, 1f);
      int random = Random.Range(0, 3);
      ChangeStates((CrabState)random);
    }
  }

  protected void FixedUpdate() {
    // handle movement
    if (crabState == CrabState.WalkLeft)
      transform.position += Vector3.left * walkSpeed * Time.deltaTime;
    else if (crabState == CrabState.WalkRight)
      transform.position += Vector3.right * walkSpeed * Time.deltaTime;
  }

  protected void ChangeStates(CrabState newState) {
    crabState = newState;
    if (crabState == CrabState.Idle) {
      rb.velocity = Vector3.zero;
      animator.SetBool("isWalking", false);
    }
    else if (crabState == CrabState.WalkLeft) {
      spriteRenderer.flipX = false;
      animator.SetBool("isWalking", true);
    }
    else if (crabState == CrabState.WalkRight) {
      spriteRenderer.flipX = true;
      animator.SetBool("isWalking", true);
    }
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
