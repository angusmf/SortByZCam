using UnityEngine;

[ExecuteInEditMode]
public class SpriteInfo : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    public bool executeInEdit = false;

    public Vector3 localTopRight;
    public int initialOffset;
    public int zOrder;
    public int rotationOffset;

    public bool dirty = false;
   

    private void Start()
    {
        if (Application.isEditor && !executeInEdit) return;

        SortByZCam.Inst.AddSpriteInfo(this);
    }

}

