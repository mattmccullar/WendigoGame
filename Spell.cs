using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Spell", menuName = "Spell", order = 2)]
public class Spell : ScriptableObject
{
    public SpellType spellType;
    public SpellCategory spellCategory;

    public string spellName;
    public string spellAnimationName;

    public Sprite spellIconImage;

    public GameObject spellPrefab; // prefab containing the animator and particle system for the spell
    public ParticleSystem spellParticles;

    // store layer as string 
    public string layerName;

    // we reference an animator in spellcaster to call these
    public AnimationClip spellAnimation;
    public AnimationClip playerAnimation;

    // the age stages & related data
    public List<AgeStageData> ageStages;
    public int ageStage = 0;

    public GameObject spawnEffect; // The effect to play when the object is spawned.
    public Vector3 spawnPosition; // position to spawn spell was previously transform

    // configurable key code for input
    public KeyCode spellActivationKey = KeyCode.None;

    // nested class to hold data for each age stage
    [System.Serializable]
    public class AgeStageData
    {
        public string stageName; //young, mature, old
        public AnimationClip agingAnimation;
    }
}
public enum SpellType
{
    Creation,
    Destruction
}

public enum SpellCategory
{
    Tree,
    Bush,
    Rock
}