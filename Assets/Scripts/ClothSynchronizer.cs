using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothSynchronizer : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _character, _cloth;

    private void Start() {
        SynchronizeBones();
    }

    private void SynchronizeBones() {
        _cloth.bones = _character.bones;
    }
}
