using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHolder : Holder
{

    private AudioSource audioSrc;

    protected override void Start()
    {
        base.Start();
        audioSrc = GetComponent<AudioSource>();
    }

    public override void Apply(GameObject obj)
    {
        Record rec = obj.GetComponent<Record>();
        if (rec != null)
        {
            base.Apply(obj);
            audioSrc.clip = rec.GetAudio();
            audioSrc.Play();
        }
    }

    protected override void Remove()
    {
        base.Remove();

        audioSrc.Stop();
        audioSrc.clip = null;
    }
}
