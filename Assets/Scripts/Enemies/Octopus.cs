using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EcxUtilities;

public class Octopus : EnemyBase {
  public enum OctopusState { Idle, Moving }

  [SerializeField] private float damage = 30f;
  [SerializeField] private float speed = 2f;
  [SerializeField] private float stateDuration = 1.5f;
  private OctopusState jumperState = OctopusState.Idle;
  private float stateTimer = 0;
  private Rigidbody2D rb;
  // private Animator animator;
  private SpriteRenderer spriteRenderer;
  private Vector2 direction;

  protected void Awake() {
    rb = GetComponent<Rigidbody2D>();
    // animator = GetComponentInChildren<Animator>();
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
      int random = Random.Range(0, 2);
      ChangeStates((OctopusState)random);
    }
    // flip facing left/right
    if (rb.velocity.x > 0.1f)
      spriteRenderer.flipX = true;
    else
      spriteRenderer.flipX = false;
  }

  protected void ChangeStates(OctopusState newState) {
    jumperState = newState;
    if (jumperState == OctopusState.Idle) {
      rb.velocity = Vector3.zero;
    }
    else if (jumperState == OctopusState.Moving) {
      direction = Random.insideUnitCircle;
      rb.AddForce(direction * speed, ForceMode2D.Impulse);
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
    AudioManager.Instance.PlayClip(AudioManager.Instance.SfxManager.OctopusHit, AudioCategory.Sfx);
  }

  protected override void PlayDieSFX() {
    AudioManager.Instance.PlayClip(AudioManager.Instance.SfxManager.OctopusDie, AudioCategory.Sfx);
  }

  public void OnCollisionEnter2D(Collision2D coll) {
    if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) {
      Player player = coll.gameObject.GetComponent<Player>();
      player.TakeDamage(damage);
      player.KnockBack((coll.transform.position - transform.position).normalized);
    }
  }
}
