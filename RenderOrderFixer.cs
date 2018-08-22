using UnityEngine;


public class RenderOrderFixer : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Camera _cam;

    public bool plainSort = true;

    int _sortOrder;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        SortByZCam.OnCamChanged += CamChanged;
    }

    private void CamChanged()
    {
        if (plainSort) SortByZ();
        else ReverseSpriteOrder();
    }

    //only works for sprites at z = 0
    private void ReverseSpriteOrder()
    {
        _spriteRenderer.sortingOrder = -_spriteRenderer.sortingOrder;
    }

    private void SortByZ()
    {
        if (transform.position.z == 0) return;

        int newOrder = (int)(transform.position.z * SortByZCam.worldScale);
        if (SortByZCam.facingForward) newOrder = -newOrder;

        if (newOrder == _sortOrder) return;
        else _sortOrder = newOrder;

        _spriteRenderer.sortingOrder = _sortOrder;

    }

}

