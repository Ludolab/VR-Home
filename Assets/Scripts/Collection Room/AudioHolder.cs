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
        base.Apply(obj);
        Record rec = obj.GetComponent<Record>();
        if (rec != null)
        {
            audioSrc.clip = rec.GetAudio();
        }
    }

    protected override void Remove()
    {
        base.Remove();

        audioSrc.Stop();
        audioSrc.clip = null;
    }
}
