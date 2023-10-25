using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ModestTree.Util
{
    // Token: 0x0200001D RID: 29
    public static class UnityUtil
    {
        // Token: 0x17000009 RID: 9
        // (get) Token: 0x060000C2 RID: 194 RVA: 0x00003C10 File Offset: 0x00001E10
        public static IEnumerable<Scene> AllScenes
        {
            get
            {
                int num;
                for (int i = 0; i < SceneManager.sceneCount; i = num + 1)
                {
                    yield return SceneManager.GetSceneAt(i);
                    num = i;
                }
                yield break;
            }
        }

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x060000C3 RID: 195 RVA: 0x00003C1C File Offset: 0x00001E1C
        public static IEnumerable<Scene> AllLoadedScenes
        {
            get
            {
                return from scene in UnityUtil.AllScenes
                       where scene.isLoaded
                       select scene;
            }
        }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x060000C4 RID: 196 RVA: 0x00003C48 File Offset: 0x00001E48
        public static bool IsAltKeyDown
        {
            get
            {
                return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
            }
        }

        // Token: 0x1700000C RID: 12
        // (get) Token: 0x060000C5 RID: 197 RVA: 0x00003C64 File Offset: 0x00001E64
        public static bool IsControlKeyDown
        {
            get
            {
                return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            }
        }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x060000C6 RID: 198 RVA: 0x00003C80 File Offset: 0x00001E80
        public static bool IsShiftKeyDown
        {
            get
            {
                return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            }
        }

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x060000C7 RID: 199 RVA: 0x00003C9C File Offset: 0x00001E9C
        public static bool WasShiftKeyJustPressed
        {
            get
            {
                return Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
            }
        }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x060000C8 RID: 200 RVA: 0x00003CB8 File Offset: 0x00001EB8
        public static bool WasAltKeyJustPressed
        {
            get
            {
                return Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt);
            }
        }

        // Token: 0x060000C9 RID: 201 RVA: 0x00003CD4 File Offset: 0x00001ED4
        public static int GetDepthLevel(Transform transform)
        {
            if (transform == null)
            {
                return 0;
            }
            return 1 + UnityUtil.GetDepthLevel(transform.parent);
        }

        // Token: 0x060000CA RID: 202 RVA: 0x00003CF0 File Offset: 0x00001EF0
        public static GameObject GetRootParentOrSelf(GameObject gameObject)
        {
            return (from x in UnityUtil.GetParentsAndSelf(gameObject.transform)
                    select x.gameObject).LastOrDefault<GameObject>();
        }

        // Token: 0x060000CB RID: 203 RVA: 0x00003D28 File Offset: 0x00001F28
        public static IEnumerable<Transform> GetParents(Transform transform)
        {
            if (transform == null)
            {
                yield break;
            }
            foreach (Transform transform2 in UnityUtil.GetParentsAndSelf(transform.parent))
            {
                yield return transform2;
            }
            IEnumerator<Transform> enumerator = null;
            yield break;
            yield break;
        }

        // Token: 0x060000CC RID: 204 RVA: 0x00003D38 File Offset: 0x00001F38
        public static IEnumerable<Transform> GetParentsAndSelf(Transform transform)
        {
            if (transform == null)
            {
                yield break;
            }
            yield return transform;
            foreach (Transform transform2 in UnityUtil.GetParentsAndSelf(transform.parent))
            {
                yield return transform2;
            }
            IEnumerator<Transform> enumerator = null;
            yield break;
            yield break;
        }

        // Token: 0x060000CD RID: 205 RVA: 0x00003D48 File Offset: 0x00001F48
        public static IEnumerable<Component> GetComponentsInChildrenTopDown(GameObject gameObject, bool includeInactive)
        {
            return gameObject.GetComponentsInChildren<Component>(includeInactive).OrderBy(delegate (Component x)
            {
                if (!(x == null))
                {
                    return UnityUtil.GetDepthLevel(x.transform);
                }
                return int.MinValue;
            });
        }

        // Token: 0x060000CE RID: 206 RVA: 0x00003D78 File Offset: 0x00001F78
        public static IEnumerable<Component> GetComponentsInChildrenBottomUp(GameObject gameObject, bool includeInactive)
        {
            return gameObject.GetComponentsInChildren<Component>(includeInactive).OrderByDescending(delegate (Component x)
            {
                if (!(x == null))
                {
                    return UnityUtil.GetDepthLevel(x.transform);
                }
                return int.MinValue;
            });
        }

        // Token: 0x060000CF RID: 207 RVA: 0x00003DA8 File Offset: 0x00001FA8
        public static IEnumerable<GameObject> GetDirectChildrenAndSelf(GameObject obj)
        {
            yield return obj;
            foreach (object obj2 in obj.transform)
            {
                Transform transform = (Transform)obj2;
                yield return transform.gameObject;
            }
            IEnumerator enumerator = null;
            yield break;
            yield break;
        }

        // Token: 0x060000D0 RID: 208 RVA: 0x00003DB8 File Offset: 0x00001FB8
        public static IEnumerable<GameObject> GetDirectChildren(GameObject obj)
        {
            foreach (object obj2 in obj.transform)
            {
                Transform transform = (Transform)obj2;
                yield return transform.gameObject;
            }
            IEnumerator enumerator = null;
            yield break;
            yield break;
        }

        // Token: 0x060000D1 RID: 209 RVA: 0x00003DC8 File Offset: 0x00001FC8
        public static IEnumerable<GameObject> GetAllGameObjects()
        {
            return from x in UnityEngine.Object.FindObjectsOfType<Transform>()
                   select x.gameObject;
        }

        // Token: 0x060000D2 RID: 210 RVA: 0x00003DF4 File Offset: 0x00001FF4
        public static List<GameObject> GetAllRootGameObjects()
        {
            return (from x in UnityUtil.GetAllGameObjects()
                    where x.transform.parent == null
                    select x).ToList<GameObject>();
        }
    }
}
