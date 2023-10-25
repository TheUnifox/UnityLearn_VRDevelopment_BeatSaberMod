// Decompiled with JetBrains decompiler
// Type: BakedLightUtils
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public abstract class BakedLightUtils
{
  public const string kMirrorParentNameToIgnore = "PlayersPlace";
  private const string kDepthOnlyShaderName = "Custom/SetDepthOnly";
  [DoesNotRequireDomainReloadInit]
  private static readonly int _zWritePropertyId = Shader.PropertyToID("_ZWrite");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _stencilRefValuePropertyId = Shader.PropertyToID("_StencilRefValue");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _stencilCompPropertyId = Shader.PropertyToID("_StencilComp");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _stencilPassOpPropertyId = Shader.PropertyToID("_StencilPass");

  public static void ValidateLoadedEnvironmentScene(bool validateBakedGIEnabled)
  {
    GameObject gameObject = (GameObject) null;
    Scene scene = new Scene();
    for (int index = 0; index < SceneManager.sceneCount; ++index)
    {
      Scene sceneAt = SceneManager.GetSceneAt(index);
      if (sceneAt.name.Contains("Environment"))
      {
        gameObject = sceneAt.GetRootGameObjects()[0];
        scene = sceneAt;
        break;
      }
    }
    BakedReflectionProbe componentInChildren1 = gameObject.GetComponentInChildren<BakedReflectionProbe>(false);
    bool flag1 = (Object) gameObject.GetComponentInChildren<BakedLightWithIdBase>(false) != (Object) null;
    bool flag2 = (Object) componentInChildren1 != (Object) null;
    gameObject.GetComponentInChildren<BakedLightDataLoader>();
    LightmapLightsWithIds componentInChildren2 = gameObject.GetComponentInChildren<LightmapLightsWithIds>();
    LightmapLightWithIds[] componentsInChildren1 = gameObject.GetComponentsInChildren<LightmapLightWithIds>();
    gameObject.GetComponentsInChildren<BakeIdMapper>();
    if (flag1 | flag2)
    {
      int num1 = (Object) componentInChildren2 != (Object) null ? 1 : (componentsInChildren1.Length != 0 ? 1 : 0);
      int num2 = (Object) componentInChildren2 != (Object) null ? 1 : 0;
      int length = componentsInChildren1.Length;
      HashSet<LightConstants.BakeId> bakeIdSet = new HashSet<LightConstants.BakeId>();
      foreach (LightmapLightWithIds lightmapLightWithIds in componentsInChildren1)
      {
        if (!bakeIdSet.Contains(lightmapLightWithIds.bakeId))
          bakeIdSet.Add(lightmapLightWithIds.bakeId);
      }
    }
    if (flag1)
    {
      int num3 = validateBakedGIEnabled ? 1 : 0;
      foreach (BakedLightWithIdBase componentsInChild in gameObject.GetComponentsInChildren<BakedLightWithIdBase>(true))
      {
        int num4 = componentsInChild.gameObject.activeSelf ? 1 : 0;
        componentsInChild.SetupLightSource(0.0f);
        IBakedLightWithRenderer lightWithRenderer = componentsInChild as IBakedLightWithRenderer;
      }
    }
    else
      Debug.Log((object) ("\"" + scene.name + "\": No BakedLightWithIdBase found in the scene, skipping Baked Lights validation"));
    if (flag2)
    {
      FakeMirrorObjectsInstaller componentInChildren3 = gameObject.GetComponentInChildren<FakeMirrorObjectsInstaller>();
      gameObject.GetComponentInChildren<SceneDecoratorContext>();
      gameObject.GetComponentInChildren<FakeMirrorSettings>();
      foreach (Mirror componentsInChild in gameObject.GetComponentsInChildren<Mirror>())
      {
        if (!(componentsInChild.transform.parent.name == "PlayersPlace"))
        {
          Material noMirrorMaterial = componentsInChild.GetComponent<Mirror>().noMirrorMaterial;
          Renderer[] componentsInChildren2 = componentsInChild.GetComponentsInChildren<Renderer>();
          Material sharedMaterial1 = componentInChildren3.mirroredGameNoteControllerPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterial;
          double num5 = (double) noMirrorMaterial.GetFloat(BakedLightUtils._stencilRefValuePropertyId);
          double num6 = (double) noMirrorMaterial.GetFloat(BakedLightUtils._stencilCompPropertyId);
          double num7 = (double) noMirrorMaterial.GetFloat(BakedLightUtils._stencilPassOpPropertyId);
          foreach (Renderer renderer in componentsInChildren2)
          {
            if (!((Object) renderer.gameObject == (Object) componentsInChild.gameObject))
            {
              Material sharedMaterial2 = renderer.sharedMaterial;
              double num8 = (double) sharedMaterial2.GetFloat(BakedLightUtils._stencilRefValuePropertyId);
              double num9 = (double) sharedMaterial2.GetFloat(BakedLightUtils._stencilCompPropertyId);
              double num10 = (double) sharedMaterial2.GetFloat(BakedLightUtils._stencilPassOpPropertyId);
            }
          }
        }
      }
    }
    else
      Debug.Log((object) ("\"" + scene.name + "\": No Reflection Probe found in the scene, skipping reflection probe validation"));
    Debug.Log((object) "Baked Lights and Reflection Probe validation finished with success");
  }
}
