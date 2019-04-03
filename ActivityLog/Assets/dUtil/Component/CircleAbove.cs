using UnityEngine;


public class CircleAbove : MonoBehaviour {

    [SerializeField] Vector3 m_speed = Vector3.zero;

    private void Update() {
        transform.Rotate(m_speed);
    }

}
