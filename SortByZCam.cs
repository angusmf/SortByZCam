using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class SortByZCam : MonoBehaviour
{
    #region Inspector items
    public int worldScale = 1000;
    public string LayerName = "Objects";

    const int upperBound = 32767;
    const int lowerBound = -32767;

    #endregion

    public bool executeInEdit = false;

    List<SpriteInfo> sprites = new List<SpriteInfo>();

    //when all sprites need to have their order re-set
    bool dirty = false;

    //flag used when setting sprite ordering
    bool facingForward;

    bool initialized = false;

    //Count of sprites with SpriteInfo components
    static int spriteCount = 0;

    //Singleton instance ref
    public static SortByZCam Inst;

    //garbage catcher
    SpriteInfo[] spriteArr;

    #region Public Methods 

    //called by SpriteInfo in Start to add themselves to our List
    public void AddSpriteInfo(SpriteInfo si)
    {
        sprites.Add(si);

        si.spriteRenderer = si.GetComponent<SpriteRenderer>();

        //get the size of the sprite even if it is rotated to start with
        Quaternion rot = si.transform.rotation;
        si.transform.rotation = Quaternion.identity;
        si.localTopRight = new Vector3(si.spriteRenderer.bounds.extents.x, si.spriteRenderer.bounds.extents.y, 0) + si.transform.position;
        si.localTopRight = si.transform.InverseTransformPoint(si.localTopRight);
        si.transform.rotation = rot;

        //read the initial sort order
        //si.zOrder = si.spriteRenderer.sortingOrder;
        //and stash the initial sort order as an offset
        //if a non-zero value is already serialized into SpriteInfo, we take that, otherwise we take the value from the renderer.
        //        si.initialOffset = (si.initialOffset == 0) ? si. initialOffset : si.sortOrder;
        //si.initialOffset = si.sortOrder;
        si.zOrder = 0;

        si.spriteRenderer.sortingLayerName = LayerName;
    }

    #endregion

    #region Sprite Processing

    //apply calculations and offsets to get current correct sorting order for a sprite
    bool ProcessSprite(SpriteInfo si)
    {
        if (!transform.hasChanged) return false;

        int z = GetZOrder(si);
        int r = GetRotationOffsetByZ(si);

        if (si.zOrder != z - si.initialOffset || si.rotationOffset != r)
        {
            si.dirty = true;
            si.zOrder = z;
            si.rotationOffset = r;
            return true;
        }
        return false;
    }

    //apply the sprite ordering for a sprite with correction for camera-forwardness
    void SetSpriteOrder(SpriteInfo si)
    {
        int newOrder = 0;
        newOrder = (facingForward) ? si.zOrder * -1 : si.zOrder;
        newOrder += (facingForward) ? si.initialOffset : si.initialOffset * -1;
        newOrder += si.rotationOffset;

        if (newOrder > upperBound) newOrder = upperBound;
        if (newOrder < lowerBound) newOrder = lowerBound;

        si.spriteRenderer.sortingOrder = newOrder;
        si.dirty = false;
    }

    #endregion

    #region Calcs

    private bool CheckForwardness()
    {
        if (Vector3.Dot(transform.forward, Vector3.forward) > 0) return true;
        return false;
    }

    private int GetZOrder(SpriteInfo si)
    {
        return (int)(si.transform.position.z * worldScale);
    }

    private int GetRotationOffsetByZ(SpriteInfo si)
    {
        return (int)((si.transform.position.z - si.transform.TransformPoint(si.localTopRight).z) * worldScale / 2);
    }

    #endregion

    #region Unity A-S-U

    private void Awake()
    {
        if (Inst != null)
        {
            Destroy(gameObject);
            return;
        }
        else Inst = this;
    }


    private void Start()
    {
        if (Application.isEditor && !executeInEdit) return;

        StartCoroutine(WaitForSprites());
    }

    IEnumerator WaitForSprites()
    {
        yield return new WaitForEndOfFrame();

        spriteCount = sprites.Count;
        spriteArr = sprites.ToArray();
        for (int i = 0; i < spriteCount; i++)
        {
            ProcessSprite(spriteArr[i]);
        }
        initialized = true;
    }

    private void Update()
    {
        if (Application.isEditor && !executeInEdit) return;
        if (!initialized) return;

        if (transform.hasChanged)
        {
            if (facingForward != CheckForwardness())
            {
                facingForward = !facingForward;
                dirty = true;
            }
        }

        for (int i = 0; i < spriteCount; i++)
        {
            if (ProcessSprite(spriteArr[i]) || dirty)
            {
                SetSpriteOrder(spriteArr[i]);
            }
        }
    }

    #endregion

}

