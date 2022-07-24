using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void EnemyDie()
    {
        Destroy(transform.parent.gameObject);
    }
}
