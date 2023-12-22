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
            var vcam = FindObjectOfType<CinemachineVirtualCamera>();
            vcam.transform.localPosition = RoundVec(vcam.transform.localPosition * 1000) / 1000;
            Vector2 mainPos = vcam.transform.localPosition;
            
            float quadOldZ = quad.localPosition.z;
            float camOldZ = vcam.transform.localPosition.z;
            mainPos += Vector2.Scale(mainPos, parallaxScale-Vector2.one);
            
            Vector2 roundedPos = RoundVec(mainPos);
            print("camPos " + camera.transform.position);
            print("mainPos " + mainPos);
            print("roundedPos " + roundedPos);
            Vector2 newPos = roundedPos - mainPos;
            newPos = RoundVec(newPos * 1000) / 1000;
            print("offset " + newPos);

            // vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = newPos;
            Vector3 ret = roundedPos;
            ret.z = camOldZ;
            vcam.transform.position = ret;
            
            Vector3 quadPos = newPos;
            quadPos.z = quadOldZ;
            quad.localPosition = quadPos;
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