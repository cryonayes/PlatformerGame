using UnityEngine;

namespace Background
{
    public class Parallax : MonoBehaviour
    {
        private float lenght, startpos;

        [SerializeField]
        private GameObject mCamera;

        public float parallaxEffect;
    
        void Start()
        {
            startpos = transform.position.x;
            lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        void FixedUpdate()
        {
            var position = mCamera.transform.position;
            var t = (position.x * (1 - parallaxEffect));
        
            var distance = (position.x * parallaxEffect);
            var temp = transform.position;
            transform.position = new Vector3(startpos + distance, temp.y, temp.z);

            if (t > startpos + lenght) startpos += lenght;
            else if (t < startpos - lenght) startpos -= lenght;
        }
    }
}
