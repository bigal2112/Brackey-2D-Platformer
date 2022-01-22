using UnityEngine;

[System.Serializable]
public class Sound
{

  public string name;
  public AudioClip clip;

  [Range(0f, 1f)]
  public float volume = 0.7f;
  [Range(0.5f, 1.5f)]
  public float pitch = 1f;

  [Range(0f, 0.5f)]
  public float randomVolume = 0.1f;
  [Range(0f, 0.5f)]
  public float randomPitch = 0.1f;

  public bool loop = false;

  private AudioSource source;

  public void SetSource(AudioSource _source)
  {
    source = _source;
    source.clip = clip;
    source.loop = loop;
  }

  public void Play()
  {
    //    play the clip with a random volume and pitch
    source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
    source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f)); ;
    source.Play();
  }

  public void Stop()
  {
    source.Stop();
  }
}

public class AudioManager : MonoBehaviour
{
  [SerializeField]
  Sound[] sounds;

  public static AudioManager audioManagerInstance;

  private void Awake()
  {
    //  all this ensure the same AudioManager instance is present for all the components
    //  this will mean any clips play will not end when the scene is changed.

    //  if there is an instance of the audio manager
    if (audioManagerInstance != null)
    {
      //  and the instance is not this instance (the first instance) then destory it
      if (audioManagerInstance != this)
        Destroy(this.gameObject);
    }
    else
    //  if there is no instance then create one and set to the not be destroyed between scenes
    {
      audioManagerInstance = this;
      DontDestroyOnLoad(this);
    }
  }

  private void Start()
  {
    for (int i = 0; i < sounds.Length; i++)
    {
      //  create a new game object and add an Audio Source component to it.
      GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
      _go.transform.SetParent(this.transform);
      sounds[i].SetSource(_go.AddComponent<AudioSource>());
    }

    PlaySound("Music");
  }

  public void PlaySound(string _name)
  {
    for (int i = 0; i < sounds.Length; i++)
    {
      if (sounds[i].name == _name)
      {
        sounds[i].Play();
        return;
      }
    }

    //  no sound with _name found
    Debug.LogWarning("No sound with the name " + _name + " found");
  }

  public void StopSound(string _name)
  {
    for (int i = 0; i < sounds.Length; i++)
    {
      if (sounds[i].name == _name)
      {
        sounds[i].Stop();
        return;
      }
    }
  }

}
