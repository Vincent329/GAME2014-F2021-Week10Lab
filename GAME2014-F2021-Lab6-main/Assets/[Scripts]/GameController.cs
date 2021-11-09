using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform player, playerSpawnPoint;

    private void Start()
    {
        player.position = playerSpawnPoint.position;
    }
}
