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
<<<<<<< HEAD
=======
    {
        return damage;
    }

    void Start()
>>>>>>> 269bf8225ca31a2feb56416e61c53779f683cf1c
    {
        return damage;
    }

    public int getCriticalChance()
    {
        return criticalChance;
    }

    public int getCriticalDamage()
    {
        return criticalDamage;
    }

    public int getAttackSpeed()
    {
        return attackSpeed;
    }
}
