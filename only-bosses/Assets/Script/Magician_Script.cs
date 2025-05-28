using UnityEngine;

public class Magician_Script : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator animator;
    [SerializeField] private HPBarUI magicianHPBar;

    private int baseAttackCount;
    private bool isBaseAttack = false;
    private PlayerStatus status;

    private int currentHp;

    private int elementCount;
    private const int MAX_ELEMNT = 1;

    public Sprite skillQCastSprite;
    private int firstSkillCoolTime;
    private int secondSkillCoolTime;
    private int thirdSkillCoolTime;

    // 보스1 상대로 스킬 맞는거 
    private bool isFeared = false;
    private Vector2 fearDirection;
    private float fearTimer = 0f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isBaseAttack = false;
        status = new PlayerStatus(450, 2, 20, 6, 5, 30, 5, 0, 0);

        currentHp = status.getHp();

        // 장비에 따라 status 변환 




        firstSkillCoolTime = 0;
        secondSkillCoolTime = 0;
        thirdSkillCoolTime = 0;
    }

    void FixedUpdate()
    {
        if (!isBaseAttack)
        {
            if (--baseAttackCount <= 0)
            {
                isBaseAttack = true;
                elementCount = MAX_ELEMNT;
            }
        }

        if (firstSkillCoolTime > 0) firstSkillCoolTime--;
        if (secondSkillCoolTime > 0) secondSkillCoolTime--;
        if (thirdSkillCoolTime > 0) thirdSkillCoolTime--;
    }

    void Update()
    {
        bool isWalking = Input.GetKey("a") || Input.GetKey("d");
        animator.SetBool("IsWalking", isWalking);

        if (Input.GetMouseButtonDown(0))
        {
            if (baseAttack())
            {
                animator.SetTrigger("AttackA");
            }
        }

        if (Input.GetKey("q")) useFirstSkill();

        Transform fireTransform = transform.Find("magicianQ(Clone)");
        if (fireTransform != null)
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 pos = transform.position;
            Vector2 dir = (worldPos - pos).normalized;

            if (dir.x > 0)
            {
                fireTransform.localPosition = new Vector3(1.0f, 0f, -1f);
                fireTransform.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                fireTransform.localPosition = new Vector3(-1.0f, 0f, -1f);
                fireTransform.GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        bool skillE = Input.GetKey("e") && secondSkillCoolTime <= 0;
        animator.SetBool("SkillE", skillE);

        bool skillR = Input.GetKey("r") && thirdSkillCoolTime <= 0;
        animator.SetBool("SkillR", skillR);

        if (isFeared)
        {
            transform.Translate(fearDirection * 2f * Time.deltaTime); 
            fearTimer -= Time.deltaTime;
            if (fearTimer <= 0f)
            {
                isFeared = false;
            }
            return; 
        }
    }

    public bool baseAttack()
    {
        if (!isBaseAttack) return false;

        if (!Physics2D.Raycast(transform.position, Vector3.down, 1F, LayerMask.GetMask("Ground"))) // ?
        {
            Rigidbody2D rbody = GetComponent<Rigidbody2D>();
            if (rbody.linearVelocity.y < 0)
                rbody.AddForce(new Vector2(0, transform.position.y + 5), ForceMode2D.Impulse);
        }

        int dmg = status.getDamage();

        if (--elementCount <= 0)
        {
            dmg = (int)(dmg * 1.2); // ?
            baseAttackCount = status.getAttackSpeed() * 50;
            isBaseAttack = false;
        }

        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 pos = transform.position;
        Vector2 dir = (worldPos - pos).normalized;

        string[] elementNames = { "fire", "ice", "bolt" };
        string selectedElement = elementNames[Random.Range(0, elementNames.Length)];

        GameObject elementPrefab = Resources.Load<GameObject>($"etc/{selectedElement}");

        elementPrefab.transform.position = transform.position;
        Element element = elementPrefab.GetComponent<Element>();
        element.playerObject = gameObject;

        element.dmg = dmg;
        element.MaxDistance = status.getAttackDistance();
        elementPrefab.GetComponent<SpriteRenderer>().flipX = dir.x < 0;
        element.vector = dir;

        Instantiate(elementPrefab, elementPrefab.transform.position, elementPrefab.transform.rotation);

        return true;
    }

    public void useFirstSkill()
    {
        if (firstSkillCoolTime > 0) return;

        firstSkillCoolTime = 12 * 50;

        sr.sprite = skillQCastSprite;

        animator.enabled = false;

        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 pos = transform.position;
        Vector2 dir = (worldPos - pos).normalized;

        GameObject magicianQPrefab = Resources.Load<GameObject>("etc/magicianQ");

        GameObject instance = Instantiate(magicianQPrefab, transform.position, Quaternion.identity);

        instance.transform.SetParent(transform);

        if (dir.x > 0)
        {
            instance.transform.localPosition = new Vector3(1.0f, 0f, -1f);
            instance.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            instance.transform.localPosition = new Vector3(-1.0f, 0f, -1f);
            instance.GetComponent<SpriteRenderer>().flipX = true;
        }

        Magician_skill1 script = instance.GetComponent<Magician_skill1>();
        script.dmg = (int)(status.getDamage() * 1.8f);

        Invoke(nameof(ResumeIdle), 2f);
    }

    public void useSecondSkill()
    {

        secondSkillCoolTime = 20 * 50;

        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        GameObject eSkillPrefab = Resources.Load<GameObject>("etc/magicianE");

        Vector3 spawnPos = new Vector3(worldPos.x, worldPos.y, -1f);

        GameObject instance = Instantiate(eSkillPrefab, spawnPos, Quaternion.identity);

        Magician_skill2 script = instance.GetComponent<Magician_skill2>();

        script.dmg = (int)(status.getDamage() * 1.8f);
    }

    public void useThirdSkill()
    {

        thirdSkillCoolTime = 15 * 50;

        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        GameObject rSkillPrefab = Resources.Load<GameObject>("etc/magicianR");

        Vector3 spawnPos = new Vector3(worldPos.x, worldPos.y, -1f);

        GameObject instance = Instantiate(rSkillPrefab, spawnPos, Quaternion.identity);

        Magician_skill3 script = instance.GetComponent<Magician_skill3>();

        script.dmg = (int)(status.getDamage() * 1.8f);
    }

    private void ResumeIdle()
    {
        animator.enabled = true;
        animator.Play("magician_idle");
    }

    public void SetMagicianHPBar(HPBarUI hpBar)
    {
        magicianHPBar = hpBar;
    }

    public void TakeDamage(int dmg)
    {
        currentHp -= dmg;
        Debug.Log($"받은 데미지: {dmg} | 현재 HP: {currentHp}");

        magicianHPBar.SetHP(currentHp, status.getHp());
    }

    public void Fear(Vector3 bossPos)
    {
        isFeared = true;
        fearTimer = 2f;
        Vector2 dirToBoss = (bossPos - transform.position).normalized;
        fearDirection = -dirToBoss;
    }
}
