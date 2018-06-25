using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHolder : Holder
{
    public GameObject recordPlayer;
    public GameObject teller;
    public GameObject colorObj;

    private AudioSource audioSrc;
    private RecordPlayer rp;
    private Renderer colorRend;

    protected override void Start()
    {
        base.Start();
        audioSrc = GetComponent<AudioSource>();
        rp = recordPlayer.GetComponent<RecordPlayer>();
        colorRend = colorObj.GetComponent<Renderer>();
    }

    public override void Apply(GameObject obj)
    {
        Record rec = obj.GetComponent<Record>();
        if (rec != null)
        {
            base.Apply(obj);
            audioSrc.clip = rec.GetAudio();
            audioSrc.Play();
            teller.SetActive(true);
            colorRend.material.color = rec.GetColor();
            rp.recordPlayerActive = true;
        }
    }

    protected override void Remove()
    {
        base.Remove();

        audioSrc.Stop();
        audioSrc.clip = null;
        rp.recordPlayerActive = false;
        teller.SetActive(false);
    }
}
