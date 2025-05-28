using UnityEngine;

public class BossStatus
{
    
    private int health;
    private int attackSpeed;
    private int damage;
    private int attackDistance;
    private int criticalChance;
    private int criticalDamage;
    private int moveSpeed;
    
    public BossStatus(int health, int attackSpeed, int damage, int attackDistance, int criticalChance, int criticalDamage, int moveSpeed)
    {
        this.health = health;
        this.attackSpeed = attackSpeed;
        this.damage = damage;
        this.attackDistance = attackDistance;
        this.criticalChance = criticalChance;
        this.criticalDamage = criticalDamage;
        this.moveSpeed = moveSpeed;
    }

    public int getHp()
    {
        return health;
    }

    public int getMoveSpeed()
    {
        return moveSpeed;
    }

    public int getDamage()
    {
        return damage;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
