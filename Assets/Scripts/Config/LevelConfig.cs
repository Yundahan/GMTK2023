using System;
using UnityEngine;

[Serializable]
public class LevelConfig
{
    //the name of the next level
    public string nextLevelName;

    //how many seconds pass between two enemies spawning
    public float enemySpawnCD;

    //how many seconds pass between two heal areas spawning
    public float healAreaCD;

    //how much the player heals for shooting an enemy
    public int healValue;

    //how much the player is damaged for getting hit
    public int damageValue;

    //true if heal areas are active in the level, false otherwise
    public bool healAreasActive;

    //how many seconds pass between two player attacks
    public float playerAttackCD;

    //the inital mode of the laser in the level. 0 = +, 1 = x
    public int laserMode;

    //true if shot direction alternates between shots, false if it stays constant
    public bool alternateShotDirection;

    //the amount of health gained from heal areas
    public int healAreaHealAmount;

    //the speed of the enemies
    public float enemySpeed;

    //the speed of the player
    public float playerSpeed;
}