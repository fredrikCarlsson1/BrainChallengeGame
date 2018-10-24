using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var parent = transform.parent;
 
         var parentRenderer = parent.GetComponent<MeshRenderer>();
         var renderern = GetComponent<Renderer>();
         renderern.sortingLayerID = parentRenderer.sortingLayerID;
         renderern.sortingOrder = parentRenderer.sortingOrder;
 
         var spriteTransform = parent.transform;
         var text = GetComponent<TextMesh>();
         var pos = spriteTransform.position;
         text.text = string.Format("{0}, {1}", pos.x, pos.y);
     }
	
	
	// Update is called once per frame
	void Update () {
		
	}
}
