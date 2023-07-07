using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class GameController : MonoBehaviour
    {
        [Header("Component")]
        [SerializeField] private Stronghold stronghold;

        [Header("Setting")]
        [SerializeField] private Transform[] wayPoints;
    }
}
