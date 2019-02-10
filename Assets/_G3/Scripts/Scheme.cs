using UnityEngine;

public class Scheme : MonoBehaviour {

    static public Color Red {
        // #bd1e1e
        get { return new Color32(189, 30, 30, 255); }
    }

    static public Color Blue {
        // #5171a5
        get { return new Color32(81, 113, 165, 255);  }
    }

    static public Color Green {
        // #50a059
        get { return new Color32(80, 160, 89, 255); }
    }

    static public Color Yellow {
        // #f2c14e
        get { return new Color32(242, 193, 78, 255); }
    }

    static public Color Purple {
        // #3D2D5C
        get { return new Color32(61, 45, 92, 255); }
    }
}
