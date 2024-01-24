using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//캐릭터가 사용할 스킬들이나 움직임 등을 처리하는 클래스
//죽음이나 무적 등 플레이어의 상태와 관련된 코드도 이곳에서 구현됨
public class PlayerCharacter : BaseCharacter
{
    #region Movement
    private Vector2 _moveInput;
    private Animator _playerAnimator;
    public float MoveSpeed;

    #endregion
    //플레이어의 이동과 움직임 방향 변수

    #region Skills
    [HideInInspector] public Dictionary<EnumTypes.PlayerSkill, BaseSkill> Skills;
    [SerializeField] private GameObject[] _skillPrefabs;
    #endregion
    //스킬에 대한 정보를 담을 딕셔너리와 스킬프리팹등을 담을 배열

    #region Invincibility
    private bool invincibility; //무적 여부
    private Coroutine invincibilityCoroutine; //무적시간 코루틴을 제어하기 위한 변수
    private const double InvincibilityDurationInSeconds = 3; // 무적 지속 시간 (초), 매직넘버 방지용
    //캐릭터의 무적 여부와 무적 시간을 담을 변수, 그리고 남은 무적 시간을 계산할 코루틴
    public bool Invincibility //private 변수를 외부에서도 처리하기 위한 getter, setter
    {
        get { return invincibility; }
        set { invincibility = value; }
    }
    //무적 여부를 반환하고, 설정하는것을 편하게 하기 위한 함수
    #endregion

    #region Weapon
    public int CurrentWeaponLevel = 0;
    public int MaxWeaponLevel = 5;
    //캐릭터의 현재 무기 레벨과 최대 무기 레벨을 담는 변수
    #endregion

    #region AddOn
    public int MaxAddOnCound = 2;
    public Transform[] AddOnPos;
    public GameObject AddOnPrefab;
    #endregion

    //BaseCharacter의 초기화 함수를 오버라이드하여 스킬을 초기화하기 위한 함수 호출
    public override void Init(CharacterManager characterManager)
    {
        base.Init(characterManager);

        _playerAnimator = GetComponent<Animator>();

        int CurrentAddOnCount = GameInstance.instance.CurrentAddOnCount;

        for (int i = 0; i < CurrentAddOnCount; i++)
        {
            AddOnItem.SpawnAddOn(characterManager, AddOnPrefab, AddOnPos[i]);
        }

        InitializeSkills();
    }

    //오브젝트 파괴와 메인화면을 로드하여 사망과정을 진행하는 함수
    public void DeadProcess()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        UpdateMovement();
        UpdateSkillInput();
    }

    //GetAxis를 통한 플레이어의 입력을 받아 캐릭터의 움직임을 구현
    //카메라를 기준으로 화면의 끝을 넘지 못하게 하는 벡터 변수를 생성하여 플레이어가 화면 밖으로 나가지 않도록 위치 고정
    private void UpdateMovement()
    {
        _moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (_moveInput.x == 0) _playerAnimator.SetInteger("Move", 0);
        else _playerAnimator.SetInteger("Move", _moveInput.x < 0 ? 1 : 2);

        transform.Translate(new Vector3(_moveInput.x, _moveInput.y, 0f) * (MoveSpeed * Time.deltaTime));

        // 카메라의 좌측 하단은(0, 0, 0.0)이며, 우측 상단은(1.0 , 1.0)이다.
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        if (pos.y < 0f) pos.y = 0f;
        if (pos.y > 1f) pos.y = 1f;
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    //플레이어의 입력을 받아 스킬 사용 함수를 호출
    private void UpdateSkillInput()
    {
        if (Input.GetKey(KeyCode.Z)) ActivateSkill(EnumTypes.PlayerSkill.Primary);
        if (Input.GetKeyUp(KeyCode.X)) ActivateSkill(EnumTypes.PlayerSkill.Repair);
        if (Input.GetKeyUp(KeyCode.C)) ActivateSkill(EnumTypes.PlayerSkill.Bomb);
        if (Input.GetKeyUp(KeyCode.V)) ActivateSkill(EnumTypes.PlayerSkill.Freeze);
        if (Input.GetKeyUp(KeyCode.B)) ActivateSkill(EnumTypes.PlayerSkill.Shield);
        if (Input.GetKeyUp(KeyCode.N)) ActivateSkill(EnumTypes.PlayerSkill.Change);
    }

    //Skills 딕셔너리를 비우고 _skillPrefabs에 들어있는 프리팹들을 타입에 맞게 추가해주는 함수
    //추가로, GameInstance에서 현재 무기 레벨을 불러옴
    private void InitializeSkills()
    {
        Skills = new Dictionary<EnumTypes.PlayerSkill, BaseSkill>();

        for (int i = 0; i < _skillPrefabs.Length; i++)
        {
            AddSkill((EnumTypes.PlayerSkill)i, _skillPrefabs[i]);
        }

        CurrentWeaponLevel = GameInstance.instance.CurrentPlayerWeaponLevel;
    }

    //각 스킬 오브젝트를생성하고, 플레이어를 부모 오브젝트로 설정함
    //생성한 skillObject에 대한 초기화를 진행하고 Skills 딕셔너리에 추가함
    private void AddSkill(EnumTypes.PlayerSkill skillType, GameObject prefab)
    {
        GameObject skillObject = Instantiate(prefab, transform.position, Quaternion.identity);
        skillObject.transform.parent = this.transform;

        if (skillObject != null)
        {
            BaseSkill skillComponent = skillObject.GetComponent<BaseSkill>();
            skillComponent.Init(CharacterManager);
            Skills.Add(skillType, skillComponent);
        }
    }

    //스킬의 존재 여부와 쿨타임 여부를 검사하여 활성화시켜주는 함수
    private void ActivateSkill(EnumTypes.PlayerSkill skillType)
    {
        if (Skills.ContainsKey(skillType))
        {
            if (Skills[skillType].IsAvailable())
            {
                Skills[skillType].Activate();
            }
            else
            {
                if (skillType != EnumTypes.PlayerSkill.Primary)
                    GetComponent<PlayerUI>().NoticeSkillCooldown(skillType);
            }
        }
    }

    public void SetInvincibility(bool invin)
    {
        if (invin) //false 처리가 없어 지금은 의미 없는 구문
        {
            if (invincibilityCoroutine != null)  //실행중인 무적 코루틴이 있다면
            {
                StopCoroutine(invincibilityCoroutine); //진행중인 코루틴 중지
            }

            invincibilityCoroutine = StartCoroutine(InvincibilityCoroutine()); //코루틴을 다시 할당하여 무적시간 초기화
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        Invincibility = true; //무적 변수 true 설정
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>(); //스프라이트 렌더러 변수 할당

        // 무적 지속 시간 (초)
        float invincibilityDuration = 3f;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f); //무적시간동안 캐릭터 색상의 알파값을 절반으로 감소

        // 무적이 해제될 때까지 대기
        yield return new WaitForSeconds(invincibilityDuration);

        // 타이머가 만료되면 무적을 비활성화
        Invincibility = false;
        spriteRenderer.color = new Color(1, 1, 1, 1f); //색상 원상복구
    }

    public void PlusAddOnCount(int v)
    {
        GameInstance.instance.CurrentAddOnCount += v;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Item")
        {
            if (collision.GetComponent<BaseItem>() == null) return;

            collision.GetComponent<BaseItem>().OnGetItem(CharacterManager);
            Destroy(collision.gameObject);
        }
    }
}