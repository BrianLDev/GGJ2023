using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  [SerializeField] private float speed;
  private Vector3 direction;
  private Rigidbody2D rb;
  private Animator animator;
  private bool isActive = true;

  private void Awake() {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponentInChildren<Animator>();
  }

  public void Initialize(Vector3 dir, float spd) {
    direction = dir;
    speed = spd;
    rb.velocity = direction * speed;
  }

  public void OnCollisionEnter2D(Collision2D coll) {
    // TODO: DAMAGE THE OBJECT
    // TODO: PLAY SFX
    animator.SetTrigger("collision");
    Destroy(transform.gameObject, 0.5f);
    isActive = false;
  }
}
