using UnityEngine;
using System.Collections;

public class TiledMesh : MonoBehaviour {
    void Start(){
        Vector2 texScale = transform.localScale;
        if ( texScale.x > 1f )
            texScale.x /= 2.0f;
        if ( texScale.y > 1f )
            texScale.y /= 2.0f;
        GetComponent<Renderer>().material.mainTextureScale = texScale;
    }
}
