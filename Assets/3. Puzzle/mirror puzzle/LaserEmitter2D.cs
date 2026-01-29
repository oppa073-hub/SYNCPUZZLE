using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter2D : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer line;

    [Header("Ray Settings")]
    [SerializeField] private LayerMask hitMask;     
    [SerializeField] private int maxBounces = 15;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private float hitOffset = 0.2f;

    [Header("State")]
    [SerializeField] private bool laserOn = true;

    private readonly List<Vector3> points = new List<Vector3>(32);
    private void Update()
    {
        if (!laserOn)
        {
            if (line) line.positionCount = 0; 
            return;
        }

        DrawLaserPath();
    }

    private void DrawLaserPath()
    {
        points.Clear();

        Vector2 startPos = firePoint.position;
        Vector2 dir = firePoint.right.normalized;

        points.Add(startPos);

        for (int bounce = 0; bounce < maxBounces; bounce++)
        { 
            RaycastHit2D hit = Physics2D.Raycast(startPos, dir, maxDistance,hitMask);

            // 아무것도 안 맞으면 최대 거리까지 그려주고 종료
            if (hit.collider == null)
            {
                Vector2 endPos = startPos + dir * maxDistance;
                points.Add(endPos);
                break;
            }
            // 맞은 지점 추가
            points.Add(hit.point);
            int hitLayer = hit.collider.gameObject.layer;


            // 1 Mirror면: 방향 바꾸고 계속
            if (hitLayer == LayerMask.NameToLayer("Mirror"))
            {
                Vector2 normal = hit.normal;
                dir = Vector2.Reflect(dir, normal).normalized;


                startPos = hit.point + normal * hitOffset;

                continue;
            }
            // 2 Sensor면: 종료
            if (hitLayer == LayerMask.NameToLayer("Sensor"))
            {
                hit.collider.GetComponent<LaserSensor2D>()?.MarkLitThisFrame();
                break;
            }
            // 3 Wall: 종료
            break;
        }
        // LineRenderer에 한 번에 적용
        line.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            line.SetPosition(i, points[i]);
        }

    }
    public void SetLaserOn(bool on) => laserOn = on;
}
