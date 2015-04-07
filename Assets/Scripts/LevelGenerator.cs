using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

    public int numRooms = 30;
    public int seed = 1337;
    public Vector2 minSize = new Vector2(2f, 2f);
    public Vector2 maxSize = new Vector2(10f, 10f);
    public float maxRatio = 3f;
    public float totalSizeRadius = 15f;
    public float stepTime = 0.2f;
    public Bounds shipBounds = new Bounds(Vector3.zero, new Vector3(20f, 5f, 30f));

    private List<GameObject> rooms = new List<GameObject>();
    private bool allRoomsSeperated = false;
    private float lastStep = 0f;

	// Use this for initialization
	void Start () {
        // Debug draw the shipBounds
        ShowBounds sb = GetComponent<ShowBounds>();
        if (sb != null)
            sb.setBounds(shipBounds);

        Random.seed = seed;
        createRooms();
	}
	
	// Update is called once per frame
	void Update () {
        lastStep += Time.deltaTime;
        if (lastStep > stepTime)
        {
            lastStep = 0f;
            seperateRooms();
            if (allRoomsSeperated)
                removeRoomsNotInBounds();
        }
	}

    void createRooms()
    {
        while (rooms.Count < numRooms)
        {
            GameObject newRoom = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 newScale = new Vector3(Mathf.Round(Random.Range(minSize.x, maxSize.x)), 1f, Mathf.Round(Random.Range(minSize.y, maxSize.y)));
            Vector2 newPosition = Random.insideUnitCircle * totalSizeRadius;
            newPosition.x = Mathf.Round(newPosition.x);
            newPosition.y = Mathf.Round(newPosition.y);
            newRoom.transform.localScale = newScale;
            newRoom.transform.position = new Vector3(newPosition.x, 0f, newPosition.y);
            Renderer rend = newRoom.GetComponent<Renderer>();
            rend.material.SetColor("_Color", new Color(Random.value, Random.value, Random.value));

            if (Mathf.Max(newScale.x, newScale.z) / Mathf.Min(newScale.x, newScale.z) < maxRatio)
                rooms.Add(newRoom);
            else
                Destroy(newRoom);
        }
    }

    void seperateRooms()
    {
        if (allRoomsSeperated)
            return;

        allRoomsSeperated = true;

        foreach (GameObject room in rooms)
        {
            int collissionCount = 0;
            Collider[] hitColliders = Physics.OverlapSphere(room.transform.position, Mathf.Max(room.transform.localScale.x, room.transform.localScale.z) * 1.5f);

            if (hitColliders.Length == 1)
                continue;

            Renderer rend = room.GetComponent<Renderer>();
            Vector3 com = Vector3.zero;
            foreach (Collider c in hitColliders)
            {
                Renderer rend2 = c.GetComponent<Renderer>();
                if (c.gameObject != room && rend.bounds.Intersects(rend2.bounds))
                {
                    com += c.transform.position;
                    Debug.DrawLine(room.transform.position, c.transform.position, room.GetComponent<Renderer>().material.color, stepTime, false);
                    collissionCount++;
                    allRoomsSeperated = false;
                }
            }

            if (collissionCount > 0)
            {
                com /= collissionCount;
                Vector3 move = (room.transform.position - com).normalized;
                move.x = Mathf.Round(move.x);
                move.y = Mathf.Round(move.y);
                move.z = Mathf.Round(move.z);
                Debug.DrawLine(room.transform.position, room.transform.position + move, Color.white, stepTime, false);
                room.transform.position = room.transform.position + move;
            }
                
        }

    }

    void removeRoomsNotInBounds()
    {
        rooms.RemoveAll(room => {
            Renderer rend = room.GetComponent<Renderer>();
            bool remove = !shipBounds.Contains(rend.bounds.min) || !shipBounds.Contains(rend.bounds.max);
            if (remove)
                Destroy(room.gameObject);
            return remove;
        });
    }

    Vector3 centerOfMass(Collider[] colliders)
    {
        Vector3 p = Vector3.zero;
        foreach(Collider c in colliders) {
            p += c.transform.position;
        }
        p /= colliders.Length;
        return p;
    }
}