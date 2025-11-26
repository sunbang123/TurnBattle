using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 자동으로 AudioSource 부착
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    // 싱글톤으로 만들어보자
    public static SoundManager instance;

    AudioSource myAudio;

    // 오디오 클립 배열
    public AudioClip[] AttackSound;

    internal void PlayAttackSound(int v)
    {
        throw new NotImplementedException();
    }
}
