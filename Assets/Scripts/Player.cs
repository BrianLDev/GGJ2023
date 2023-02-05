using System;
using System.Collections;
using EcxUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
  public float Health => health;
  public float MaxHealth => maxHealth;
  [SerializeField] private Bullet bulletPrefab;
  [SerializeField] private float maxHealth = 100f;
  [SerializeField] private float accelForce = 6f;
  [SerializeField] private float maxSpeed = 2f;
  [SerializeField] private float shootDelay = 0.15f;
  [SerializeField] private float jumpForce = 4f;
  [SerializeField] private float jumpDelay = 0.4f;
  [SerializeField] private float knockbackForce = 2f;
  private float health;
  private Vector2 moveInput;
  private Vector3 acceleration;
  private Vector3 aimDirection;
  private Rigidbody2D rb;
  private Animator animator;
  private SpriteRenderer spriteRenderer;
  private bool isJumping = false;
  private bool isShooting = false;
  private float shootTimer = 0f;
  private float jumpTimer = 0f;

  private void Awake() {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponentInChildren<Animator>();
    spriteRenderer = GetComponentInChildren<SpriteRenderer>();
  }

  private void Update() {
    // Handle Shoot timer and state
    shootTimer -= Time.deltaTime;
    if (shootTimer <= 0) {
      isShooting = false;
      animator.SetBool("isShooting", isShooting);
    }
    // Handle Jump timer and state
    jumpTimer -= Time.deltaTime;
    if (jumpTimer <= 0) {
      isJumping = false;
      animator.SetBool("isJumping", isJumping);
    }
  }

  public void FixedUpdate() {
    // face left/right
    if (moveInput.x > 0.1f || moveInput.x < -0.1f)
      spriteRenderer.flipX = (moveInput.x >= 0) ? false : true;
    // Aim
    if (moveInput != Vector2.zero)
      aimDirection = moveInput.normalized;
    // Movement
    acceleration.x = moveInput.x * accelForce;
    if (rb.velocity.x <= maxSpeed && acceleration.x > 0) 
      rb.AddForce(acceleration);
    else if (rb.velocity.x >= -maxSpeed && acceleration.x < 0) 
      rb.AddForce(acceleration);
    animator.SetFloat("movement", Math.Abs(rb.velocity.x));
  }


  public void Shoot() {
    if (shootTimer >= 0)
      return;
    if (bulletPrefab == null) {
      Debug.LogError("Error: missing bullet prefab, can't shoot");
      return;
    }
    GameObject bulletGO = Instantiate(bulletPrefab.gameObject, transform.position, Quaternion.identity);
    bulletGO.GetComponent<Bullet>().Initialize(aimDirection, bulletPrefab.Speed);
    AudioManager.Instance.PlayClip(AudioManager.Instance.SfxManager.BigGun01, AudioCategory.Sfx);
    shootTimer = shootDelay;
    isShooting = true;
    animator.SetBool("isShooting", isShooting);
  }

  public void Jump() {
    if (jumpTimer >= 0)
      return;
    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    jumpTimer = jumpDelay;
    isJumping = true;
    animator.SetTrigger("jump");
    animator.SetBool("isJumping", isJumping);
  }

  public void Cling() {

  }

  public void Duck() {

  }

  public void TakeDamage(float amt) {
    Debug.Log("Player taking damage! " + amt);
    animator.SetTrigger("hurt");
    health -= amt;
    if (health <= 0)
      Die();
  }

  public void Heal(float amt) {
    health += amt;
    if (health >= maxHealth)
      health = maxHealth;
  }

  public void KnockBack(Vector2 dir, float multiplier = 1f) {
    rb.AddForce(dir * knockbackForce * multiplier, ForceMode2D.Impulse);
  }

  private void Die() {
    // TODO: PLAY DIE ANIMATION
    // TODO: DIE SFX
    // TODO: GAME OVER LOGIC AND WHAT NOT
  }

  private void OnCollisionEnter2D(Collision2D coll) {
    // stop jumping when hit the ground going down
    if (coll.gameObject.layer == LayerMask.NameToLayer("Ground") && rb.velocity.y <= 0) {
      isJumping = false;
      animator.SetBool("isJumping", isJumping);
    }
    // TODO: CLING MECHANIC
    // cling to walls
    // if (coll.gameObject.layer == LayerMask.NameToLayer("Walls")) {
    //   rb.velocity = Vector3.zero;
    //   isJumping = false;
    //   animator.SetBool("isJumping", isJumping);
    //   animator.SetTrigger("cling");
    // }
  }

  // INPUT
  public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
  
  public void OnFire(InputValue value) => Shoot();

  public void OnJump(InputValue value) => Jump();


}
