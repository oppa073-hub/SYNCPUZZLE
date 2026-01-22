using UnityEngine;

public class PuzzleDoor2D : MonoBehaviour
{
    [SerializeField] private LaserSensor2D[] requiredSensors;
    [SerializeField] private bool stayOpen = true;
    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private SpriteRenderer doorSprite;

    private bool isOpen;

    private void Awake()
    {
        if (!doorCollider) doorCollider = GetComponent<Collider2D>();
        if (!doorSprite) doorSprite = GetComponentInChildren<SpriteRenderer>();
        ApplyDoorVisual(false);
    }

    private void Update()
    {
        if (isOpen && stayOpen) return;

        bool allOn = true;
        for (int i = 0; i < requiredSensors.Length; i++)
        {
           // if (requiredSensors[i] == null || !requiredSensors[i].isLit)
           // {
           //     allOn = false;
           //     break;
           // }
        }

        if (allOn)
        {
            OpenDoor();
        }
        else if (!stayOpen)
        {
            CloseDoor();
        }
    }
    private void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;
        ApplyDoorVisual(true);
    }
    private void CloseDoor()
    {
        if (!isOpen) return;
        isOpen = false;
        ApplyDoorVisual(false);
    }

    private void ApplyDoorVisual(bool open)
    {
        if (doorCollider) doorCollider.enabled = !open;
        if (doorSprite) doorSprite.enabled = !open;
    }
}
