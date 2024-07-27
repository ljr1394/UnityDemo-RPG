using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance { get; private set; }

    public Skill_Dash skill_Dash;

    public Skill_Clone skill_Clone;
    public Skill_Sword skill_Sword;

    public Skill_Blackhold skill_Blackhold;
    public Skill_Crystal skill_Crystal;

    void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    private void Start()
    {
        skill_Dash = GetComponent<Skill_Dash>();
        skill_Clone = GetComponent<Skill_Clone>();
        skill_Sword = GetComponent<Skill_Sword>();
        skill_Blackhold = GetComponent<Skill_Blackhold>();
        skill_Crystal = GetComponent<Skill_Crystal>();
    }



}

