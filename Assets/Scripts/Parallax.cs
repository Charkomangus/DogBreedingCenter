using UnityEngine;
using LoLSDK;

namespace Assets.Scripts
{
    public class Parallax : MonoBehaviour
    {
        public Transform[] ParallaxBackgrounds; //Parallax backgrounds
        public float Speed =0; //amount of smoothing
        private float[] _parallaxScales; //The proportion of the movement 
        private Transform _mainCamera; //Main camera
        private Vector3 _previousCameraPosition; //Stores poisiton of camera on last frame
        

        void Awake()
        {
            //Set up camera reference
            _mainCamera = Camera.main.transform;
            ParallaxBackgrounds = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                ParallaxBackgrounds[i] = transform.GetChild(i);
            }

           
        }

        // Use this for initialization
        void Start()
        {
            //Store Camera Position
            _previousCameraPosition = _mainCamera.position;

            //Set up parallax scales array
            _parallaxScales = new float[ParallaxBackgrounds.Length];

            //Set corresponding negative depth
            for (int i = 0; i < ParallaxBackgrounds.Length; i++)
                _parallaxScales[i] = ParallaxBackgrounds[i].position.z*-1;

        }

        // Update is called once per frame
        void Update()
        {
       
            //Set parallax using difference in camera movement from last scale and its scale
            for (int i = 0; i < ParallaxBackgrounds.Length; i++)
            {
                float parallax = (_previousCameraPosition.x - _mainCamera.position.x)*_parallaxScales[i];


                //Set target position
                float backgroundTarget = ParallaxBackgrounds[i].position.x + parallax;
                Vector3 backgroundTargetPosition = new Vector3(backgroundTarget, ParallaxBackgrounds[i].position.y, ParallaxBackgrounds[i].position.z);

                //Fade between current position and the target position
                ParallaxBackgrounds[i].position = Vector3.Lerp(ParallaxBackgrounds[i].position, backgroundTargetPosition, Speed*Time.deltaTime);
            }

            _previousCameraPosition = _mainCamera.position;
        }

    }
}
