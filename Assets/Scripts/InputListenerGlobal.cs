using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VirtualKey_PhysicalKey
{
    public string virtualkey;
    public KeyCode physicalkey;
}


[System.Serializable]
public class PlayerKeys
{
    public VirtualKey_PhysicalKey[] keys;
}

public class InputListenerGlobal : MonoBehaviour
{
    public PlayerKeys[] input_array;
    void Start()
    {
        KeyListen();
    }

    // Update is called once per frame
    void Update()
    {
        KeyListen();
    }

    public void KeyListen()
    {
        for (int i = 0; i < input_array.Length; i++)
        {
            for (int j = 0; j < input_array[i].keys.Length; j++)
            {
                if (Input.GetKey(input_array[i].keys[j].physicalkey))
                {
                    EasyInput.Player(i).SetInput(input_array[i].keys[j].virtualkey, Time.unscaledDeltaTime, Time.frameCount, 1f);
                }
            }
        }
    }
}
