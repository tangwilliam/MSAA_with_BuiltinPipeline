using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rendering = UnityEngine.Rendering;
using LoadAction = UnityEngine.Rendering.RenderBufferLoadAction;
using StoreAction = UnityEngine.Rendering.RenderBufferStoreAction;

namespace TA
{
    [DisallowMultipleComponent]
    public class CameraSetup : MonoBehaviour
    {
        public Color ClearColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        [Range(0.01f,1f)]
        public float RtScale = 1.0f;

        public enum MSAAEnum
        {
            _X1 = 1,
            _X2 = 2,
            _X4 = 4,
            _X8 = 8
        }
        public MSAAEnum MSAA = MSAAEnum._X4;

        private Camera _camera;
        private RenderTexture _targetTexture;
        private int _width, _height;

        /// <summary>
        /// 对相机进行初始化设置： Load和Store的模式
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="clearColor"></param>
        public static void LoadStoreActionSetup(Camera cam, Color clearColor)
        {
            if (cam == null) return;

            cam.clearFlags = CameraClearFlags.Nothing; // ClearRenderTarget 放到 BeforeForwardOpaque

            cam.RemoveCommandBuffers(Rendering.CameraEvent.BeforeForwardOpaque);

            Rendering.CommandBuffer cmd = new Rendering.CommandBuffer();
            cmd.SetRenderTarget(Rendering.BuiltinRenderTextureType.CameraTarget,
                LoadAction.DontCare, StoreAction.Store, LoadAction.DontCare, StoreAction.DontCare
            );
            cmd.ClearRenderTarget(true, true, clearColor, 1.0f);
            cmd.name = "LoadAction: Dont Care, StoreAction: Dont Care";
            cam.AddCommandBuffer(Rendering.CameraEvent.BeforeForwardOpaque, cmd);

        }

        public static void ResetLoadStoreActionSetup(Camera cam)
        {
            if (cam == null) return;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.RemoveCommandBuffers(Rendering.CameraEvent.BeforeForwardOpaque);
        }


        /// <summary>
        /// 让相机能够渲染到UI相机前面的显示场景画面的Image上
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="target2PostRT"></param>
        public static void SetTargetToPostRT(Camera cam)
        {
            cam.RemoveCommandBuffers(Rendering.CameraEvent.BeforeImageEffects);
            Rendering.CommandBuffer cmd = new Rendering.CommandBuffer();
            cmd.SetGlobalTexture(ShaderUtils.SceneFinalRT, cam.targetTexture);
            cmd.name = "Setup SceneFinalRT";
            cam.AddCommandBuffer(Rendering.CameraEvent.BeforeImageEffects, cmd);
        }

        private void OnEnable()
        {
            _camera = GetComponent<Camera>();

            UpdateRT();

            LoadStoreActionSetup(_camera, ClearColor);

        }

        private void OnDisable()
        {
            ReleaseRT();
        }


        private void OnValidate()
        {
            UpdateRT();
        }


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
            _camera.forceIntoRenderTexture = true;
            _camera.targetTexture = _targetTexture;
            SetTargetToPostRT(_camera);
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


    }

}
