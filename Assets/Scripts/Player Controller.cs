using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  [SerializeField] private float speed = 500f;
  [SerializeField] private float jumpForce = 300f;
  [SerializeField] private float shootDelay = 100f;
  private Vector2 movement;
  private Rigidbody2D rb;
  private bool isJumping = false;
  private bool isShooting = false;
  private float shootTimer = 0f;

  public void Awake() {
    rb = GetComponent<Rigidbody2D>();
  }

  public void Update() {
    shootTimer -= Time.deltaTime;
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
  public void OnMove(InputValue value) => movement = value.Get<Vector2>();
  
  public void OnFire(InputValue value) => Shoot();


}
