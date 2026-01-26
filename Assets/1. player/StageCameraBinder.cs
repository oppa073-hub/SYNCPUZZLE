using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using Photon.Pun;

public class ConfinerBinder : MonoBehaviour
{
    [SerializeField] private Collider2D stageConfiner;

    private IEnumerator Start()
    {
        // 1 confiner 콜라이더는 반드시 있어야 함
        if (stageConfiner == null)
        {
            yield break;
        }

        // 2 로컬 플레이어 카메라가 생길 때까지 대기
        CinemachineConfiner2D confiner = null;

        for (int i = 0; i < 120; i++) // 2초 정도
        {
            confiner = FindLocalPlayerConfiner();
            if (confiner != null) break;
            yield return null;
        }

        // 3 바운딩 수ㅔ이프 연결
        confiner.BoundingShape2D = stageConfiner;
        confiner.InvalidateBoundingShapeCache();

    }

    private CinemachineConfiner2D FindLocalPlayerConfiner()
    {
        // 씬에 있는 모든 CinemachineCamera 중에서
        var cams = FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);

        foreach (var cam in cams)
        {
            // 그 카메라가 들어있는 플레이어에 PhotonView가 있고 내 것인지 확인
            var pv = cam.GetComponentInParent<PhotonView>();
            if (pv == null || !pv.IsMine) continue;

            // 내 카메라의 Confiner2D 찾기
            var confiner = cam.GetComponent<CinemachineConfiner2D>();
            if (confiner != null) return confiner;
        }

        return null;
    }
}
