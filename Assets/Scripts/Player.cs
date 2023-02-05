using System;
using System.Collections;
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
    // TODO: CHECK FOR GROUNDED INSTEAD
    if (jumpTimer <= 0) {
      isJumping = false;
      animator.SetBool("isJumping", isJumping);
    }
  }

  public void FixedUpdate() {
    // Movement
    acceleration.x = moveInput.x * accelForce;
    if (rb.velocity.x <= maxSpeed && acceleration.x > 0) 
      rb.AddForce(acceleration);
    else if (rb.velocity.x >= -maxSpeed && acceleration.x < 0) 
      rb.AddForce(acceleration);
    spriteRenderer.flipX = (rb.velocity.x >= 0) ? false : true;
    animator.SetFloat("movement", Math.Abs(rb.velocity.x));
  }


  public void Shoot() {
    if (shootTimer >= 0)
      return;

    if (bulletPrefab == null)
      Debug.LogError("Error: missing bullet prefab, can't shoot");
    else {
      GameObject bulletGO = Instantiate(bulletPrefab.gameObject, transform.position, Quaternion.identity);
      // TODO: CHANGE BULLET DIRECTION TO AIM DIRECTION INSTEAD OF RB.VELOCITY
      bulletGO.GetComponent<Bullet>().Initialize(rb.velocity.normalized, bulletPrefab.Speed);
    }
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
    animator.SetBool("isJumping", isJumping);
  }

  public void Cling() {

  }

  public void Duck() {

  }

  public void TakeDamage(float amt) {
    Debug.Log("Player taking damage! " + amt);
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

  // INPUT
  public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
  
  public void OnFire(InputValue value) => Shoot();

  public void OnJump(InputValue value) => Jump();


}
