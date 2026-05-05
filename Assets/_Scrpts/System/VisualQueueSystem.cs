using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using NUnit.Framework;
using Unity.VisualScripting;

public class VisualQueueSystem : Singleton<VisualQueueSystem>
{
    private Queue<IEnumerator> visualQueue = new Queue<IEnumerator>();
    private bool isPlaying = false;

    public void EnqueueVisual(IEnumerator visualRountine)
    {
        visualQueue.Enqueue(visualRountine);
        if (!isPlaying)
        {
            StartCoroutine(ProcessQueue());
        }

    }
    public IEnumerator ProcessQueue()
    {
        isPlaying = true;
        while (visualQueue.Count > 0)
        {
            IEnumerator currentVisual = visualQueue.Dequeue();
            yield return StartCoroutine(currentVisual);
        }
        isPlaying = false;
    }
}