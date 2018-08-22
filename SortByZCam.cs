using UnityEngine;


public class SortByZCam : MonoBehaviour
{
    public static int worldScale = 1000;

    public delegate void CamChanged();

    public static CamChanged OnCamChanged;

    public static bool facingForward;


    private void Start()
    {

        //get first cam state
        facingForward = CheckForwardness();

        //Always send the first cam state
        OnCamChanged();
    }

    private void Update()
    {
        //only send cam state if changed
        if (facingForward != CheckForwardness())
        {
            facingForward = !facingForward;
            OnCamChanged();
        }
    }

    private bool CheckForwardness()
    {
        if (Vector3.Angle(transform.forward, Vector3.forward) < 90) return true;
        return false;
    }
}

