using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
  [SerializeField] private float accelForce = 6f;
  [SerializeField] private float maxSpeed = 2f;
  [SerializeField] private float jumpForce = 30f;
  [SerializeField] private float shootDelay = 0.5f;
  [SerializeField] private Bullet bulletPrefab;
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
    // Handle Shoot timer and state
    shootTimer -= Time.deltaTime;
    if (shootTimer <= 0) {
      isShooting = false;
      animator.SetBool("isShooting", isShooting);
    }
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
    shootTimer = shootDelay;
    isShooting = true;
    animator.SetBool("isShooting", isShooting);
    if (bulletPrefab == null)
      Debug.LogError("Error: missing bullet prefab, can't shoot");
    else {
      GameObject bulletGO = Instantiate(bulletPrefab.gameObject, transform.position, Quaternion.identity);
      // TODO: CHANGE BULLET DIRECTION TO AIM DIRECTION INSTEAD OF RB.VELOCITY
      bulletGO.GetComponent<Bullet>().Initialize(rb.velocity.normalized, bulletPrefab.Speed);
    }      
  }

  public void Jump() {

  }

  public void Cling() {

  }

  public void Duck() {

  }

  public void Hurt() {

  }

  // INPUT
  public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
  
  public void OnFire(InputValue value) => Shoot();


}
