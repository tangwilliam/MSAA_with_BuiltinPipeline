using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TA
{
    public class UICameraSetup : MonoBehaviour
    {
        private void OnEnable()
        {
            Camera camera = GetComponent<Camera>();
            if (camera == null) return;

            CameraSetup.LoadStoreActionSetup(camera, Color.black);
        }

    }
}


