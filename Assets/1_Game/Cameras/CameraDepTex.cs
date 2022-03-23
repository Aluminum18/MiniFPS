using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDepTex : MonoBehaviour
{
    [SerializeField]
    private Camera _targetCamera;

    public void GenerateDepTex()
    {
        _targetCamera.depthTextureMode = DepthTextureMode.Depth;
    }
}
