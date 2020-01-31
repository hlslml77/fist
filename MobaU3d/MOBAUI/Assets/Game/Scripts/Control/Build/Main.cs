using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : BaseControl
{
    public override void DeathResponse()
    {
        GetComponent<Animation>().CrossFade("death");
    }
}
