using UnityEngine;

public class BossStatus
{

    private int maxHealth;
    private int health;
    private int attackSpeed;
    private int damage;
    private int attackDistance;
    private int criticalChance;
    private int criticalDamage;
    private int moveSpeed;

    public BossStatus(int maxHealth, int health, int attackSpeed, int damage, int attackDistance, int criticalChance, int criticalDamage, int moveSpeed)
    {
        this.maxHealth = maxHealth;
        this.health = health;
        this.attackSpeed = attackSpeed;
        this.damage = damage;
        this.attackDistance = attackDistance;
        this.criticalChance = criticalChance;
        this.criticalDamage = criticalDamage;
        this.moveSpeed = moveSpeed;
    }

    public int getMaxHealth()
    {
        return maxHealth;
    }

    public int getHealth()
    {
        return health;
    }

    public void setHealth(int health)
    {
        this.health = health;
    }

    public int getMoveSpeed()
    {
        return moveSpeed;
    }

    public int getDamage()
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
