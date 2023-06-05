using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSimulation : MonoBehaviour
{
    public float G = 1f;
    GameObject[] astroObjects;

    // Start is called before the first frame update
    void Start()
    {
        astroObjects = GameObject.FindGameObjectsWithTag("AstroObject");

        InitialVelocity();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Gravity();
        Flatten();
    }

    void Gravity()
    {
        foreach (GameObject a in astroObjects)
        {
            foreach (GameObject b in astroObjects)
            {
                if (!a.Equals(b))
                {
                    // Apply Newton's law of universal gravitation
                    // F = G * (m1 * m2) / r^2

                    float m1 = a.GetComponent<Rigidbody>().mass;
                    float m2 = b.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);

                    // calculate direction and magnitude of force
                    float F_magnitude = G * (m1 * m2) / (r * r);
                    Vector3 F_direction = (b.transform.position - a.transform.position).normalized;

                    a.GetComponent<Rigidbody>().AddForce(F_direction * F_magnitude);
                }
            }
        }
    }

    public void InitialVelocity()
    {
        foreach (GameObject a in astroObjects)
        {
            foreach (GameObject b in astroObjects)
            {
                if (!a.Equals(b))
                {
                    float m2 = b.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);
                    a.transform.LookAt(b.transform);

                    a.GetComponent<Rigidbody>().velocity += a.transform.right * Mathf.Sqrt((G * m2) / r);
                }
            }
        }
    }

    void Flatten()
    {
        foreach (GameObject a in astroObjects)
        {
            a.transform.position = new Vector3(a.transform.position.x, 0, a.transform.position.z);
        }
    }
}
