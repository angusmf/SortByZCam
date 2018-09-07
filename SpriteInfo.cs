using UnityEngine;

[ExecuteInEditMode]
public class SpriteInfo : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    public Vector3 localTopRight;
    public int initialOffset;
    public int zOrder;
    public int rotationOffset;

    public bool dirty = false;
   

    private void Start()
    {
        SortByZCam.Inst.AddSpriteInfo(this);
    }

}

