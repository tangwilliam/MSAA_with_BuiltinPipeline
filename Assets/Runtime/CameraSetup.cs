using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rendering = UnityEngine.Rendering;
using LoadAction = UnityEngine.Rendering.RenderBufferLoadAction;
using StoreAction = UnityEngine.Rendering.RenderBufferStoreAction;

namespace Will
{
    [DisallowMultipleComponent]
    public class CameraSetup : MonoBehaviour
    {
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

            //cam.clearFlags = CameraClearFlags.Nothing; // 该语句可以删除。虽然Unity论坛中那篇帖子提到需要，但XCode中测试下来删除了也没区别。

            cam.RemoveCommandBuffers(Rendering.CameraEvent.BeforeForwardOpaque);

            Rendering.CommandBuffer cmd = new Rendering.CommandBuffer();
            cmd.SetRenderTarget(Rendering.BuiltinRenderTextureType.CameraTarget,
                LoadAction.DontCare, StoreAction.Store, LoadAction.DontCare, StoreAction.DontCare
            ); // 注意：最后一环的UI相机也不能设置ColorBuffer的StoreAction为DontCare，因为仍需要Store它到SystemMemory作为双缓冲的Backbuffer

            // 也可以在编辑器里设置 ClearFlag = SolidColor，用XCode观察 Dependency View 和带宽与本句开销相同
            // 如果需要使用Unity提供的Skybox机制，那么就必然不能加本句代码了。
            // 用RenderDoc查看，使用ClearFlag = Skybox 时，没有此句代码，Unity仍会在Renderpass开始时调用glClear()
            //cmd.ClearRenderTarget(true, true, Color.clear, 1.0f);

            cmd.name = "Setup LoadAction and StoreAction";
            cam.AddCommandBuffer(Rendering.CameraEvent.BeforeForwardOpaque, cmd);

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
            cmd.SetGlobalTexture(ShaderUtils.SceneFinalRT, cam.targetTexture); // 这是个唯一的标识，方便在后效环节中使用
            cmd.name = "Setup SceneFinalRT";
            cam.AddCommandBuffer(Rendering.CameraEvent.BeforeImageEffects, cmd);
        }

        private void OnEnable()
        {
            _camera = GetComponent<Camera>();

            UpdateRT();

            LoadStoreActionSetup(_camera, Color.clear);

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
                                                        (int)MSAA, RenderTextureMemoryless.MSAA); // 为简化，没考虑HDR的情况
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
