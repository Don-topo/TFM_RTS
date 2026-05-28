using UnityEngine;

public class ShowBox : MonoBehaviour
{
    public Vector3 boxSize = new Vector3(2, 2, 2);
    public float offset = 1f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        // Aplicar posición y rotación
        Gizmos.matrix = Matrix4x4.TRS(
            transform.position + new Vector3(offset, 0,0),
            transform.rotation,
            Vector3.one
        );

        // Dibujar caja
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
