using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    public GameObject PlayerPrefab;
    [SerializeField] private Room startingRoom;
    
    /// <summary>
    /// set on awake
    /// </summary>
    [HideInInspector] public Room CurrentRoom;
    [HideInInspector] public Player Player;
    [HideInInspector] public Tilemap Map;

    private Camera cam;

    void Awake() 
    {
        CurrentRoom = startingRoom;
        cam = Camera.main;
        
        Map = GetComponent<Tilemap>();
        Map.CompressBounds();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Room room))
                room.Level = this;
        }
    }

    private void Start()
    {
        SpawnPlayer();
        
        float x = startingRoom.OriginWS.x + 160;
        float y = startingRoom.OriginWS.y + 94;
        cam.transform.position = new Vector3(x, y, -10);
    }

    public void SwitchRoom(Room nextRoom)
    {
        CurrentRoom = nextRoom;
        FreezeLevel(1f);
        Player.OnSwitchRoom(nextRoom);
    }
    
    /// <summary>
    /// 플레이어는 항상 레벨아래에 위치하기 때문에 스폰도 룸이 아닌 레벨단위가 맞다.
    /// </summary>
    public void SpawnPlayer()
    {
        Player = Instantiate(PlayerPrefab, (Vector3Int)CurrentRoom.SpawnPos, 
            quaternion.identity).GetComponent<Player>();
    }
    
    public void FreezeLevel(float time)
    {
        Player.IsPaused = true;
        
        // adjust camera
        float x = CurrentRoom.OriginWS.x + 160;
        float y = CurrentRoom.OriginWS.y + 94;
        cam.transform.DOMove(new Vector3(x, y, -10), time)
            .SetEase(Ease.OutCubic)
            .OnComplete(()=> Player.IsPaused = false);
    }

    public void Shake(float duration, float strength)
    {
        cam.DOShakePosition(duration, strength);
    }
    
    public void DirectionalShake(Vector2 dashDir, float duration, float strength)
    {
        // TODO: shake to direction
        cam.DOShakePosition(duration, strength);
    }
} 