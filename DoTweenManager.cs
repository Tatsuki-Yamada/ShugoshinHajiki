using System.Collections.Generic;
using UnityEngine;

namespace ShugoshinHajiki.Objects.Enemy
{
    public class DoTweenManager : MonoBehaviour
    {
        /// <summary>
        /// アクセスを楽にするためだけのSingleton化、自動生成等は一切なし。
        /// </summary>
        private static DoTweenManager _instance;
        public static DoTweenManager _Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (DoTweenManager)FindObjectOfType(typeof(DoTweenManager));
                    if (_instance == null)
                    {
                        Debug.Log("DoTweenManagerが見つかりません。");
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// 左上が0、右下が24、左から右に5ずつ進んでいく。
        /// </summary>
        Dictionary<int, Vector2> posPreset = new Dictionary<int, Vector2>()
    {
        {0, new Vector2(-8, 6) },
        {1, new Vector2(-4, 6) },
        {2, new Vector2(0, 6) },
        {3, new Vector2(4, 6) },
        {4, new Vector2(8, 6) },
        {5, new Vector2(-8, 3.5f) },
        {6, new Vector2(-4, 3.5f) },
        {7, new Vector2(0, 3.5f) },
        {8, new Vector2(4, 3.5f) },
        {9, new Vector2(8, 3.5f) },
        {10, new Vector2(-8, 0) },
        {11, new Vector2(-4, 0) },
        {12, new Vector2(0, 0) },
        {13, new Vector2(4, 0) },
        {14, new Vector2(8, 0) },
        {15, new Vector2(-8, -3.5f) },
        {16, new Vector2(-4, -3.5f) },
        {17, new Vector2(0, -3.5f) },
        {18, new Vector2(4, -3.5f) },
        {19, new Vector2(8, -3.5f) },
        {20, new Vector2(-8, -6) },
        {21, new Vector2(-4, -6) },
        {22, new Vector2(0, -6) },
        {23, new Vector2(4, -6) },
        {24, new Vector2(8, -6) },
    };

        Dictionary<int, Vector2> specialPosPreset = new Dictionary<int, Vector2>()
        {
            {0, new Vector2(0, 3.5f) },
            {1, new Vector2(2.75f, -1.5f) },
            {2, new Vector2(-2.75f, -1.5f) },
        };

        // int _debugIndex = 1;


        private void Update()
        {
            // ポジションの確認をしたいときはコメントを外す
            /*
            if(Input.GetKeyDown(KeyCode.J))
            {
                num--;
                transform.position = posPreset[debugIndex];
            }
            else if(Input.GetKeyDown(KeyCode.K))
            {
                num++;
                transform.position = posPreset[debugIndex];
            }
            */
        }


        /// <summary>
        /// 引数のポジションidに対応する座標を、まとめてリストで返します。
        /// </summary>
        /// <param name="indexes">例：GetPath(1, 24, 3, ...)</param>
        /// <returns></returns>
        public Vector3[] GetPath(params int[] indexes)
        {
            List<Vector3> _returnList = new List<Vector3>();

            foreach (int i in indexes)
            {
                _returnList.Add(posPreset[i - 1]);
            }

            return _returnList.ToArray();
        }

    }
}
