using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour {
    private static GameObject me;
    [SerializeField] private AudioClip[] _soundEffects;
    private static AudioClip[] soundEffects;

    //This corresponds to the indexes of 'soundEffects' array
    public enum Clips {
        LaserBeam, ShootDisc, Demon1, Demon2
    }

    private void Start() {
        me = gameObject;
        soundEffects = _soundEffects;
    }

    public static float Play(int index){
        if(index < soundEffects.Length){
            AudioSource src = me.AddComponent<AudioSource>();

            src.clip = soundEffects[index];
            src.loop = false;

            GameController.gCont.CoroutinePasser(DestroyClipWhenDone(src));
            return src.clip.length;
        }else{
            Debug.LogError("Sound Effect #" + index + " not found!");
            return -1;
        }
    }
    public static float Play(Clips clip){
        return Play((int)clip);
    }

    public static IEnumerator DestroyClipWhenDone(AudioSource source){
        yield return new WaitForSeconds(source.clip.length);
        Destroy(source);
    }
}
