using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseListener : MonoBehaviour
{
    // Reference to the main script that might have OnNoiseReceived()
    public UnityEngine.Object mainScript;

    
    // Run the OnNoiseReceived method on mainScript, giving it the noisePosition argument
    public void ReceiveNoise(Vector3 noisePosition)
    {
        // Check if mainScript is assigned
        if (mainScript != null)
        {
            // Get the actual component instance
            Component target = mainScript as Component;
            
            // Use reflection to invoke the method if it exists
            if (target != null)
            {
                var methodInfo = target.GetType().GetMethod("OnNoiseReceived", new Type[] { typeof(Vector3) });
                

                if (methodInfo != null && methodInfo.IsPublic)
                {
                    try
                    {
                        methodInfo.Invoke(target, new object[] { noisePosition });
                    }
                    catch
                    {
                        Debug.LogError("<color='red'>Error!</color> mainScript doesn't have the OnNoiseReceived method!");
                    }
                }
            }
        }
        else
        {
            Debug.LogError("<color='red'>Error!</color> mainScript not assigned!");
        }
    }
}