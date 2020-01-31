using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camp : BaseControl
{
    public override void DeathResponse()
    {
        gameObject.SetActive(false);
    }

    public override void ResurgeResponse()
    {
        this.gameObject.SetActive(true);
    }
}
