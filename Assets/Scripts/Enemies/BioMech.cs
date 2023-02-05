using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // choose state
    stateTimer -= Time.deltaTime;
    if (stateTimer <= 0) {
      stateTimer = stateDuration * Random.Range(0.5f, 1.5f);
      int random = Random.Range(0, 5);
      ChangeStates((BioMechState)random);
    }
  }

  protected void FixedUpdate() {
    // handle movement
    if (bioMechState == BioMechState.WalkLeft)
      transform.position += Vector3.left * walkSpeed * Time.deltaTime;
    else if (bioMechState == BioMechState.WalkRight)
      transform.position += Vector3.right * walkSpeed * Time.deltaTime;
    // Attacking
    // TODO: ATTACK LEFT, RIGHT
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