using UnityEngine;

namespace ShugoshinHajiki.Effects
{
    public class EnemySignController : MonoBehaviour
    {
        public int repeatTimes { get; set; } = 2;
        int _repeatCount;

        /// <summary>
        /// アニメーションが一周終わった時、Animationから呼び出される関数です。
        /// 指定した回数繰り返したら親ごとDestroyしてます。
        /// </summary>
        public void AnimationEnd()
        {
            _repeatCount += 1;

            if (_repeatCount >= repeatTimes)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
