using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A test object that will set a track to play when the player 
/// collides with it, then it destroys itself
/// </summary>
public class MusicBox : MonoBehaviour
{
    public Track track;
    public bool playImmediately;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "BikeMesh" || collision.collider.name.StartsWith("EM"))
        {
            Jukebox musicEngine = (Jukebox)FindObjectOfType(typeof(Jukebox));
            if(musicEngine != null)
            {
                musicEngine.SetNextTrack(track, playImmediately);
                Destroy(this.gameObject);
            }
        }
    }
}
