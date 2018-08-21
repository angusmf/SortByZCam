using UnityEngine;


public class RenderOrderFixer : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Camera _cam;

    public bool ignorePresetOrder = true;
    public bool startFacingForward = true;

    bool camFacingForward;
    int _sortOrder;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _cam = SortByZCam.mainCam;

        SortByZCam.OnFaceForward += FaceForward;
        SortByZCam.OnFaceBackward += FaceBackward;

        if (ignorePresetOrder)
            if (startFacingForward) SortByZ(true);
            else SortByZ(false);

    }

    private void Update()
    {
        SortByZ(camFacingForward);
    }

    private void FaceForward()
    {
        camFacingForward = true;
    }

    private void FaceBackward()
    {
        camFacingForward = false;
    }

    private void SortByZ(bool faceForward)
    {   
        int newOrder = (int)(transform.position.z * SortByZCam.worldScale);

        if (newOrder == _sortOrder) return;
        else _sortOrder = newOrder;

        if (faceForward) _spriteRenderer.sortingOrder = -_sortOrder;
        else _spriteRenderer.sortingOrder = _sortOrder;

    }

}

