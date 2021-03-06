using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField]
    private float moveYSpeed = 20f;

    [SerializeField]
    private float lifetime = 3f;

    [SerializeField]
    private float disappearSpeed = 3f;

    [SerializeField]
    private Color damageColor = Color.red;

    [SerializeField]
    private Color healthColor = Color.green;

    private TextMeshPro textMesh = null;
    private Color textColor = Color.black;

    private void Awake() {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(int _dmg) {
        if(_dmg < 0) {
            textMesh.color = healthColor;
        }else {
            textMesh.color = damageColor;
        }
        textMesh.SetText((-_dmg).ToString());
        textColor = textMesh.color;
    }

    private void Update() {
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        lifetime -= Time.deltaTime;

        if(lifetime <= 0) {
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if(textColor.a < 0)
                Destroy(gameObject);
        }
    }
}
