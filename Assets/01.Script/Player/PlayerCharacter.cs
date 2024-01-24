using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//ĳ���Ͱ� ����� ��ų���̳� ������ ���� ó���ϴ� Ŭ����
//�����̳� ���� �� �÷��̾��� ���¿� ���õ� �ڵ嵵 �̰����� ������
public class PlayerCharacter : BaseCharacter
{
    #region Movement
    private Vector2 _moveInput;
    private Animator _playerAnimator;
    public float MoveSpeed;

    #endregion
    //�÷��̾��� �̵��� ������ ���� ����

    #region Skills
    [HideInInspector] public Dictionary<EnumTypes.PlayerSkill, BaseSkill> Skills;
    [SerializeField] private GameObject[] _skillPrefabs;
    #endregion
    //��ų�� ���� ������ ���� ��ųʸ��� ��ų�����յ��� ���� �迭

    #region Invincibility
    private bool invincibility; //���� ����
    private Coroutine invincibilityCoroutine; //�����ð� �ڷ�ƾ�� �����ϱ� ���� ����
    private const double InvincibilityDurationInSeconds = 3; // ���� ���� �ð� (��), �����ѹ� ������
    //ĳ������ ���� ���ο� ���� �ð��� ���� ����, �׸��� ���� ���� �ð��� ����� �ڷ�ƾ
    public bool Invincibility //private ������ �ܺο����� ó���ϱ� ���� getter, setter
    {
        get { return invincibility; }
        set { invincibility = value; }
    }
    //���� ���θ� ��ȯ�ϰ�, �����ϴ°��� ���ϰ� �ϱ� ���� �Լ�
    #endregion

    #region Weapon
    public int CurrentWeaponLevel = 0;
    public int MaxWeaponLevel = 5;
    //ĳ������ ���� ���� ������ �ִ� ���� ������ ��� ����
    #endregion

    #region AddOn
    public int MaxAddOnCound = 2;
    public Transform[] AddOnPos;
    public GameObject AddOnPrefab;
    #endregion

    //BaseCharacter�� �ʱ�ȭ �Լ��� �������̵��Ͽ� ��ų�� �ʱ�ȭ�ϱ� ���� �Լ� ȣ��
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

    //������Ʈ �ı��� ����ȭ���� �ε��Ͽ� ��������� �����ϴ� �Լ�
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

    //GetAxis�� ���� �÷��̾��� �Է��� �޾� ĳ������ �������� ����
    //ī�޶� �������� ȭ���� ���� ���� ���ϰ� �ϴ� ���� ������ �����Ͽ� �÷��̾ ȭ�� ������ ������ �ʵ��� ��ġ ����
    private void UpdateMovement()
    {
        _moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (_moveInput.x == 0) _playerAnimator.SetInteger("Move", 0);
        else _playerAnimator.SetInteger("Move", _moveInput.x < 0 ? 1 : 2);

        transform.Translate(new Vector3(_moveInput.x, _moveInput.y, 0f) * (MoveSpeed * Time.deltaTime));

        // ī�޶��� ���� �ϴ���(0, 0, 0.0)�̸�, ���� �����(1.0 , 1.0)�̴�.
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        if (pos.y < 0f) pos.y = 0f;
        if (pos.y > 1f) pos.y = 1f;
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    //�÷��̾��� �Է��� �޾� ��ų ��� �Լ��� ȣ��
    private void UpdateSkillInput()
    {
        if (Input.GetKey(KeyCode.Z)) ActivateSkill(EnumTypes.PlayerSkill.Primary);
        if (Input.GetKeyUp(KeyCode.X)) ActivateSkill(EnumTypes.PlayerSkill.Repair);
        if (Input.GetKeyUp(KeyCode.C)) ActivateSkill(EnumTypes.PlayerSkill.Bomb);
        if (Input.GetKeyUp(KeyCode.V)) ActivateSkill(EnumTypes.PlayerSkill.Freeze);
        if (Input.GetKeyUp(KeyCode.B)) ActivateSkill(EnumTypes.PlayerSkill.Shield);
        if (Input.GetKeyUp(KeyCode.N)) ActivateSkill(EnumTypes.PlayerSkill.Change);
    }

    //Skills ��ųʸ��� ���� _skillPrefabs�� ����ִ� �����յ��� Ÿ�Կ� �°� �߰����ִ� �Լ�
    //�߰���, GameInstance���� ���� ���� ������ �ҷ���
    private void InitializeSkills()
    {
        Skills = new Dictionary<EnumTypes.PlayerSkill, BaseSkill>();

        for (int i = 0; i < _skillPrefabs.Length; i++)
        {
            AddSkill((EnumTypes.PlayerSkill)i, _skillPrefabs[i]);
        }

        CurrentWeaponLevel = GameInstance.instance.CurrentPlayerWeaponLevel;
    }

    //�� ��ų ������Ʈ�������ϰ�, �÷��̾ �θ� ������Ʈ�� ������
    //������ skillObject�� ���� �ʱ�ȭ�� �����ϰ� Skills ��ųʸ��� �߰���
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

    //��ų�� ���� ���ο� ��Ÿ�� ���θ� �˻��Ͽ� Ȱ��ȭ�����ִ� �Լ�
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
        if (invin) //false ó���� ���� ������ �ǹ� ���� ����
        {
            if (invincibilityCoroutine != null)  //�������� ���� �ڷ�ƾ�� �ִٸ�
            {
                StopCoroutine(invincibilityCoroutine); //�������� �ڷ�ƾ ����
            }

            invincibilityCoroutine = StartCoroutine(InvincibilityCoroutine()); //�ڷ�ƾ�� �ٽ� �Ҵ��Ͽ� �����ð� �ʱ�ȭ
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        Invincibility = true; //���� ���� true ����
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>(); //��������Ʈ ������ ���� �Ҵ�

        // ���� ���� �ð� (��)
        float invincibilityDuration = 3f;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f); //�����ð����� ĳ���� ������ ���İ��� �������� ����

        // ������ ������ ������ ���
        yield return new WaitForSeconds(invincibilityDuration);

        // Ÿ�̸Ӱ� ����Ǹ� ������ ��Ȱ��ȭ
        Invincibility = false;
        spriteRenderer.color = new Color(1, 1, 1, 1f); //���� ���󺹱�
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