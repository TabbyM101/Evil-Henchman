using UnityEngine;
using UnityEngine.UI;

public class UIGridRenderer : Graphic
{
   public Vector2Int gridSize = new Vector2Int(1, 1);
   public float thickness = 10f;

   float width;
   float height;
   float cellWidth;
   float cellHeight;
   
   
   protected override void OnPopulateMesh(VertexHelper vh)
   {
      vh.Clear();

      width = rectTransform.rect.width;
      height = rectTransform.rect.height;

      cellWidth = width / (float)gridSize.x;
      cellHeight = height / (float)gridSize.y;

      int count = 0;

      for (int y = 0; y < gridSize.y; y++)
      {
         for (int x=0; x<gridSize.x; x++)
         {
            DrawCell(x,y,count,vh);
            count++;
         }
      }
      
      
   }

   private void DrawCell(int x, int y, int index, VertexHelper vh)
   {
      float xPos = cellWidth * x;
      float yPos = cellHeight * y;
      
      UIVertex vertex = UIVertex.simpleVert;
      vertex.color = color;

      vertex.position = new Vector3(0, 0);
      vh.AddVert(vertex);

      vertex.position = new Vector3(0, height);
      vh.AddVert(vertex);

      vertex.position = new Vector3(width, height);
      vh.AddVert(vertex);

      vertex.position = new Vector3(width, 0);
      vh.AddVert(vertex);
      
      //vh.AddTriangle(0,1,2);
      //vh.AddTriangle(2,3,0);

      float widthSqr = thickness * thickness;
      float distanceSqr = widthSqr / 2f;
      float distance = Mathf.Sqrt(distanceSqr);

      vertex.position = new Vector3(distance, distance);
      vh.AddVert(vertex);

      vertex.position = new Vector3(distance, height - distance);
      vh.AddVert(vertex);

      vertex.position = new Vector3(width - distance, height - distance);
      vh.AddVert(vertex);

      vertex.position = new Vector3(width - distance, distance);
      vh.AddVert(vertex);
      
      //left edge
      vh.AddTriangle(0,1,5);
      vh.AddTriangle(5,4,0);
      
      //top edge
      vh.AddTriangle(1,2,6);
      vh.AddTriangle(6,5,1);
      
      //right edge
      vh.AddTriangle(2,3,7);
      vh.AddTriangle(7,6,2);
      
      //bottom edge
      vh.AddTriangle(3,0,4);
      vh.AddTriangle(4,7,3);
   }
}
