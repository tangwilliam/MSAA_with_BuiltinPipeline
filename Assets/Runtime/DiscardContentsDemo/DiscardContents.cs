using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Will
{
    public class DiscardContents : MonoBehaviour
    {
        [Range(0.01f, 1f)]
        public float RtScale = 1.0f;

        public GameObject TargetImageUI;

        public enum MSAAEnum
        {
            _X1 = 1,
            _X2 = 2,
            _X4 = 4,
            _X8 = 8
        }
        public MSAAEnum MSAA = MSAAEnum._X4;

        public bool DiscardColorBuffer = false;

        private Camera _camera;
        private RenderTexture _targetTexture;
        private int _width, _height;

        public void UpdateRT()
        {
            if (_camera == null) return;

            ReleaseRT();

            _width = Mathf.RoundToInt(_camera.pixelWidth * RtScale); // 注意：Screen.width会跟着RT的分辨率而改变，所以不能使用它
            _height = Mathf.RoundToInt(_camera.pixelHeight * RtScale);

            _targetTexture = RenderTexture.GetTemporary(_width, _height, 24,
                                                        RenderTextureFormat.Default, RenderTextureReadWrite.Default,
                                                        (int)MSAA, RenderTextureMemoryless.MSAA);
            _targetTexture.name = "SceneTargetTexture";
            _targetTexture.DiscardContents(DiscardColorBuffer, true);
            _camera.forceIntoRenderTexture = true;
            _camera.targetTexture = _targetTexture;

            RawImage targetImage = TargetImageUI.GetComponent<RawImage>();
            targetImage.texture = _targetTexture;
        }

        private void ReleaseRT()
        {
            if (_camera == null) return;

            if (_targetTexture != null)
            {
                _camera.targetTexture = null;
                RenderTexture.ReleaseTemporary(_targetTexture);
            }
        }


        private void OnEnable()
        {
            _camera = GetComponent<Camera>();

            UpdateRT();
        }

        private void OnDisable()
        {
            ReleaseRT();
        }

        private void OnValidate()
        {
            UpdateRT();
        }

        private void OnPreRender()
        {
            if (_camera == null) return;

            RenderTexture renderTexture = _camera.targetTexture;
            if (renderTexture != null)
            {
                renderTexture.DiscardContents(DiscardColorBuffer, true);
            }
        }

    }
}


