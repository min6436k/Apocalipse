using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
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
        if (target == null) IsTracking = false;

        rb.velocity = transform.up * speed;

        if (!IsTracking) return;

        Vector2 dir = (Vector2)target.position - rb.position;

        dir.Normalize();

        float rotateAmount = Vector3.Cross(dir, transform.up).z;

        if (rotateAmount == 0) rotateAmount = 1;

        rb.angularVelocity = -rotateAmount * rotateSpeed;
    }

    IEnumerator HitCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(SpeedUp());
        IsTracking = true;
        yield return new WaitForSeconds(0.2f);
        col.enabled = true;
    }

    IEnumerator SpeedUp()
    {
        int flickCount = 0; // ������ Ƚ���� ����ϴ� ����

        while (flickCount < 10)
        {
            speed += 2f;
            rotateSpeed += 200;

            yield return new WaitForSeconds(0.1f); // 0.1�� ���

            flickCount++; // ������ Ƚ�� ����
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Enemy") return;

        Destroy(this.gameObject);
    }

}
