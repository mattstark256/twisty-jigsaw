using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// On 05/06/2019 the framerate suddenly started to be really low, like 10fps or something. It seems to be an issue with the target framerate because setting it to 60 gets rid of the issue.
// https://answers.unity.com/questions/1523129/game-fps-suddenly-slow-on-android.html


public class FramerateFix : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
    }
}
