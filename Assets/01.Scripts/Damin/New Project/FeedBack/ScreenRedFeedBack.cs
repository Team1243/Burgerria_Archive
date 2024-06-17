using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenRedFeedBack : FeedBack
{
    [SerializeField] private float m_speed = 6f;
    [SerializeField] private float m_maxValue = 0.5f;

    private Volume m_volume;
    private Vignette m_vignette;

    private bool m_enabled = false;

    private void Start()
    {
        m_volume = GetComponent<Volume>();
        m_volume.profile.TryGet<Vignette>(out m_vignette);
    }

    private void Update()
    {
        if(m_enabled)
            m_vignette.intensity.value = MathF.Sin(Time.time * m_speed) * m_maxValue;
    }

    public override void CreateFeedBack()
    {
        m_enabled = true;
    }

    public override void CompleteFeedBack()
    {
        m_enabled = false;
        m_vignette.intensity.value = 0;
    }

}
