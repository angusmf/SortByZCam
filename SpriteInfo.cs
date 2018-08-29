using UnityEngine;



public class SpriteInfo : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    public Vector3 localTopRight;
    public int initialOffset;
    public int sortOrder;
    public int rotationOffset;

    public bool dirty = false;

    private void Awake()
    {
        SortByZCam.CountSprite();
    }

    private void Start()
    {



        SortByZCam.Inst.AddSpriteInfo(this);
    }



}

