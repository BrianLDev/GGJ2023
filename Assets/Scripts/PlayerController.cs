using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  [SerializeField] private float accelForce = 6f;
  [SerializeField] private float maxSpeed = 2f;
   [SerializeField] private float jumpForce = 30f;
  [SerializeField] private float shootDelay = 100f;
  private Vector2 moveInput;
  private Vector3 acceleration;
  private Rigidbody2D rb;
  private Animator animator;
  private SpriteRenderer spriteRenderer;
  private bool isJumping = false;
  private bool isShooting = false;
  private float shootTimer = 0f;

  private void Awake() {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponentInChildren<Animator>();
    spriteRenderer = GetComponentInChildren<SpriteRenderer>();
  }

  private void Update() {
    shootTimer -= Time.deltaTime;
  }

  public void FixedUpdate() {
    // Movement
    acceleration.x = moveInput.x * accelForce;
    rb.AddForce(acceleration);
    if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
      rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    spriteRenderer.flipX = (rb.velocity.x > 0) ? false : true;
    animator.SetFloat("movement", Math.Abs(rb.velocity.x));
  }


  public void Shoot() {
    if (shootTimer >= 0)
      return;
    isShooting = true;
    shootTimer = shootDelay;
    // TODO: SHOOT ANIMATION
    // TODO: SHOOT A BULLET
  }

  // INPUT
  public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
  
  public void OnFire(InputValue value) => Shoot();


}
