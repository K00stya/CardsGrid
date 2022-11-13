using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GameSize : MonoBehaviour
{
    public float rel = 10;
    private float oldfRel;

    void Update()
    {
        if (Screen.height >= Screen.width)
        {
            float rel2 = ((float) Screen.height / Screen.width);
            if (rel2 > 1.77f && oldfRel != rel2)
            {
                oldfRel = rel2;
                float a = ((rel2 - rel) / rel) * 100f;
                float b = 1f - (1f / 100f) * a;
                transform.localScale = new Vector3(b, b, b);
            }

        }
    }
}
