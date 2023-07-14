using System;
using UnityEngine;

[Serializable]
public class GeneralConfig
{
    //the height of the playing field used for enemy spawn positions
    public float playingFieldHeight;

    //the width of the playing field used for enemy spawn positions
    public float playingFieldWidth;

    //how far away from the x axis heal areas can spawn
    public float healAreaBoundsVertical;

    //how far away from the y axis heal areas can spawn
    public float healAreaBoundsHorizontal;

    //how far away from the x axis player can go
    public float playerBoundsVertical;

    //how far away from the y axis player can go
    public float playerBoundsHorizontal;

    //how many seconds the simulation waits before loading the next level
    public float levelEndTime;

    //how many seconds the indicator for the lasers lasts
    public float laserIndicatorDuration;

    //how many seconds the laser lasts/damages
    public float laserDuration;

    //how many seconds the indicator for the heal area lasts
    public float healAreaIndicatorDuration;

    //how many seconds the heal area lasts/heals
    public float healAreaDuration;

    //how many seconds at the start of the level without the player attacking
    public float initialAttackCD;

    //the maximum health points of the player
    public int playerMaxHP;

    //how close an enemy has to be to the player to hit it
    public float enemyHitDistance;

    //how many seconds pass before the enemy adjusts his direction towards the player again
    public float directionChangeCD;

    //for how many seconds after spawn the enemies are invulnerable
    public float enemyInvulnerabilityTime;
}