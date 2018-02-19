using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillFigureScript : MonoBehaviour {

	private RawImage image;
	private Texture2D texture;
	private bool isDraw = false;
	private List<Vector2> fullCoordinates = new List<Vector2>();
	private List<Vector2> currentCoordinates = new List<Vector2>();
	private Vector2 startCoord;

	private void Start()
	{
		image = GetComponent<RawImage>();
		texture = new Texture2D(1920, 1080);

		for (int i = 0; i < texture.width; i++)
		{
			for (int j = 0; j < texture.height; j++)
			{
				texture.SetPixel(i, j, Color.green);
			}
		}

		texture.Apply();
		image.texture = texture;
		image.SetNativeSize();
	}

	private void Update()
	{
		if (Input.anyKeyDown)
		{
			if (fullCoordinates.Count > 1)
			{
				for (int i = 0; i < fullCoordinates.Count - 1; i++)
				{
					DrawLine(texture, fullCoordinates[i], fullCoordinates[i + 1], Color.green);
				}
				DrawLine(texture, fullCoordinates[0], fullCoordinates[fullCoordinates.Count - 1], Color.green);
				fullCoordinates.RemoveRange(0, fullCoordinates.Count);
			}
			texture.Apply();
			isDraw = true;
			startCoord = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		}

		if (Input.GetMouseButtonUp(0))
		{
			isDraw = false;
			if (currentCoordinates.Count > 0)
			{
				DrawLine(texture, startCoord, currentCoordinates[0], Color.red);
			}

			FillLeft((int)(startCoord.x + fullCoordinates[fullCoordinates.Count / 2].x) / 2,
				(int)(startCoord.y + fullCoordinates[fullCoordinates.Count / 2].y) / 2, Color.black);

			texture.Apply();
			currentCoordinates.RemoveRange(0, currentCoordinates.Count);
		}

		if (isDraw)
		{
			texture.SetPixel((int)Input.mousePosition.x, (int)Input.mousePosition.y, Color.red);
			fullCoordinates.Add(Input.mousePosition);
			currentCoordinates.Add(Input.mousePosition);
			if (currentCoordinates.Count >= 2)
			{
				DrawLine(texture, currentCoordinates[0], currentCoordinates[1], Color.red);
				currentCoordinates.RemoveRange(0, 1);
			}
			texture.Apply();
		}
	}

	public void DrawLine(Texture2D tex, Vector2 p1, Vector2 p2, Color col)
	{
		Vector2 t = p1;
		float frac = 1 / Mathf.Sqrt(Mathf.Pow(p2.x - p1.x, 2) + Mathf.Pow(p2.y - p1.y, 2));
		float ctr = 0;

		while ((int)t.x != (int)p2.x || (int)t.y != (int)p2.y)
		{
			t = Vector2.Lerp(p1, p2, ctr);
			ctr += frac;
			tex.SetPixel((int)t.x, (int)t.y, col);
		}
	}

	private void FillLeft(int x, int y, Color color)
	{
		if (color != texture.GetPixel(x, y) && texture.GetPixel(x, y) != Color.red && x > 0 && x < 1920 && y > 0 && y < 1080)
		{
			texture.SetPixel(x, y, Color.red);
			FillLeft(x - 1, y, color);
			FillDown(x, y - 1, color);
			FillUp(x, y + 1, color);
		}
	}

	private void FillRight(int x, int y, Color color)
	{
		if (color != texture.GetPixel(x, y) && texture.GetPixel(x, y) != Color.red && x > 0 && x < 1920 && y > 0 && y < 1080)
		{
			texture.SetPixel(x, y, Color.red);
			FillRight(x + 1, y, color);
			FillDown(x, y - 1, color);
			FillUp(x, y + 1, color);
		}
	}

	private void FillDown(int x, int y, Color color)
	{
		if (color != texture.GetPixel(x, y) && texture.GetPixel(x, y) != Color.red && x > 0 && x < 1920 && y > 0 && y < 1080)
		{
			texture.SetPixel(x, y, Color.red);
			if (texture.GetPixel(x + 1, y) != Color.red)
			{
				FillRight(x + 1, y, color);
			}

			FillDown(x, y - 1, color);
		}
	}

	private void FillUp(int x, int y, Color color)
	{
		if (color != texture.GetPixel(x, y) && texture.GetPixel(x, y) != Color.red && x > 0 && x < 1920 && y > 0 && y < 1080)
		{
			texture.SetPixel(x, y, Color.red);
			if (texture.GetPixel(x + 1, y) != Color.red)
			{
				FillRight(x + 1, y, color);
			}

			FillUp(x, y + 1, color);
		}
	}
}