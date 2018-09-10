using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CombineSkinnedMgr
{

    /// <summary>
    /// Only for merge materials.
    /// </summary>
    private const int COMBINE_TEXTURE_MAX = 512;
    private const string COMBINE_DIFFUSE_TEXTURE = "_MainTex";

    /// <summary>
    /// Combine SkinnedMeshRenderers together and share one skeleton.
    /// Merge materials will reduce the drawcalls, but it will increase the size of memory. 
    /// </summary>
    /// <param name="go">combine meshes to this skeleton(a gameobject)</param>
    /// <param name="smrs">meshes need to be merged</param>
    /// <param name="combine">merge materials or not</param>
    internal static void Combine(GameObject go, SkinnedMeshRenderer[] smrs, bool combine)
    {
        // Fetch all bones of the skeleton
        List<Transform> transforms = new List<Transform>();
        transforms.AddRange(go.GetComponentsInChildren<Transform>());

        List<Material> materials = new List<Material>();
        List<Transform> bones = new List<Transform>();
        List<CombineInstance> combineInstances = new List<CombineInstance>(); //the list of meshes

        //collect informations from smrs
        for (int i = 0; i < smrs.Length; ++i)
        {
            SkinnedMeshRenderer smr = smrs[i];
            materials.AddRange(smr.materials);

            for (int subIndex = 0; subIndex < smr.sharedMesh.subMeshCount; ++subIndex)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = smr.sharedMesh;
                ci.subMeshIndex = subIndex;
                combineInstances.Add(ci);
            }

            for (int j = 0; j < smr.bones.Length; ++j)
            {
                for (int z = 0; z < transforms.Count; ++z)
                {
                    if (smr.bones[j].name.Equals(transforms[z].name))
                    {
                        bones.Add(transforms[z]);
                        break;
                    }
                }
            }
        }

        if (combine)
        {
            // Below informations only are used for merge materilas(bool combine = true)
            Material newMaterial = new Material(Shader.Find("Mobile/Diffuse"));
            List<Vector2[]> oldUV = new List<Vector2[]>();
            //merge texture
            List<Texture2D> textures = new List<Texture2D>();
            for (int i = 0; i < materials.Count; ++i)
            {
                textures.Add(materials[i].GetTexture(COMBINE_DIFFUSE_TEXTURE) as Texture2D);
            }

            Texture2D newDiffuseTex = new Texture2D(COMBINE_TEXTURE_MAX, COMBINE_TEXTURE_MAX, TextureFormat.RGBA32, true);
            Rect[] uvs = newDiffuseTex.PackTextures(textures.ToArray(), 0);
            newMaterial.mainTexture = newDiffuseTex;

            //reset uv
            Vector2[] uva, uvb;
            for (int j = 0; j < combineInstances.Count; ++j)
            {
                uva = (Vector2[])(combineInstances[j].mesh.uv);
                uvb = new Vector2[uva.Length];
                for (int k = 0; k < uva.Length; ++k)
                {
                    uvb[k] = new Vector2((uva[k].x * uvs[j].width) + uvs[j].x, (uva[k].y * uvs[j].height) + uvs[j].y);
                }
                oldUV.Add(combineInstances[j].mesh.uv);
                combineInstances[j].mesh.uv = uvb;
            }

            // Create a new SkinnedMeshRenderer
            SkinnedMeshRenderer oldSmr = go.GetComponent<SkinnedMeshRenderer>();
            if (oldSmr != null)
            {
                GameObject.DestroyImmediate(oldSmr);
            }
            SkinnedMeshRenderer smr = go.AddComponent<SkinnedMeshRenderer>();
            smr.sharedMesh = new Mesh();
            smr.sharedMesh.CombineMeshes(combineInstances.ToArray(), combine, false);// Combine meshes
            smr.bones = bones.ToArray();// Use new bones
            if (combine)
            {
                smr.material = newMaterial;
                for (int i = 0; i < combineInstances.Count; ++i)
                {
                    combineInstances[i].mesh.uv = oldUV[i];
                }
            }
            else
            {
                smr.materials = materials.ToArray();
            }
        }
    }
}
