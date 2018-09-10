using UnityEngine;
using System.Collections.Generic;
using System;

public class CharacterMgr
{
    private static int _nGenerateIndex = 0;
    private static Dictionary<int, DiyCharacterController> _dicCharacter = new Dictionary<int, DiyCharacterController>();

    internal static DiyCharacterController Generate(string[] configs, bool combine = false)
    {
        DiyCharacterController characterInst = new DiyCharacterController(_nGenerateIndex, configs[0], configs[1], configs[2], configs[3], configs[4], configs[5]);
        characterInst.Generator(combine);

        _dicCharacter.Add(_nGenerateIndex, characterInst);
        ++_nGenerateIndex;

        return characterInst;
    }

    internal static void ChangeSkin(DiyCharacterController characterInst, string[] configs, bool combine = false)
    {
        characterInst.ChangeSkin(configs[1], configs[2], configs[3], configs[4], configs[5]);
        characterInst.Regenerator(combine);
    }

    internal static void ChangeSkin(int characterTag, string[] configs, bool combine = false)
    {
        DiyCharacterController characterInst = null;
        _dicCharacter.TryGetValue(characterTag, out characterInst);

        if (null == characterInst)
        {
            return;
        }

        ChangeSkin(characterInst, configs, combine);
    }
}
