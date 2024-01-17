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
    public float MoveSpeed;
    #endregion
    //플레이어의 이동속도와 움직임 방향 변수

    #region Skills
    [HideInInspector] public Dictionary<EnumTypes.PlayerSkill, BaseSkill> Skills;
    [SerializeField] private GameObject[] _skillPrefabs;
    #endregion
    //스킬에 대한 정보를 담을 딕셔너리와 스킬프리팹등을 담을 배열

    #region Invincibility
    private bool invincibility;
    private Coroutine invincibilityCoroutine;
    private const double InvincibilityDurationInSeconds = 3; // 무적 지속 시간 (초)
    //캐릭터의 무적 여부와 무적 시간을 담을 변수, 그리고 남은 무적 시간을 계산할 코루틴
    public bool Invincibility
    {
        get { return invincibility; }
        set { invincibility = value; }
    }
    //무적 여부를 반환하고, 설정하는것을 편하게 하기 위한 함수
    #endregion

    #region Weapon
    public int CurrentWeaponLevel = 0;
    public int MaxWeaponLevel = 3;
    //캐릭터의 현재 무기 레벨과 최대 무기 레벨을 담는 변수
    #endregion

    //BaseCharacter의 초기화 함수를 오버라이드하여 스킬을 초기화하기 위한 함수 호출
    public override void Init(CharacterManager characterManager)
    {
        base.Init(characterManager);
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
            //else
            //{
            //    if (skillType != EnumTypes.PlayerSkill.Primary)
            //        GetComponent<PlayerUI>().NoticeSkillCooldown(skillType);
            //}
        }
    }
}