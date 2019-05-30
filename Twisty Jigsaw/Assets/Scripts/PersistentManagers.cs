using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// I'm not using this one yet but it might be good for seamless transitions for music etc.


public class PersistentManagers : MonoBehaviour
{
    public static PersistentManagers instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
