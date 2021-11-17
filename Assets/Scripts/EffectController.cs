using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

[Serializable]
public struct Effects
{
    public GameObject efx;
    public string name;
}

public class EffectController : MonoBehaviour
{
    public static EffectController instance;

    [SerializeField]
    public List<Effects> effects;

    void Awake()
    {
        instance = this;
    }

    public void SetPositionAndPlay(string effectName, Vector3 pos, bool changePos)
    {
        foreach (Effects e in effects)
        {
            if (e.name == effectName)
            {
                if (changePos)
                    e.efx.transform.position = pos;

                e.efx.SetActive(true);
                e.efx.GetComponent<ParticleSystem>().Play();
            }
        }
    }

    public GameObject SpawnEffect(string effectName, Vector3 pos, Transform parent)
    {
        foreach (Effects e in effects)
        {
            if (e.name == effectName)
            {
                GameObject effect = LeanPool.Spawn(e.efx, pos, e.efx.transform.rotation, parent);
                effect.SetActive(true);
                effect.GetComponent<ParticleSystem>().Play();
                LeanPool.Despawn(effect, 5);
                return effect;
            }
        }
        return null;
    }
}