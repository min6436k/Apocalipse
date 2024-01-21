using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector]
    public float MoveSpeed = 2f;
    [HideInInspector]
    public float acceleration = 0;
    [HideInInspector]
    public float accelTime = 0;
    [HideInInspector]
    public GameObject Homingtile;

    private Vector3 _direction;

    public GameObject ExplodeFX;

    public bool autoDestroy = true;

    [SerializeField]
    private float _lifeTime = 3f;

    //_lifeTime�� ���� �� ������Ʈ �ı�
    void Start()
    {
        if (autoDestroy) Destroy(gameObject, _lifeTime);
        if (acceleration != 0) StartCoroutine(Accel());
    }

    //������Ʈ ���� ��ġ �̵�
    void Update()
    {
        transform.Translate(_direction * MoveSpeed * Time.deltaTime);
    }

    //���ư� ���� ����
    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }

    private void OnDestroy()
    {
        //Instantiate(ExplodeFX, transform.position, Quaternion.identity);
    }
    IEnumerator Accel()
    {

        float CurrentAccelTime = 0f;

        while (CurrentAccelTime < accelTime)
        {
            MoveSpeed += acceleration * Time.deltaTime;

            CurrentAccelTime += Time.deltaTime;

            yield return null;
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet" && this.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }

        if (collision.name.Contains("Meteor"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}