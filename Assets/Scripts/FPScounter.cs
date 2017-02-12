using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using LoLSDK;


namespace Assets.Scripts
{
    public class FPScounter : MonoBehaviour
    {

        double _frameCount = 0;
        double dt = 0.0;
        double fps = 0.0;
        double updateRate = 4.0; // 4 updates per sec.

        private void Update()
        {
            _frameCount++;
            dt += Time.deltaTime;
            if (dt > 1.0/updateRate)
            {
                fps = _frameCount/dt;
                _frameCount = 0;
                dt -= 1.0/updateRate;
            }
            fps = (int) fps;
            GetComponent<Text>().text = fps.ToString(CultureInfo.CurrentCulture);
        }
    }
}