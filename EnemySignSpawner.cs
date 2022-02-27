using UnityEngine;

namespace ShugoshinHajiki.Effects
{
    public class EnemySignSpawner : MonoBehaviour
    {
        [SerializeField, Tooltip("敵の予兆エフェクトのPrefabです。")] GameObject enemySignPrefab;
        [SerializeField, Tooltip("エフェクトの長さです。")] float effectLength = 0.5f;
        [SerializeField, Tooltip("エフェクトの幅です。")] float effectWidth = 1f;
        [SerializeField, Tooltip("アニメーションを繰り返す回数です。")] int repeatTimes = 2;


        /// <summary>
        /// EnemySpawnerの位置をもらって、エフェクトを生成します。
        /// </summary>
        /// <param name="spawnerPos"></param>
        public async void Spawn(Vector2 spawnerPos)
        {
            // エフェクトを生成してセンターに向ける処理です。
            GameObject obj = Instantiate(enemySignPrefab, spawnerPos, Quaternion.identity);
            obj.transform.rotation = Quaternion.FromToRotation(Vector2.up, spawnerPos);

            // アニメーションのループ回数を設定します。
            obj.transform.GetChild(0).gameObject.GetComponent<EnemySignController>().repeatTimes = repeatTimes;

            // エフェクトのサイズを距離に応じて変えます。
            float diff = Vector2.Distance(obj.transform.position, Vector2.zero);
            obj.transform.localScale = new Vector2(obj.transform.localScale.x * effectWidth, obj.transform.localScale.y * diff * effectLength);
            obj.transform.position = Vector2.MoveTowards(obj.transform.position, Vector2.zero, diff * effectLength);

            await System.Threading.Tasks.Task.Delay(500);
        }
    }
}
