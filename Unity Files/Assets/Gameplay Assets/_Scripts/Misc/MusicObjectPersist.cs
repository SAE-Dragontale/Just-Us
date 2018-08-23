using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicObjectPersist : MonoBehaviour {

	// Retain personalised instance, so we don't create more than one of this object.
	private static MusicObjectPersist _scPersistantMusic;

	// Use this for initialization
	void Start () {

		// Make sure we've only got one of this little bugger running.
		if(!_scPersistantMusic)
			_scPersistantMusic = this;
		else
			Destroy (this.gameObject);

		// Make sure this isn't destroyed. I quite like it.
		DontDestroyOnLoad (gameObject);

	}

}