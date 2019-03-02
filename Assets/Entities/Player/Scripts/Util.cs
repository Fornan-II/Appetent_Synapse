using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static Vector2 VectorInCircleSpace(Vector2 inputVector)
    {
        Vector2 maxedVector = Vector2.one;

        //Find maximum value 
        if (Mathf.Abs(inputVector.y) > Mathf.Abs(inputVector.x))
        {
            maxedVector.Set(1.0f, inputVector.x / inputVector.y);
            if (inputVector.y < 0.0f)
            {
                maxedVector.x = -1.0f;
            }
        }
        else if (Mathf.Abs(inputVector.y) < Mathf.Abs(inputVector.x))
        {
            maxedVector.Set(inputVector.y / inputVector.x, 1.0f);
            if (inputVector.x < 0.0f)
            {
                maxedVector.y = -1.0f;
            }
        }

        inputVector /= maxedVector.magnitude;

        return inputVector;
    }

    public static Vector2 VectorInCircleSpace(float x, float y)
    {
        return VectorInCircleSpace(new Vector2(x, y));
    }
}
