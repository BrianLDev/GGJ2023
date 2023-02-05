using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  [SerializeField] private float damage = 20f;    // default value.  Override on prefabs if needed
  [SerializeField] private float speed = 2f;      // default value.  Override on prefabs if needed
  [SerializeField] private float lifetime = 10f;  // default value.  Override on prefabs if needed
  public float Speed => speed;
  private Vector3 direction;
  private Rigidbody2D rb;
  private Animator animator;
  private float timer;

  private void Awake() {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponentInChildren<Animator>();
    timer = lifetime;
  }

  private void Start() {
    // TODO: FIX THIS SO CAN SHOOT EITHER LEFT OR RIGHT WHEN STATIONARY
    if (direction == Vector3.zero)
      direction = Vector3.right;
    rb.velocity = direction * speed;
  }

  public void Initialize(Vector3 dir, float spd) {
    direction = dir;
    speed = spd;
  }

  private void Update() {
    // track lifetime timer and destroy GO if it is old
    timer -= Time.deltaTime;
    if (timer <= 0)
      Destroy(transform.gameObject, 0.5f);
  }

  public void OnCollisionEnter2D(Collision2D coll) {
    rb.velocity = Vector3.zero;
    animator.SetTrigger("collision");
    if (coll.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
      coll.gameObject.GetComponent<EnemyBase>().TakeDamage(damage);
    }
    // TODO: PLAY SFX
    Destroy(transform.gameObject, 0.25f);
  }
}
