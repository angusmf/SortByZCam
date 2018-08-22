using UnityEngine;


public class RenderOrderFixer : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;

    public bool plainSort = true;

    int _sortOrder;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _sortOrder = _spriteRenderer.sortingOrder;

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
        _spriteRenderer.sortingOrder = (SortByZCam.facingForward) ? _sortOrder : -_sortOrder;
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

