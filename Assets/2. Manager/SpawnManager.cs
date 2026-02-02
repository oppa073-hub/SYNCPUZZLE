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
        if (!HasLocalPlayer() && Spawn.Count > 0)
        {
            StartCoroutine(SpawnPlayerWhenConnected());  //룸에 접속했을때 플레이어 스폰
        }
    }
    IEnumerator SpawnPlayerWhenConnected()
    {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        if (HasLocalPlayer()) yield break;
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
    public Vector3 GetSpawnPosForActor(int actorNumber)
    {
        if (Spawn == null || Spawn.Count == 0) return Vector3.zero;

        int raw = actorNumber - 1;
        int idx = Mathf.Clamp(raw, 0, Spawn.Count - 1);
        return Spawn[idx];
    }
    [PunRPC]
    private void RPC_RespawnAll()
    {
        // 각 클라에서 "내 플레이어"만 찾아서 텔레포트
        var controllers = FindObjectsByType<playerController>(FindObjectsSortMode.None);

        for (int i = 0; i < controllers.Length; i++)
        {
            var pc = controllers[i];
            if (pc == null) continue;
            if (pc.photonView == null) continue;
            if (!pc.photonView.IsMine) continue;   // 로컬 플레이어만

            Vector3 spawnPos = GetSpawnPosForActor(PhotonNetwork.LocalPlayer.ActorNumber);
            pc.TeleportTo(spawnPos);            
            break;
        }
    }
    public void RequestRespawnAll()
    {
        if (!PhotonNetwork.InRoom) return;

        //마스터만 전체 리스폰을 지시
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC(nameof(RPC_RespawnAll), RpcTarget.All);
        else
            photonView.RPC(nameof(RPC_RequestRespawnAll), RpcTarget.MasterClient);
    }

    [PunRPC]
    private void RPC_RequestRespawnAll()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        photonView.RPC(nameof(RPC_RespawnAll), RpcTarget.All);
    }

}
