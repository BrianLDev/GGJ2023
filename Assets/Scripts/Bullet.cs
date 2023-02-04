using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  [SerializeField] private float speed = 2f;      // default value.  Override on prefabs if needed
  [SerializeField] private float lifetime = 10f;  // default value.  Override on prefabs if needed
  public float Speed => speed;
  private Vector3 direction;
  private Rigidbody2D rb;
  private Animator animator;
  private bool isActive = true;
  private float timer;

  private void Awake() {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponentInChildren<Animator>();
    timer = lifetime;
  }

  public void Initialize(Vector3 dir, float spd) {
    direction = dir;
    speed = spd;
    rb.velocity = direction * speed;
  }

  private void Update() {
    // track lifetime timer and destroy GO if it is old
    timer -= Time.deltaTime;
    if (timer <= 0)
      Destroy(transform.gameObject, 0.5f);
  }

  public void OnCollisionEnter2D(Collision2D coll) {
    // TODO: DAMAGE THE OBJECT
    // TODO: PLAY SFX
    animator.SetTrigger("collision");
    Destroy(transform.gameObject, 0.5f);
    isActive = false;
  }
}
