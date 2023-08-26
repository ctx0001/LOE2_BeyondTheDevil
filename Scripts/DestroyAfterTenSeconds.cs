using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class DestroyAfterTenSeconds : MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject, 10f);
        }
    }
}