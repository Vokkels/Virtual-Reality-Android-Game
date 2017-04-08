/**
 * @author Zander Labuschagne
 * E-mail: ZANDER.LABUSCHAGNE@PROTONMAIL.CH
 * Class which checks whether the target was hit or missed
 * If a player was hit, his or her named can be determined because every player's object has an object of this class.
**/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectRecognition
{
	private const int WIDTH = 48;
	private const int HEIGHT = 48;
	private const int CROSSHAIR_SIZE_X = 16;
	private const int CROSSHAIR_SIZE_Y = 16;
	private Color targetColor;
	private Color scannedColor;
	private float scannedHue;
	private float scannedSaturation;
	private float scannedValue;
	private const float HUE_TOLERANCE = 0.025f;
	private const float SATURATION_TOLERANCE = 0.1f;
	private Material screenshot;

	/**
	* Example of how the main or calling method would look like
	* Example to show how this class should be used
	*/
	public void clickCompute()
	{
		bool hit = getHit(camera);

		if(hit)
			//debugger.GetComponent<Text>().text = debugger.GetComponent<Text>().text + "\r\nHIT!";
			debugger.GetComponent<Text>().text = "HIT!";
		else
			//debugger.GetComponent<Text>().text = debugger.GetComponent<Text>().text + "\r\nMISS!";
			debugger.GetComponent<Text>().text = "MISS!";
	}

	/**
	* Scan the color of the player's shirt before the game starts
	* @param camara is the access to the camera this method receives
	* This method calculates the average RGB color at the center of the camera image with 16x16 pixel size
	* The result is assigned to scannedColor
	*/
	public void setColor(WebCamTexture camera)
	{
		Color[] crosshairPixelSet = camera.GetPixels (camera.width / 2 - CROSSHAIR_SIZE_X / 2, camera.height / 2 - CROSSHAIR_SIZE_Y / 2, CROSSHAIR_SIZE_X, CROSSHAIR_SIZE_Y);
		int total = crosshairPixelSet.Length;
		float r = 0f;
		float g = 0f;
		float b = 0f;
		for(int i = 0; i < total; i++)
		{
			r += crosshairPixelSet[i].r;
			g += crosshairPixelSet[i].g;
			b += crosshairPixelSet[i].b;
		}
		float scannedRed = r / total;
		float scannedGreen = g / total;
		float scannedBlue = b / total;
		scannedColor = new Color (scannedRed, scannedGreen, scannedBlue);
	}

	/**
	* This method calculates the average RGB color from a set of RGB pixels
	* @param colors is the RGB pixel set to be used to calculate the average color from
	* @return targetColor which is the average RGB color calculated
	*/
	public Color getAverageColor(Color[] colors)
	{
		int total = colors.Length;
		float r = 0f;
		float g = 0f;
		float b = 0f;

		for(int i = 0; i < total; i++)
		{
			r += colors[i].r;
			g += colors[i].g;
			b += colors[i].b;
		}
		r = r / total;
		g = g / total;
		b = b / total;

		targetColor = new Color(r, g, b);
		return targetColor;
	}

	/**
	* Determines whether a person was hit or missed
	* @param camera is the access to the camera this method receives
	* @return true if a player was hit
	* @return false if a player was missed
	* This method is executed for every shot fired.
	*/
	public bool getHit(WebCamTexture camera)
	{
		Color[] crosshairPixelSet = camera.GetPixels (camera.width / 2 - CROSSHAIR_SIZE_X / 2, camera.height / 2 - CROSSHAIR_SIZE_Y / 2, CROSSHAIR_SIZE_X, CROSSHAIR_SIZE_Y);//Client

		targetColor = getAverageColor(crosshairPixelSet);//Client

		float h = 0f;
		float s = 0f;
		float v = 0f;

		Color.RGBToHSV (color, out h, out s, out v);
		Color.RGBToHSV (scannedColor, out scannedHue, out scannedSaturation, out scannedValue);
		//send h, s to server for comparrison and hit determination
		return getHueHit (h) && getSaturationHit(s);
	}

	/**
	* Hue was brought into the calculation to improve accuracy in variable illumination circumsances
	* Determines whether the hue is within the tolerance range of the original scanned hue
	* @param h the hue value from the camera shot which is compared to the original scanned hue
	* @return true if the hue value supports the hit
	* @return false if the hue is not within the hit tolerance
	*/
	private bool getHueHit(float h)
	{
		if (h <= scannedHue + HUE_TOLERANCE && h >= scannedHue - HUE_TOLERANCE)
			return true;
		return false;
	}

	/**
	* Saturation was brought into the calculation as well to improve accuracy in variable illumination circumsances
	* Determines whether the saturation is within the tolerance range of the original scanned saturation
	* @param s the saturation value from the camera shot which is compared to the original scanned saturation
	* @return true if the saturation value supports the hit
	* @return false if the saturation is not within the hit tolerance
	*/
	private bool getSaturationHit(float s)
	{
		if (s <= scannedSaturation + SATURATION_TOLERANCE && s >= scannedSaturation - SATURATION_TOLERANCE)
			return true;
		return false;
	}
}

//Algorithm to determine dominant color
//float maxColor = 0;
/*if(Mathf.Max(r, g) == r && Mathf.Max(r, b) == r)//Add tolerance factor into calculation if more than 3 colors are used
			return Color.red;
		if(Mathf.Max(r, g) == g && Mathf.Max(g, b) == g)//Add tolerance factor into calculation if more than 3 colors are used
			return Color.green;
		if(Mathf.Max(r, b) == b && Mathf.Max(g, b) == b)//Add tolerance factor into calculation if more than 3 colors are used
			return Color.blue;*/
