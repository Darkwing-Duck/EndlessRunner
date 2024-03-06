using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utils
{

	public static class AppDomainExtensions
	{
		public static IEnumerable<Assembly> GetUserDefinedAssemblies(this AppDomain appDomain)
        {
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if(assembly.IsDynamic) {
                    continue;
                }
             
                var assemblyName = assembly.GetName().Name;
                
                if(internalAssemblyNames.Contains(assemblyName)) {
                    continue;
                }
             
                yield return assembly;
            }
        }
 
        private static readonly HashSet<string> internalAssemblyNames = new()
        {
            "mscorlib",
            "System",
            "System.Core",
            "System.Security.Cryptography.Algorithms",
            "System.Net.Http",
            "System.Data",
            "System.Runtime.Serialization",
            "System.Xml.Linq",
            "System.Numerics",
            "System.Xml",
            "System.Configuration",
            "ExCSS.Unity",
            "Unity.Cecil",
            "Unity.VisualScripting",
            "Unity.CompilationPipeline.Common",
            "Unity.SerializationLogic",
            "Unity.TestTools.CodeCoverage.Editor",
            "Unity.ScriptableBuildPipeline.Editor",
            "Unity.Addressables.Editor",
            "Unity.ScriptableBuildPipeline",
            "Unity.CollabProxy.Editor",
            "Unity.Timeline.Editor",
            "Unity.PerformanceTesting.Tests.Runtime",
            "Unity.Settings.Editor",
            "Unity.PerformanceTesting",
            "Unity.PerformanceTesting.Editor",
            "Unity.Rider.Editor",
            "Unity.ResourceManager",
            "Unity.TestTools.CodeCoverage.Editor.OpenCover.Mono.Reflection",
            "Unity.PerformanceTesting.Tests.Editor",
            "Unity.TextMeshPro",
            "Unity.Timeline",
            "Unity.Addressables",
            "Unity.TestTools.CodeCoverage.Editor.OpenCover.Model",
            "Unity.VisualStudio.Editor",
            "Unity.TextMeshPro.Editor",
            "Unity.VSCode.Editor",
            "UnityEditor",
            "UnityEditor.UI",
            "UnityEditor.TestRunner",
            "UnityEditor.CacheServer",
            "UnityEditor.WindowsStandalone.Extensions",
            "UnityEditor.Graphs",
            "UnityEditor.UnityConnectModule",
            "UnityEditor.UIServiceModule",
            "UnityEditor.UIElementsSamplesModule",
            "UnityEditor.UIElementsModule",
            "UnityEditor.SceneTemplateModule",
            "UnityEditor.PackageManagerUIModule",
            "UnityEditor.GraphViewModule",
            "UnityEditor.CoreModule",
            "UnityEngine",
            "UnityEngine.UI",
            "UnityEngine.XRModule",
            "UnityEngine.WindModule",
            "UnityEngine.VirtualTexturingModule",
            "UnityEngine.TestRunner",
            "UnityEngine.VideoModule",
            "UnityEngine.VehiclesModule",
            "UnityEngine.VRModule",
            "UnityEngine.VFXModule",
            "UnityEngine.UnityWebRequestWWWModule",
            "UnityEngine.UnityWebRequestTextureModule",
            "UnityEngine.UnityWebRequestAudioModule",
            "UnityEngine.UnityWebRequestAssetBundleModule",
            "UnityEngine.UnityWebRequestModule",
            "UnityEngine.UnityTestProtocolModule",
            "UnityEngine.UnityCurlModule",
            "UnityEngine.UnityConnectModule",
            "UnityEngine.UnityAnalyticsModule",
            "UnityEngine.UmbraModule",
            "UnityEngine.UNETModule",
            "UnityEngine.UIElementsNativeModule",
            "UnityEngine.UIElementsModule",
            "UnityEngine.UIModule",
            "UnityEngine.TilemapModule",
            "UnityEngine.TextRenderingModule",
            "UnityEngine.TextCoreModule",
            "UnityEngine.TerrainPhysicsModule",
            "UnityEngine.TerrainModule",
            "UnityEngine.TLSModule",
            "UnityEngine.SubsystemsModule",
            "UnityEngine.SubstanceModule",
            "UnityEngine.StreamingModule",
            "UnityEngine.SpriteShapeModule",
            "UnityEngine.SpriteMaskModule",
            "UnityEngine.SharedInternalsModule",
            "UnityEngine.ScreenCaptureModule",
            "UnityEngine.RuntimeInitializeOnLoadManagerInitializerModule",
            "UnityEngine.ProfilerModule",
            "UnityEngine.Physics2DModule",
            "UnityEngine.PhysicsModule",
            "UnityEngine.PerformanceReportingModule",
            "UnityEngine.ParticleSystemModule",
            "UnityEngine.LocalizationModule",
            "UnityEngine.JSONSerializeModule",
            "UnityEngine.InputLegacyModule",
            "UnityEngine.InputModule",
            "UnityEngine.ImageConversionModule",
            "UnityEngine.IMGUIModule",
            "UnityEngine.HotReloadModule",
            "UnityEngine.GridModule",
            "UnityEngine.GameCenterModule",
            "UnityEngine.GIModule",
            "UnityEngine.DirectorModule",
            "UnityEngine.DSPGraphModule",
            "UnityEngine.CrashReportingModule",
            "UnityEngine.CoreModule",
            "UnityEngine.ClusterRendererModule",
            "UnityEngine.ClusterInputModule",
            "UnityEngine.ClothModule",
            "UnityEngine.AudioModule",
            "UnityEngine.AssetBundleModule",
            "UnityEngine.AnimationModule",
            "UnityEngine.AndroidJNIModule",
            "UnityEngine.AccessibilityModule",
            "UnityEngine.ARModule",
            "UnityEngine.AIModule",
            "SyntaxTree.VisualStudio.Unity.Bridge",
            "nunit.framework",
            "Newtonsoft.Json",
            "ReportGeneratorMerged",
            "Unrelated",
            "netstandard",
            "SyntaxTree.VisualStudio.Unity.Messaging",
            "UnityEngine.ContentLoadModule",
            "UnityEngine.NVIDIAModule",
            "UnityEngine.PropertiesModule",
            "UnityEngine.TextCoreFontEngineModule",
            "UnityEngine.TextCoreTextEngineModule",
            "UnityEngine.UnityAnalyticsCommonModule",
            "UnityEditor.DeviceSimulatorModule",
        };
	}

}