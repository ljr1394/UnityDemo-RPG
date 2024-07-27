using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeAnimotorTriggers : MonoBehaviour
{
    private ThunderStrike_Controller tc;

    private void Start()
    {
        tc = GetComponentInParent<ThunderStrike_Controller>();
    }
    public void StrikeTakeDamege()
    {
        tc.StrikeTakeDamege();
    }


    public void AnimotionFinish()
    {
        tc.SelfDestroy();
    }
}
