using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class SortByZCam : MonoBehaviour
{
    #region Inspector items
    public int worldScale = 1000;
    public string LayerName = "Objects";

    #endregion

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

    //called by SpriteInfo in Awake to get a count;
    public static void CountSprite()
    {
        spriteCount++;
    }

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
        si.sortOrder = si.spriteRenderer.sortingOrder;
        //and stash the initial sort order as an offset
        si.initialOffset = si.sortOrder;

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

        if (si.sortOrder != z - si.initialOffset || si.rotationOffset != r)
        {
            si.dirty = true;
            si.sortOrder = z - si.initialOffset;
            si.rotationOffset = r;
            return true;
        }
        return false;
    }

    //apply the sprite ordering for a sprite with correction for camera-forwardness
    void SetSpriteOrder(SpriteInfo si)
    {
        si.spriteRenderer.sortingOrder = (facingForward) ? si.sortOrder * -1 : si.sortOrder + ((facingForward) ? si.rotationOffset * -1 : si.rotationOffset);
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
        StartCoroutine(WaitForSprites());
    }

    IEnumerator WaitForSprites()
    {
        yield return new WaitForEndOfFrame();
        if (sprites.Count == spriteCount)
        {
            spriteArr = sprites.ToArray();
            for (int i = 0; i < spriteCount; i++)
            {
                ProcessSprite(spriteArr[i]);
            }
            initialized = true;
        }
        else
        {
            Debug.LogError("sprites.Count (" + sprites.Count + ") != spriteCount (" + spriteCount + ")");
        }
    }

    private void Update()
    {
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

