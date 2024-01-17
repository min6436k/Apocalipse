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
    public bool Is5Level = false;
    [HideInInspector]
    public GameObject Homingtile;

    private Vector3 _direction;

    public GameObject ExplodeFX;
    //�̵��ӵ�, ����, ?? ����

    [SerializeField]
    private float _lifeTime = 3f;

    //_lifeTime�� ���� �� ������Ʈ �ı�
    void Start()
    {
        Destroy(gameObject, _lifeTime);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Enemy") return;

        if (!Is5Level)
        {

        }
        else
        {
            int _rotate = -1;
            for (int i = 0; i < 3; i++)
            {
                GameObject instance = Instantiate(Homingtile, transform.position, Quaternion.Euler(new Vector3(0, 0, 80*_rotate++)));
                instance.GetComponent<Homingtile>().target = collision.transform;
            }

            Destroy(this.gameObject);
        }

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
}