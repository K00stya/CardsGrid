using DG.Tweening;
using UnityEngine;

public class ButtonSwaying : MonoBehaviour
{
    public bool Swaying = true;
    public float Speed = 1f;

    public Vector3 MaxScale = new Vector3(1.1f, 1.1f, 1.1f);
    public Vector3 MinScale = new Vector3(0.9f, 0.9f, 0.9f);
    public float MaxAngle = 1f;
    private Vector3 rot;
    void Start()
    {
        rot = transform.localRotation.eulerAngles;
        transform.localScale = MaxScale;
        if(Swaying)
            Sway();
    }

    void Sway()
    {
        if(Swaying)
        {
            MaxAngle = -MaxAngle;
            transform.DOLocalRotate(new Vector3(rot.x, rot.y, MaxAngle), Speed);
            transform.DOScale(MaxScale, Speed)
                .OnComplete(() =>
                {
                    transform.DOLocalRotate(new Vector3(rot.x, rot.y, 0), Speed);
                    transform.DOScale(MinScale, Speed)
                        .OnComplete(Sway);
                });
        }
    }

    public void GoSway()
    {
        transform.DOKill();
        Sway();
    }

    public void StopSway()
    {
        Swaying = false;
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.Euler(new Vector3(rot.x, rot.y, 0));
    }
}
