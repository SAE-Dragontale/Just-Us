
/* --------------------------------------------------------------------------------------------------------------------------------------------------------- //
	Author: 		Hayden Reeve
	File:			WireframeDraw.cs
	Description: 	This script draws wireframes in the unity editor.
// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WireframeDraw : MonoBehaviour {

	// --- GIZMOS SETTINGS

	[Header("Gizmos Settings")]
	[SerializeField] private Color _clDrawColour = new Color (1f, 1f, 1f, 1f);

	[Header("Collider Settings")]
	[SerializeField] private bool _isDrawingBoxCollider = false;
	[SerializeField] private bool _isDrawingSphereCollider = false;

	[Header("Cube Settings")]
	[SerializeField] private bool _isDrawingCube = false;
	[SerializeField] private Vector3 _v3CubeScale = new Vector3(20,20,20);

	[Header("Sphere Settings")]
	[SerializeField] private bool _isDrawingSphere = false;
	[SerializeField] private float _flSphereRadius = 20f;


	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

	void OnDrawGizmos () {

		// --- GIZMOS SETTINGS

		Gizmos.color = _clDrawColour;

		// --- DRAW DEFINED GIZMOS

		if (_isDrawingCube)
			Gizmos.DrawWireCube (transform.position, _v3CubeScale);

		if (_isDrawingSphere)
			Gizmos.DrawWireSphere (transform.position, _flSphereRadius);

		// --- DRAW COMPONENT GIZMOS

		if (_isDrawingBoxCollider && gameObject.GetComponent<BoxCollider> () != null)
			Gizmos.DrawWireCube (transform.position, gameObject.GetComponent<BoxCollider> ().bounds.size);

		if (_isDrawingSphereCollider && gameObject.GetComponent<SphereCollider> () != null) {
			Gizmos.DrawWireSphere (transform.position, gameObject.GetComponent<SphereCollider> ().radius * Mathf.Max (Mathf.Max (transform.lossyScale.x, transform.lossyScale.y), transform.lossyScale.z));
		}

	}

}

#endif