using DG.Tweening;
using UnityEngine;

public class ButtonSwaying : MonoBehaviour
{
    public bool Swaying = true;
    public float Speed = 1f;

    public Vector3 MaxScale = new Vector3(1.1f, 1.1f, 1.1f);
    public Vector3 MinScale = new Vector3(0.9f, 0.9f, 0.9f);
    public float MaxAngle = 1f;
    void OnEnable()
    {
        transform.localScale = MaxScale;
        if(Swaying)
            Sway();
    }

    void Sway()
    {
        if(Swaying)
        {
            MaxAngle = -MaxAngle;
            transform.DORotate(new Vector3(0, 0, MaxAngle), Speed);
            transform.DOScale(MaxScale, Speed)
                .OnComplete(() =>
                {
                    transform.DORotate(new Vector3(0, 0, 0), Speed);
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
        transform.rotation = Quaternion.identity;
    }
}
