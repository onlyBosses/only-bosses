using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerStatus
{
    private int maxHealth;
    private int health;
    private int attackSpeed;
    private int damage;
    private int attackDistance;
    private int criticalChance;
    private int criticalDamage;
    private float moveSpeed;
    private int reduceCoolTime;
    private int increasedSkillDamage;

    // private ArrayList<string> selectedItem;

    public PlayerStatus(int maxHealth, int health, int attackSpeed, int damage, int attackDistance, int criticalChance, int criticalDamage, float moveSpeed, int reduceCoolTime, int increasedSkillDamage)
    {
        this.maxHealth = maxHealth;
        this.health = health;
        this.attackSpeed = attackSpeed;
        this.damage = damage;
        this.attackDistance = attackDistance;
        this.criticalChance = criticalChance;
        this.criticalDamage = criticalDamage;
        this.moveSpeed = moveSpeed;
        this.reduceCoolTime = reduceCoolTime;
        this.increasedSkillDamage = increasedSkillDamage;
    }

    public int getMaxHealth()
    {
        return maxHealth;
    }

    public void setMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public int getHealth()
    {
        return health;
    }

    public void setHealth(int health)
    {
        this.health = health;
    }

    public int getDamage()
    {
        return damage;
    }

    public void setDamage(int damage)
    {
        this.damage = damage;
    }

    public int getAttackSpeed()
    {
        return attackSpeed;
    }

    public int getAttackDistance()
    {
        return attackDistance;
    }

    public void setAttackDistance(int attackDistance)
    {
        this.attackDistance = attackDistance;
    }

    public int getCriticalChance()
    {
        return criticalChance;
    }

    public void setCriticalChance(int criticalChance)
    {
        this.criticalChance = criticalChance;
    }

    public int getCriticalDamage()
    {
        return criticalDamage;
    }

    public void setCriticalDamage(int criticalDamage)
    {
        this.criticalDamage = criticalDamage;
    }

    public float getMoveSpeed()
    {
        return moveSpeed;
    }

    public void setMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
}
