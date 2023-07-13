using System;
using UnityEngine;

[Serializable]
public class LevelConfig
{
    public float enemySpawnCD;
    public float healAreaCD;
    public int healValue;
    public int damageValue;
    public bool healAreasActive;
    public float playerAttackCD;
    public int laserMode;// 0 = +, 1 = x
    public bool alternateShotDirection;
}