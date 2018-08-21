using UnityEngine;



public class SortByZCam : MonoBehaviour
{
    public static Camera mainCam;
    public static int worldScale = 1000;


   public delegate void FaceForward();
    public delegate void FaceBackward();

    public static FaceForward OnFaceForward;

    public static FaceBackward OnFaceBackward;

    bool facingForward;

    private void Start()
    {
        if (mainCam == null)
            mainCam = GetComponent<Camera>();
        else
            Destroy(this);
    }

    private void Update()
    {

        float angle = Vector3.Angle(transform.forward, Vector3.forward);
        if (facingForward)
            if (angle > 90)
            {
                facingForward = false;
                OnFaceBackward();
            }
            else return;
        else
            if (angle < 90)
        {
            facingForward = true;
            OnFaceForward();
        }
    }
}

