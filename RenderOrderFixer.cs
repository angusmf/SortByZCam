using UnityEngine;


public class RenderOrderFixer : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;

    public bool plainSort = true;
    public bool movesInZ = false;

    int _sortOrder;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _sortOrder = _spriteRenderer.sortingOrder;

        SortByZCam.OnCamChanged += CamChanged;
    }

    private void Update()
    {
        if (transform.position.z == 0 || !movesInZ) return;
        int newOrder = GetNewOrder();
        if (_sortOrder != newOrder) SortByZ(newOrder);

    }

    private int GetNewOrder()
    {
        int newOrder = (int)(transform.position.z * SortByZCam.worldScale);
        return (SortByZCam.facingForward) ? -newOrder : newOrder;
    }

    private void CamChanged()
    {
        if (plainSort) SortByZ(GetNewOrder());
        else ReverseSpriteOrder();
    }

    //only works for sprites at z = 0
    private void ReverseSpriteOrder()
    {
        _spriteRenderer.sortingOrder = (SortByZCam.facingForward) ? _sortOrder : -_sortOrder;
    }

    private void SortByZ(int newOrder)
    {
        if (transform.position.z == 0) return;

        if (newOrder == _sortOrder) return;
        else _sortOrder = newOrder;

        _spriteRenderer.sortingOrder = _sortOrder;

    }

}

