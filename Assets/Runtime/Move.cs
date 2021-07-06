using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Will
{
    public class Move : MonoBehaviour
    {
        public float Speed = 1f;
        public float Range = 1f;

        private Transform _transform;
        private Vector3 _originPos;

        // Start is called before the first frame update
        void Start()
        {
            _transform = this.gameObject.transform;
            _originPos = _transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            float delta = Range * Mathf.Sin(Speed * Time.time);
            Vector3 newPos = new Vector3(_originPos.x, _originPos.y + delta, _originPos.z);
            _transform.position = newPos;
        }
    }

}

