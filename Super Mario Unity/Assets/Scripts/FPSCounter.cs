using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
	private float fpsMeasurePeriod = 0.25f;
	private int fpsAccumulator = 0;
	private float fpsNextPeriod = 0;
	private int currentFps;
	private Text fpsText;
	
	void Start()
	{
		fpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
		fpsText = GetComponent<Text>();
	}
	
	
	void Update()
	{
		fpsAccumulator++;
		if (Time.realtimeSinceStartup > fpsNextPeriod)
		{
			currentFps = (int) (fpsAccumulator/fpsMeasurePeriod);
			fpsAccumulator = 0;
			fpsNextPeriod += fpsMeasurePeriod;
			fpsText.text = string.Format("{0}", currentFps);
		}
	}
}
