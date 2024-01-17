using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homingtile : MonoBehaviour
{
    private float speed = 12f;
    private float rotateSpeed = 1000f;
    private bool IsTracking = false;

    public Transform target;

    private Rigidbody2D rb;
    private BoxCollider2D col;

    Rigidbody2D rigid;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();

        StartCoroutine(HitCooldown());
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        rb.velocity = transform.up * speed;

        if (!col.enabled || target == null) return;

        Vector2 dir = (Vector2)target.position - rb.position;

        dir.Normalize();

        float rotateAmount = Vector3.Cross(dir, transform.up).z;

        if (rotateAmount == 0) rotateAmount = 1;

        rb.angularVelocity = -rotateAmount * rotateSpeed;
    }

    IEnumerator HitCooldown()
    {
        yield return new WaitForSeconds(0.1f);

        col.enabled = true;

        StartCoroutine(SpeedUp());
    }

    IEnumerator SpeedUp()
    {
        int flickCount = 0; // 깜박인 횟수를 기록하는 변수

        while (flickCount < 10)
        {
            speed += 2f;
            rotateSpeed += 200;

            yield return new WaitForSeconds(0.1f); // 0.1초 대기

            flickCount++; // 깜박인 횟수 증가
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Enemy" || !col.enabled) return;

        Destroy(this.gameObject);
    }

}
