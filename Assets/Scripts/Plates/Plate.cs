using UnityEngine;
using System.Collections.Generic;

namespace Plates
{
    public class Plate : MonoBehaviour
    {
        public static List<Plate> AllPlates = new List<Plate>();
        
        [HideInInspector] public float GScore;
        [HideInInspector] public float HScore;
        [HideInInspector] public Plate CameFrom;

        public List<Plate> Connections;

        private MeshRenderer _renderer;
        private Color _originalColor;
        private bool _hovered;

        public float FScore() => GScore + HScore;

        private void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
            _originalColor = _renderer.material.color;
            
            AllPlates.Add(this);
        }
        
        private void OnDestroy()
        {
            AllPlates.Remove(this);
        }

        private void Update()
        {
            ChangeColor();
        }

        private void ChangeColor()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (!_hovered)
                    {
                        _hovered = true;
                        _renderer.material.color = Color.yellow;
                    }

                    return;
                }
            }

            if (_hovered)
            {
                _hovered = false;
                _renderer.material.color = _originalColor;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            if (Connections.Count > 0)
            {
                foreach (Plate connection in Connections)
                {
                    Gizmos.DrawLine(gameObject.transform.position, connection.transform.position);
                }
            }
        }
    }
}