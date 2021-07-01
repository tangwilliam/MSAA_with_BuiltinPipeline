using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TA
{

    static public class ShaderUtils
    {
        //-----------------------------------------------------------
        // Property ID 

        // Post Processing
        public static readonly int MainTex = Shader.PropertyToID("_MainTex");
        public static readonly int MainTex_ST = Shader.PropertyToID("_MainTex_ST");
        public static readonly int BlurTex = Shader.PropertyToID("_BlurTex");
        public static readonly int SceneFinalRT = Shader.PropertyToID("_PostRT");

        // Grab texture
        public static readonly int ScreenCopyTexID = Shader.PropertyToID("_ScreenCopyTex");

    }

}
