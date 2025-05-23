using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerStatus
{
    private int health;
    private int attackSpeed;
    private int damage;
    private int attackDistance;
    private int criticalChance;
    private int criticalDamage;
    private int moveSpeed;
    private int reduceCoolTime;
    private int increasedSkillDamage;

    // private ArrayList<string> selectedItem;

    public PlayerStatus(int health, int attackSpeed, int damage, int attackDistance, int criticalChance, int criticalDamage, int moveSpeed, int reduceCoolTime, int increasedSkillDamage)
    {
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
}
