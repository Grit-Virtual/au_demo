using UnityEngine;
using Unity.IO.Compression;
using System.IO;
using System;
using System.Collections.Generic;

public class ObjectMesh
{
	public Vector3[] vertices;
	public int[] triangles;
	public Color32[] colors;
	public int[] colorIndexes;
	public bool loaded = false;

	public ObjectMesh (Vector3[] vertices, int[] triangles, Color32[] colors, int[] indexes, bool log)
	{
		this.vertices = vertices;
		this.triangles = triangles;
		this.colors = colors;
		this.colorIndexes = indexes;

		if (log) {
			for (int i = 0; i < triangles.Length; i += 3) {
				Debug.Log (i);
				Debug.Log (vertices [triangles [i]]);
				Debug.Log (vertices [triangles [i + 1]]);
				Debug.Log (vertices [triangles [i + 2]]);
			}
		}
	}

	public static void Decode(ProjectObject obj, byte[] data, List<int> fragmentRanges, bool log) {
		if (obj.meshes == null) {
			obj.meshes = new ObjectMesh[fragmentRanges.Count / 2];
		}
		for (int k = 0; k < obj.lods.Count; k++) {
			int lod = getLod (obj.lods.Count, k, obj.meshes.Length);
			if (obj.meshes [lod] != null)
				continue;

			var input = new MemoryStream (data, fragmentRanges [lod * 2], fragmentRanges [lod * 2 + 1] - fragmentRanges [lod * 2] + 1);
			var stream = new GZipStream(input, CompressionMode.Decompress);
			byte[] buffer = new byte[8192];
			int read;
			MemoryStream output = new MemoryStream ();
			while ((read = stream.Read (buffer, 0, buffer.Length)) > 0) {
				output.Write (buffer, 0, read);
			}

			int i = 0;
			byte[] d = output.ToArray ();

			int fragments = readInt (d, ref i);
			for (int j = 0; j < fragments; j++) {
				int size = readInt (d, ref i);
				int[] sizes = getInts (d, ref i, size / 4);
				int version = sizes.Length > 4 ? sizes [4] : 0;

				float[] verts = getFloats (d, ref i, sizes [0] / 4);
				int[] triangles = version == 0 ? getShorts (d, ref i, sizes [1] / 2) : getInts (d, ref i, sizes [1] / 4);
				float[] colors = getFloats (d, ref i, sizes [2] / 4);
				int[] copies = version == 0 ? getShorts (d, ref i, sizes [3] / 2) : getInts (d, ref i, sizes [3] / 4);
				int[] materials = sizes.Length > 5 ? getShorts (d, ref i, sizes [5] / 2) : new int[] { triangles.Length };

				obj.meshes [lod] = new ObjectMesh (asVector3 (verts, copies), triangles, getColors (colors), getColorIndexes (materials, verts.Length / 3, copies), log);
			}
		}
		// return new ObjectMesh[] { Combine(meshes) };
	}

	public static int getLod(int lodCount, int i, int meshCount) {
		return meshCount - (lodCount - i <= meshCount ? lodCount - i : 1);
	}

	public static ObjectMesh Combine(ObjectMesh[] meshes) {
		int vertCount = 0;
		// int uvCount = 0;
		int triangleCount = 0;
		int lineCount = 0;
		int colorCount = 0;

		Color color = Color.white;
		foreach (ObjectMesh mesh in meshes) {
			vertCount += mesh.vertices.Length;
			triangleCount += mesh.triangles.Length;
			colorCount += mesh.colors.Length;
		}

		Vector3[] verts = new Vector3[vertCount];
		int[] triangles = new int[triangleCount];
		Color32[] colors = new Color32[colorCount];
		int[] colorIndexes = new int[vertCount];

		int vertPos = 0;
		int trianglePos = 0;
		int colorPos = 0;

		int vertOffset = 0;
		foreach (ObjectMesh mesh in meshes) {
			Array.Copy (mesh.vertices, 0, verts, vertPos, mesh.vertices.Length);
			Array.Copy (mesh.colors, 0, colors, colorPos, mesh.colors.Length);

			for (int i = 0; i < mesh.triangles.Length; i++) {
				triangles [trianglePos + i] = vertOffset + mesh.triangles [i];
			}

			for (int i = 0; i < mesh.colorIndexes.Length; i++) {
				colorIndexes [vertPos + i] = colorPos + mesh.colorIndexes [i];
			}

			vertOffset += mesh.vertices.Length / 3;
			vertPos += mesh.vertices.Length;
			colorPos += mesh.colors.Length;
			trianglePos += mesh.triangles.Length;
		}

		return new ObjectMesh (verts, triangles, colors, colorIndexes, false);
	}

	private static int[] getColorIndexes(int[] materialCounts, int vertCount, int[] copies) {
		int[] indexes = new int[vertCount + copies.Length];
		int offset = 0;
		for (int i = 0; i < materialCounts.Length; i++) {
			for (int j = 0; j < materialCounts [i]; j++) {
				indexes [offset++] = i;
			}
		}
		for (int i = 0; i < copies.Length; i++) {
			int count = 0;
			for (int j = 0; j < materialCounts.Length; j++) {
				count += materialCounts [j];
				if (copies [i] < count) {
					indexes [offset++] = j;
					break;
				}
			}
		}
		return indexes;
	}

	private static Color32[] getColors(float[] colors) {
		Color32[] list = new Color32[colors.Length / 4];
		for (int i = 0; i < colors.Length; i += 4) {
			list [i / 4] = new Color (colors [i], colors [i + 1], colors [i + 2], colors [i + 3]);
		}
		return list;
	}

	private static Color asColor(float[] colors) {
		return new Color (
			colors.Length > 0 ? colors [0] : 1.0f,
			colors.Length > 1 ? colors [1] : 1.0f,
			colors.Length > 2 ? colors [2] : 1.0f,
			colors.Length > 3 ? colors [3] : 1.0f);
	}

	private static Vector3[] asVector3(float[] list, int[] copies) {
		int baseLength = list.Length / 3;
		Vector3[] vectors = new Vector3[baseLength + copies.Length];
		for (int i = 0; i < list.Length; i += 3) {
			vectors[i / 3] = new Vector3(list[i], list[i + 1], list[i + 2]);
		}
		for (int i = 0; i < copies.Length; i++) {
			vectors [baseLength + i] = vectors [copies [i]];
		}
		return vectors;
	}

	private static Vector2[] asVector2(float[] list) {
		Vector2[] vectors = new Vector2[list.Length / 2];
		for (int i = 0; i < list.Length; i += 2) {
			vectors[i / 2] = new Vector2(list[i], list[i + 1]);
		}
		return vectors;
	}

	private static float[] getFloats(byte[] data, ref int i, int length) {
		float[] result = new float[length];
		for (int j = 0; j < length; j++) {
			result [j] = readFloat (data, ref i);
		}
		return result;
	}

	private static int[] getInts(byte[] data, ref int i, int length) {
		int[] result = new int[length];
		for (int j = 0; j < length; j++) {
			result [j] = readInt (data, ref i);
		}
		return result;
	}

	private static int[] getShorts(byte[] data, ref int i, int length) {
		int[] result = new int[length];
		for (int j = 0; j < length; j++) {
			result [j] = readShort (data, ref i);
		}
		return result;
	}

	private static int readInt(byte[] data, ref int i) {
		if (BitConverter.IsLittleEndian)
			Array.Reverse (data, i, 4);
		int value = BitConverter.ToInt32 (data, i);
		i += 4;
		return value;
	}

	private static float readFloat(byte[] data, ref int i) {
		if (BitConverter.IsLittleEndian)
			Array.Reverse (data, i, 4);
		float value = BitConverter.ToSingle (data, i);
		i += 4;
		return value;
	}

	private static int readShort(byte[] data, ref int i) {
		if (BitConverter.IsLittleEndian)
			Array.Reverse (data, i, 2);
		int value = BitConverter.ToUInt16 (data, i);
		i += 2;
		return value;
	}
}
