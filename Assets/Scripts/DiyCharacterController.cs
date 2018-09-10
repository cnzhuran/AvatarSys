using System;
using UnityEditor;
using UnityEngine;

public class DiyCharacterController
{
    private int generateTag;
    private string skeleton;
    private string head;
    private string chest;
    private string hand;
    private string feet;
    private string weapon;
    private bool combine;

    private GameObject m_goRoot;
    private GameObject m_goWeapon;

    private Animation m_animation;

    public DiyCharacterController(int generateTag, string skeleton, string head, string chest, string hand, string feet, string weapon)
    {
        this.generateTag = generateTag;
        this.skeleton = skeleton;

        ChangeSkin(head, chest, hand, feet, weapon);
    }

    internal void ChangeSkin(string head, string chest, string hand, string feet, string weapon)
    {
        this.head = head;
        this.chest = chest;
        this.hand = hand;
        this.feet = feet;
        this.weapon = weapon;
    }

    internal void Generator(bool combine)
    {
        //create skeleton
        GameObject skeletonRes = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/" + skeleton + ".prefab");
        this.m_goRoot = GameObject.Instantiate<GameObject>(skeletonRes);

        //animation
        m_animation = this.m_goRoot.GetComponent<Animation>();

        //others
        Regenerator(combine);

        PlayAnimation("breath");
    }

    internal void Regenerator(bool combine)
    {
        string[] parts = new string[4]
            {
                this.head,
                this.chest,
                this.hand,
                this.feet
            };
        //collect other parts SkinnedMeshRenderer
        GameObject[] gos = new GameObject[4];
        SkinnedMeshRenderer[] smrs = new SkinnedMeshRenderer[4];
        for (int i = 0; i < parts.Length; ++i)
        {
            GameObject goPart = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/" + parts[i] + ".prefab");
            gos[i] = GameObject.Instantiate<GameObject>(goPart);
            smrs[i] = gos[i].GetComponentInChildren<SkinnedMeshRenderer>();
        }

        //combine meshes
        CombineSkinnedMgr.Combine(this.m_goRoot, smrs, combine);

        // Delete temporal resources
        for (int i = 0; i < gos.Length; i++)
        {
            GameObject.DestroyImmediate(gos[i].gameObject);
        }

        // Create weapon
        GameObject weaponRes = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/" + weapon + ".prefab");
        m_goWeapon = GameObject.Instantiate(weaponRes) as GameObject;

        Transform[] transforms = m_goRoot.GetComponentsInChildren<Transform>();
        foreach (Transform joint in transforms)
        {
            if (joint.name == "weapon_hand_r")
            {// find the joint (need the support of art designer)
                m_goWeapon.transform.parent = joint.gameObject.transform;
                break;
            }
        }

        // Init weapon relative informations
        m_goWeapon.transform.localScale = Vector3.one;
        m_goWeapon.transform.localPosition = Vector3.zero;
        m_goWeapon.transform.localRotation = Quaternion.identity;
    }

    private void PlayAnimation(string name)
    {
        m_animation.wrapMode = WrapMode.Loop;
        m_animation.Play(name);
    }

}
