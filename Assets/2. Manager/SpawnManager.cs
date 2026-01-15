using Photon;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class SpawnManager : MonoBehaviourPunCallbacks
{
    public static SpawnManager Instance { get; private set; }
    [SerializeField] GameObject playerPrefab;
    [SerializeField] List<Vector3> Spawn;
    int index;
    Player player;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }


    }
 
    private  void Start()
    {
        WaitRoom();
        if (!HasLocalPlayer())
        {
            StartCoroutine(SpawnPlayerWhenConnected());  //룸에 접속했을때 플레이어 스폰
        }
      if (playerPrefab.tag == playerPrefab.name) return;

    }
    IEnumerator SpawnPlayerWhenConnected()
    {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        player = PhotonNetwork.LocalPlayer;
        int raw = player.ActorNumber - 1;
        index = Math.Clamp(raw, 0, Spawn.Count - 1);
        PhotonNetwork.Instantiate(playerPrefab.name, Spawn[index], Quaternion.identity);
    }
    IEnumerator WaitRoom()
    {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        if (HasLocalPlayer()) yield break;
    }
    private bool HasLocalPlayer()
    {
        if (!PhotonNetwork.InRoom) return false;
        
        PhotonView[] views = FindObjectsByType<PhotonView>(FindObjectsSortMode.None);

        foreach (PhotonView view in views)
        {
            if (!view.IsMine) continue;

            if (view.GetComponent<playerController>() != null)
            {
                return true;
            }
        }
        return false;
    }
}
