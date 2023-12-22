using UnityEngine;
using UnityEngine.Rendering;
// using Mathf;
using UnityEngine.Experimental.Rendering;
using System.Reflection;
using System.Linq;
using Cinemachine;
using UnityEngine.Rendering.Universal;

public class URPCallbackExample : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform cams;
    [SerializeField] private Transform quad;
    // [SerializeField] private Transform layer;

    [SerializeField] private Vector2 parallaxScale;

    [SerializeField] private bool pixelate = true;
    
    // Unity calls this method automatically when it enables this component
    private void OnEnable()
    {
        // Add WriteLogMessage as a delegate of the RenderPipelineManager.beginCameraRendering event
        RenderPipelineManager.beginCameraRendering += WriteLogMessage;
    }

    // Unity calls this method automatically when it disables this component
    private void OnDisable()
    {
        // Remove WriteLogMessage as a delegate of the  RenderPipelineManager.beginCameraRendering event
        RenderPipelineManager.beginCameraRendering -= WriteLogMessage;
    }

    // When this method is a delegate of RenderPipeline.beginCameraRendering event, Unity calls this method every time it raises the beginCameraRendering event
    void WriteLogMessage(ScriptableRenderContext context, Camera camera)
    {
        if (camera == cam)
        {
            Vector2 mainPos = cams.transform.position;
            
            // if (!pixelate)
            // {
            //     layer.transform.position = new Vector3(500, 0, layer.transform.position.z) - (Vector3) Vector2.Scale(mainPos, parallaxScale);
            //     return;
            // }
            //
            // layer.transform.position = new Vector3(0, 0, layer.transform.position.z);
            
            float oldCamZ = camera.transform.localPosition.z;
            float quadOldZ = quad.localPosition.z;

            mainPos += Vector2.Scale(mainPos, parallaxScale-Vector2.one);
            
            Vector2 roundedPos = RoundVec(mainPos);
            Vector2 newPos = roundedPos - mainPos;

            mainPos += newPos;
            
            Vector3 ret = mainPos;
            ret.z = oldCamZ;
            camera.transform.position = ret;
            // FindObjectOfType<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>()
                // .m_FollowOffset = (Vector2)ret;
            
            Vector3 quadPos = newPos;
            quadPos.z = quadOldZ;
            quad.localPosition = quadPos;
            
            // float oldCamZ = camera.transform.localPosition.z;
            // float quadOldZ = quad.localPosition.z;
            //
            // Vector2 mainPos = cams.transform.position;
            //
            // mainPos += Vector2.Scale(mainPos, parallaxScale);
            // // layer.transform.position = mainPos;
            //
            // // mainPos = cams.transform.position;
            // Vector2 roundedPos = RoundVec(mainPos);
            // Vector2 newPos = roundedPos - mainPos;
            //
            // mainPos += newPos;
            //
            // // Vector3 ret = mainPos;
            // Vector3 ret = newPos;
            // ret.z = oldCamZ;
            // // camera.transform.position = ret;
            // camera.transform.localPosition = ret;
            //
            // Vector3 quadPos = newPos;
            // quadPos.z = quadOldZ;
            // quad.localPosition = quadPos;
        }
    }
    
    private void ExtractScriptableRendererData()
    {
        var pipeline = ((UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset);
        FieldInfo propertyInfo = pipeline.GetType(  ).GetField( "m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic );
        var _scriptableRendererData = ((ScriptableRendererData[]) propertyInfo?.GetValue( pipeline ))?[0];
        
        foreach ( var renderObjSetting in _scriptableRendererData.rendererFeatures.OfType<UnityEngine.Experimental.Rendering.Universal.RenderObjects>( ) )
        {
            // renderObjSetting.settings.cameraSettings.cameraFieldOfView    = _currentFPSFov;
            // renderObjSetting.settings.cameraSettings.offset                = _currentFPSOffset;

            renderObjSetting.settings.cameraSettings.offset = new Vector3(10, 10, 0);
        }
    }

    private float RoundFunc(float x) => Mathf.Round(x);
    
    private Vector3 RoundVec(Vector3 v)
    {
        return new Vector3(RoundFunc(v.x), RoundFunc(v.y), RoundFunc(v.z));
    }
}