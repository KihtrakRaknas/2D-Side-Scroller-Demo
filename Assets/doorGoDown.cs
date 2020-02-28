using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorGoDown : MonoBehaviour
{
    // Start is called before the first frame update
    public static int killedBois;
    void Start()
    {
        killedBois = 0;
    }

    // Update is called once per frame
    void Update()
    {
        print(killedBois);
        if (killedBois > 20)
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y-.01f);
    }
}
