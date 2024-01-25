using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BossA : MonoBehaviour
{
    public GameObject Projectile; //�Ѿ��� �������� ���� ����
    public GameObject afterImage;
    public float ProjectileMoveSpeed = 5.0f; //�Ѿ��� �̵��ӵ�
    public float FireRate = 2.0f; //�Ѿ��� �߻� ����
    public float MoveSpeed = 2.0f; //�̵��ӵ�
    public float MoveDistance = 5.0f; //�� ������ ������ �Ÿ� ����
    public bool _bCanMove = false; //�����ϼ� �ִ���

    [SerializeField]
    private int currentPatternIndex = 0; //���� ���� ����
    [HideInInspector]
    public int MaxpatternIndex = 6;

    private bool _movingRight = true; //���������� �����̴�������
    private Vector3 _originPosition; //�ʱ� ��ġ

    public virtual void Start()
    {
        MaxpatternIndex = 6;

        _originPosition = transform.position; //�ʱ� ��ġ ����
        StartCoroutine(MoveDownAndStartPattern()); //�����Ӱ� ������ ���۽�Ű�� �Լ� ȣ��
    }

    public IEnumerator MoveDownAndStartPattern()
    {
        while (transform.position.y > _originPosition.y - 3f) //������Ʈ�� ��ǥ�� ������ y������ 3�̻� Ŭ����
        {
            transform.Translate(Vector3.down * MoveSpeed * Time.deltaTime); //������ �ӵ���ŭ �Ʒ��� �̵�
            yield return null; //null ��ȯ
        }

        _bCanMove = true; //�����ϼ� �ִ� ���·� ����
        yield return new WaitForSeconds(FireRate);
        NextPattern();
    }

    public void Update()
    {
        if (_bCanMove) //������ �� �ִٸ�
            MoveSideways(); //���� ������ �Լ� ȣ��
    }

    public void NextPattern()
    {
        // ���� ���� ����
        switch (currentPatternIndex)
        {
            case 0: //���� �ε����� 0�̶��
                StartCoroutine(Pattern1()); //ù��° ���� ȣ��
                break; //switch Ż��
            case 1: //==
                StartCoroutine(Pattern2()); ; //==
                break; // ==
            case 2:
                StartCoroutine(Pattern3());
                break;
            case 3:
                StartCoroutine(Pattern4());
                break;
            case 4:
                StartCoroutine(Pattern5());
                break;
            case 5:
                StartCoroutine(Pattern6());
                break;
        }

        // ���� �ε����� ������Ű��, ������ ������ ��� �ٽ� ó�� �������� ���ư�
        currentPatternIndex = (currentPatternIndex + 1) % MaxpatternIndex;
    }

    public void MoveSideways()
    {
        if (_movingRight) //���������� �����̴����̶��
        {
            transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime); //���������� �̵��ӵ���ŭ �̵�
            if (transform.position.x > MoveDistance) //�Ÿ����Ѻ��� x���� Ŭ ���
            {
                _movingRight = false; //������ �̵� false
            }
        }
        else //���������� �����̴����� �ƴ϶��
        {
            transform.Translate(Vector3.left * MoveSpeed * Time.deltaTime); //������ �̵��ӵ���ŭ �̵�
            if (transform.position.x < -MoveDistance) //�Ÿ����Ѻ��� x���� ���� ���
            {
                _movingRight = true; //������ �̵� true
            }
        }
    }

    private void StartMovingSideways() //������ ����
    {
        StartCoroutine(MovingSidewaysRoutine());
    }

    public IEnumerator MovingSidewaysRoutine()
    {
        while (true) //���ѹݺ�
        {
            MoveSideways(); //���� �̵� �Լ� ȣ��
            yield return null; //null ��ȯ
        }
    }

    public void ShootProjectile(Vector3 position, Vector3 direction, float acceleration = 0, float accelTime = 0,float speed = 0)
    {
        GameObject instance = Instantiate(Projectile, position, Quaternion.identity); //Projectile�� �Է¹��� ��ġ�� ���� �� ������ ����
        Projectile projectile = instance.GetComponent<Projectile>(); //projectile������ instance�� Ŭ���� ���� ����

        if (projectile != null) //projectile�� ���� �����Ѵٸ�
        {
            projectile.MoveSpeed = speed==0 ? ProjectileMoveSpeed : speed; //projectile�� �̵��ӵ� ����
            projectile.SetDirection(direction.normalized); //�Է¹��� ������ ��ֶ������Ͽ� SetDirection�Լ��� ȣ���� projectile�� �̵� ���� ����
            projectile.acceleration = acceleration;
            projectile.accelTime = accelTime;
        }
    }

    public virtual IEnumerator Pattern1()
    {
        // ���� 1: �������� �Ѿ� �߻�
        int numBullets1 = 30; //1������ �Ѿ� ����
        float angleStep1 = 360.0f / numBullets1; //�Ѿ� ������ŭ 360���� ����

        for (int i = 0; i < numBullets1; i++) //�Ѿ� ������ŭ �ݺ�
        {
            float angle1 = i * angleStep1; //�Ѿ��� ������ i * ������ ������ ����
            float radian1 = angle1 * Mathf.Deg2Rad; //���� ǥ���� ������ ȣ�������� ����
            Vector3 direction1 = new Vector3(Mathf.Cos(radian1), Mathf.Sin(radian1), 0); //�ﰢ�Լ��� �̿��� �Ѿ��� �ٶ� ���� ���,  ����ǥ��, ������ǥ��? ������ �� �𸣴� ����

            ShootProjectile(transform.position, direction1); //ShootProjectile �Լ� ȣ��
        }

        yield return new WaitForSeconds(FireRate);

        NextPattern();
    }

    public virtual IEnumerator Pattern2()
    {
        // ���� 2: ��������� �Ѿ� �߻�, ���� ����
        int numBullets2 = 12;
        float angleStep2 = 360.0f / numBullets2;

        for (int i = 0; i < numBullets2; i++)
        {
            float angle2 = i * angleStep2;
            float radian2 = angle2 * Mathf.Deg2Rad;
            Vector3 direction2 = new Vector3(Mathf.Cos(radian2), Mathf.Sin(radian2), 0);

            ShootProjectile(transform.position, direction2);
        }

        yield return new WaitForSeconds(FireRate);

        NextPattern();
    }

    public virtual IEnumerator Pattern3()
    {
        // ���� 3: �� �� �������� �÷��̾�� �ϳ��� �߻�
        int numBullets = 5; //�߻��� �Ѿ� ����
        float interval = 0.8f; //�Ѿ� ������ �ð�����

        for (int i = 0; i < numBullets; i++) //�Ѿ� ������ŭ �ݺ�
        {
            Vector3 playerDirection = (PlayerPosition() - transform.position).normalized; //�÷��̾��� ��ġ�� �ڽ��� ��ġ�� ���� ��ֶ������Ͽ� �ڽŰ� �÷��̾��� ������ ����
            ShootProjectile(transform.position, playerDirection); //ShootProjectile �Լ� ȣ��
            yield return new WaitForSeconds(interval); //1�� ���
        }

        yield return new WaitForSeconds(FireRate);

        NextPattern();
    }

    public virtual IEnumerator Pattern4()
    {
        int numBullets3 = 10; //�Ѿ� ����
        float angleStep3 = 360.0f / numBullets3; //�Ѿ� ������ŭ 360���� ����
        float radius = 2.0f; //������

        for (int i = 0; i < numBullets3; i++) //�Ѿ� ������ŭ �ݺ�
        {
            float angle3 = i * angleStep3; //�Ѿ��� ������ i * ������ ������ ����
            float radian3 = angle3 * Mathf.Deg2Rad; //���� ǥ���� ������ ȣ�������� ����
            float x = radius * Mathf.Cos(radian3); 
            float y = radius * Mathf.Sin(radian3);

            Vector3 direction3 = new Vector3(x, y, 0).normalized; //��ֶ���� ���� ���⺤�� ���ϱ�

            ShootProjectile(transform.position, direction3);//ShootProjectile �Լ� ȣ��
        }

        yield return new WaitForSeconds(FireRate);

        NextPattern();
    }

    public virtual IEnumerator Pattern5()
    {
        int numBullets = 20;
        float BulletInterval = 0.4f;

        for (int i = 0; i < numBullets; i++) 
        {
            Vector3 position = transform.position;

            position.x -= 0.3f;
            for (int j = 0; j < 2; j++)
            {
                ShootProjectile(position, Vector3.down, 4, 1);

                position.x += 0.6f;
            }

            yield return new WaitForSeconds(BulletInterval);
            if(BulletInterval > 0.05f) BulletInterval *= 0.5f;
        }
        yield return new WaitForSeconds(FireRate);

        NextPattern();
    }

    public virtual IEnumerator Pattern6()
    {
        _bCanMove = false;

        for(int i = -2; i<3; i++)
        {
            if(i != 0)
            {
                GameObject instance = Instantiate(afterImage, transform);
                BossAfterImage AfterImage  = instance.GetComponent<BossAfterImage>();
                AfterImage.moveStep = i;
                AfterImage.ProjectileMoveSpeed = ProjectileMoveSpeed;
                AfterImage.projectile = Projectile;
            }
        }

        yield return new WaitForSeconds(1);

        _bCanMove = true;

        shotBullet();

        yield return new WaitForSeconds(1.5f);

        shotBullet();

        yield return new WaitForSeconds(1.5f);

        shotBullet();

        yield return new WaitForSeconds(FireRate);

        NextPattern();
    }

    public void shotBullet()
    {
        for (int i = 0; i < 2; i++)
        {
            ShootProjectile(transform.position, Vector3.down);
            ShootProjectile(transform.position, new Vector3(0.3f, -1, 0));
            ShootProjectile(transform.position, new Vector3(-0.3f, -1, 0));
        }
    }

    public Vector3 PlayerPosition()
    {
        return GameManager.Instance.GetPlayerCharacter().transform.position; //�÷��̾��� ��ġ ���ϱ�
    }

    public void OnDestroy()
    {
        GameManager.Instance.StageClear(); //���ӸŴ����� �������� Ŭ���� �Լ� ȣ��
    }

}