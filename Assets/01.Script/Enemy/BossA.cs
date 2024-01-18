using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BossA : MonoBehaviour
{
    public GameObject Projectile; //총알의 프리팹을 담을 변수
    public GameObject afterImage;
    public float ProjectileMoveSpeed = 5.0f; //총알의 이동속도
    public float FireRate = 2.0f; //총알의 발사 간격
    public float MoveSpeed = 2.0f; //이동속도
    public float MoveDistance = 5.0f; //양 옆으로 움직일 거리 제한

    private int _currentPatternIndex = 0; //현재 패턴 상태
    private bool _movingRight = true; //오른쪽으로 움직이는중인지
    private bool _bCanMove = false; //움직일수 있는지
    private Vector3 _originPosition; //초기 위치

    private void Start()
    {
        _originPosition = transform.position; //초기 위치 설정
        StartCoroutine(MoveDownAndStartPattern()); //움직임과 패턴을 시작시키는 함수 호출
    }

    private IEnumerator MoveDownAndStartPattern()
    {
        while (transform.position.y > _originPosition.y - 3f) //오브젝트의 좌표가 기존의 y값보다 3이상 클동안
        {
            transform.Translate(Vector3.down * MoveSpeed * Time.deltaTime); //움직임 속도만큼 아래로 이동
            yield return null; //null 반환
        }

        _bCanMove = true; //움직일수 있는 상태로 변경
        Invoke("NextPattern", 2.0f);
    }

    private void Update()
    {
        if (_bCanMove) //움직일 수 있다면
            MoveSideways(); //가로 움직임 함수 호출
    }

    private void NextPattern()
    {
        // 패턴 인덱스를 증가시키고, 마지막 패턴일 경우 다시 처음 패턴으로 돌아감
        _currentPatternIndex = (_currentPatternIndex + 1) % 6;

        // 현재 패턴 실행
        switch (_currentPatternIndex)
        {
            case 0: //패턴 인덱스가 0이라면
                Pattern1(); //첫번째 패턴 호출
                break; //switch 탈출
            case 1: //==
                Pattern2(); //==
                break; // ==
            case 2:
                StartCoroutine(Pattern3());
                break;
            case 3:
                Pattern4();
                break;
            case 4:
                StartCoroutine(Pattern5());
                break;
            case 5:
                StartCoroutine(Pattern6());
                break;
        }
    }

    private void MoveSideways()
    {
        if (_movingRight) //오른쪽으로 움직이는중이라면
        {
            transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime); //오른쪽으로 이동속도만큼 이동
            if (transform.position.x > MoveDistance) //거리제한보다 x값이 클 경우
            {
                _movingRight = false; //오른쪽 이동 false
            }
        }
        else //오른쪽으로 움직이는중이 아니라면
        {
            transform.Translate(Vector3.left * MoveSpeed * Time.deltaTime); //왼으로 이동속도만큼 이동
            if (transform.position.x < -MoveDistance) //거리제한보다 x값이 작을 경우
            {
                _movingRight = true; //오른쪽 이동 true
            }
        }
    }

    private void StartMovingSideways() //사용되지 않음
    {
        StartCoroutine(MovingSidewaysRoutine());
    }

    private IEnumerator MovingSidewaysRoutine()
    {
        while (true) //무한반복
        {
            MoveSideways(); //가로 이동 함수 호출
            yield return null; //null 반환
        }
    }

    public void ShootProjectile(Vector3 position, Vector3 direction, float acceleration = 0, float accelTime = 0)
    {
        GameObject instance = Instantiate(Projectile, position, Quaternion.identity); //Projectile을 입력받은 위치로 생성 후 변수에 저장
        Projectile projectile = instance.GetComponent<Projectile>(); //projectile변수에 instance의 클래스 정보 저장

        if (projectile != null) //projectile의 값이 존재한다면
        {
            projectile.MoveSpeed = ProjectileMoveSpeed; //projectile의 이동속도 설정
            projectile.SetDirection(direction.normalized); //입력받은 방향을 노멀라이즈하여 SetDirection함수를 호출해 projectile의 이동 방향 설정
            projectile.acceleration = acceleration;
            projectile.accelTime = accelTime;
        }
    }

    private void Pattern1()
    {
        // 패턴 1: 원형으로 총알 발사
        int numBullets1 = 30; //1패턴의 총알 갯수
        float angleStep1 = 360.0f / numBullets1; //총알 갯수만큼 360도를 분할

        for (int i = 0; i < numBullets1; i++) //총알 갯수만큼 반복
        {
            float angle1 = i * angleStep1; //총알의 각도를 i * 분할한 각도로 설정
            float radian1 = angle1 * Mathf.Deg2Rad; //도로 표현된 변수를 호도법으로 변경
            Vector3 direction1 = new Vector3(Mathf.Cos(radian1), Mathf.Sin(radian1), 0); //삼각함수를 이용해 총알이 바라볼 방향 계산,  극좌표계, 직교좌표계? 지금은 잘 모르는 개념

            ShootProjectile(transform.position, direction1); //ShootProjectile 함수 호출
        }

        Invoke("NextPattern", 0.5f);
    }

    private void Pattern2()
    {
        // 패턴 2: 방사형으로 총알 발사, 위와 동일
        int numBullets2 = 12;
        float angleStep2 = 360.0f / numBullets2;

        for (int i = 0; i < numBullets2; i++)
        {
            float angle2 = i * angleStep2;
            float radian2 = angle2 * Mathf.Deg2Rad;
            Vector3 direction2 = new Vector3(Mathf.Cos(radian2), Mathf.Sin(radian2), 0);

            ShootProjectile(transform.position, direction2);
        }

        Invoke("NextPattern", 0.5f);
    }

    private IEnumerator Pattern3()
    {
        // 패턴 3: 몇 초 간격으로 플레이어에게 하나씩 발사
        int numBullets = 5; //발사할 총알 개수
        float interval = 1.0f; //총알 사이의 시간간격

        for (int i = 0; i < numBullets; i++) //총알 갯수만큼 반복
        {
            Vector3 playerDirection = (PlayerPosition() - transform.position).normalized; //플레이어의 위치와 자신의 위치의 차를 노멀라이즈하여 자신과 플레이어의 방향을 구함
            ShootProjectile(transform.position, playerDirection); //ShootProjectile 함수 호출
            yield return new WaitForSeconds(interval); //1초 대기
        }

        Invoke("NextPattern", 0.5f);
    }

    private void Pattern4()
    {
        // 패턴 3: 나선형으로 총알 발사
        int numBullets3 = 10; //총알 갯수
        float angleStep3 = 360.0f / numBullets3; //총알 갯수만큼 360도를 분할
        float radius = 2.0f; //반지름

        for (int i = 0; i < numBullets3; i++) //총알 갯수만큼 반복
        {
            float angle3 = i * angleStep3; //총알의 각도를 i * 분할한 각도로 설정
            float radian3 = angle3 * Mathf.Deg2Rad; //도로 표현된 변수를 호도법으로 변경
            float x = radius * Mathf.Cos(radian3); 
            float y = radius * Mathf.Sin(radian3);

            Vector3 direction3 = new Vector3(x, y, 0).normalized; //노멀라이즈를 통해 방향벡터 구하기

            ShootProjectile(transform.position, direction3);//ShootProjectile 함수 호출
        }

        Invoke("NextPattern", 0.5f);
    }

    private IEnumerator Pattern5()
    {

        int numBullets = 20;
        float interval = 1.0f;
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
        yield return new WaitForSeconds(interval);

        Invoke("NextPattern", 0.5f);
    }

    private IEnumerator Pattern6()
    {
        _bCanMove= false;

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

        Invoke("NextPattern", 1.0f);
    }

    private void shotBullet()
    {
        for (int i = 0; i < 2; i++)
        {
            ShootProjectile(transform.position, Vector3.down);
            ShootProjectile(transform.position, new Vector3(0.3f, -1, 0));
            ShootProjectile(transform.position, new Vector3(-0.3f, -1, 0));
        }
    }

    private Vector3 PlayerPosition()
    {
        return GameManager.Instance.GetPlayerCharacter().transform.position; //플레이어의 위치 구하기
    }

    private void OnDestroy()
    {
        //GameManager.Instance.StageClear();
    }

}