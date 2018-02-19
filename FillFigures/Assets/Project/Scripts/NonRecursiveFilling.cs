using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class NonRecursiveFilling : MonoBehaviour {

	private RawImage image;
	private Texture2D texture;
	private bool isDraw = false;
	private List<Vector2> fullCoordinates = new List<Vector2>();
	private List<Vector2> currentCoordinates = new List<Vector2>();
	private Vector2 startCoord;

	void Start () {
		image = GetComponent<RawImage>();
		texture = new Texture2D(1920, 1080);

		for (int i = 0; i < texture.width; i++)
		{
			for (int j = 0; j < texture.height; j++)
			{
				texture.SetPixel(i, j, Color.green);
			}
		}

		//DrawLine(texture, new Vector2(960, 540), new Vector2(960, 640), Color.black);
		//DrawLine(texture, new Vector2(960, 640), new Vector2(1060, 740), Color.black);
		//DrawLine(texture, new Vector2(1060, 740), new Vector2(1160, 740), Color.black);
		//DrawLine(texture, new Vector2(1160, 740), new Vector2(1060, 640), Color.black);
		//DrawLine(texture, new Vector2(1060, 640), new Vector2(1060, 540), Color.black);
		//DrawLine(texture, new Vector2(1060, 540), new Vector2(1160, 540), Color.black);
		//DrawLine(texture, new Vector2(1160, 540), new Vector2(1060, 440), Color.black);
		//DrawLine(texture, new Vector2(1060, 440), new Vector2(860, 440), Color.black);
		//DrawLine(texture, new Vector2(860, 440), new Vector2(760, 540), Color.black);
		//DrawLine(texture, new Vector2(760, 540), new Vector2(860, 640), Color.black);
		//DrawLine(texture, new Vector2(860, 640), new Vector2(960, 540), Color.black);

		//FillFigure(new Vector2(1010, 540), Color.blue);

		texture.Apply();
		image.texture = texture;
		image.SetNativeSize();
	}
	
	void Update () {
		if (Input.anyKeyDown)
		{
			if (fullCoordinates.Count > 1)
			{
				for (int i = 0; i < fullCoordinates.Count - 1; i++)
				{
					DrawLine(texture, fullCoordinates[i], fullCoordinates[i + 1], Color.black);
				}
				DrawLine(texture, fullCoordinates[0], fullCoordinates[fullCoordinates.Count - 1], Color.black);
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
				DrawLine(texture, startCoord, currentCoordinates[0], Color.black);
			}
			FillFigure(new Vector2(1010, 540), Color.blue);
			texture.Apply();
			currentCoordinates.RemoveRange(0, currentCoordinates.Count);
		}

		if (isDraw)
		{
			texture.SetPixel((int)Input.mousePosition.x, (int)Input.mousePosition.y, Color.black);
			fullCoordinates.Add(Input.mousePosition);
			currentCoordinates.Add(Input.mousePosition);
			if (currentCoordinates.Count >= 2)
			{
				DrawLine(texture, currentCoordinates[0], currentCoordinates[1], Color.black);
				currentCoordinates.RemoveRange(0, 1);
			}
			texture.Apply();
		}
	}

	void FillFigure(Vector2 vec, Color color)
	{
		List<Vector2> coordinate = new List<Vector2>();
		int x = (int) vec.x;
		int y = (int) vec.y;
		while (texture.GetPixel(x, y) != Color.black)
		{
			int rightX = x + 1;
			while (texture.GetPixel(rightX, y) != Color.black)
			{
				texture.SetPixel(rightX, y, Color.red);
				rightX++;
			}

			int leftX = x - 1;
			while (texture.GetPixel(leftX, y) != Color.black)
			{
				texture.SetPixel(leftX, y, Color.red);
				leftX--;
			}

			texture.SetPixel(x, y, Color.red);
			if (texture.GetPixel(x, y + 1) == Color.black)
			{
				x = rightX - 1;
			}
			else if (texture.GetPixel(x, y + 1) == Color.black && texture.GetPixel(rightX - 1, y + 1) == Color.black)
			{
				x = leftX + 1;
			}
			y++;
		}

		if (texture.GetPixel(x + 1, y + 1) != Color.black)
		{

		}

		//y = (int)vec.y;
		//while (texture.GetPixel(x, y) != Color.black)
		//{
		//	int rightX = x + 1;
		//	while (texture.GetPixel(rightX, y) != Color.black)
		//	{
		//		texture.SetPixel(rightX, y, Color.red);
		//		rightX++;
		//	}

		//	int leftX = x - 1;
		//	while (texture.GetPixel(leftX, y) != Color.black)
		//	{
		//		texture.SetPixel(leftX, y, Color.red);
		//		leftX--;
		//	}

		//	texture.SetPixel(x, y, Color.red);
		//	y--;
		//}
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
}
