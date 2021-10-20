using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI.Extension
{
    public class Radar:Graphic
    {
        public uint m_Sum = 6;

        public Color32 m_PropertyColor = Color.blue;

        public Vector2 m_Sizes { get; set; }
        public float m_MaxValues=10;
        public float m_OffsetValues=0;
        public List<float> m_CurValue = new List<float>();
        public bool m_IsDrawLine { get; set; }
        public Color32 m_LineColor = Color.black;
        public float m_LineSize { get; set; }
        public float m_LineSum { get; set; }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (!IsActive()) return;
            m_Sum=m_Sum>3?m_Sum:3;
            vh.Clear();
            var vertexList = new List<UIVertex>();
            ApplyGradient(ref vertexList);
            vh.AddUIVertexTriangleStream(vertexList);
        }

        public override void SetNativeSize()
        {
            base.SetNativeSize();
        }

        private void ApplyGradient(ref List<UIVertex> vertices)
        {
            PropertyInfo(ref vertices);
        }

        private void PropertyInfo(ref List<UIVertex> vertices)
        {
            List<Vector3> tempvector = GetDrawUIVertes(m_Sum,m_Sizes);

            for (int i = 0; i < m_Sum; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    UIVertex tempver = new UIVertex();
                    tempver.color = m_PropertyColor;
                    if (j == 0)
                    {
                        tempver.position = tempvector[i] * (m_CurValue[i] / m_MaxValues );
                    }
                    else if (j == 1)
                    {
                        tempver.position = Vector3.zero;
                    }
                    else if (j == 2)
                    {
                        int index = (int)((i + 1) % m_Sum);
                        tempver.position = tempvector[index] * (m_CurValue[index] ) / (m_MaxValues ) ;
                    }
                    vertices.Add(tempver);
                }
            }
        }

        private List<Vector3> GetDrawUIVertes(uint count, Vector2 r)
        {
            List<Vector3> tempvec = new List<Vector3>();
            for (int i = 0; i < count; i++)
            {
                float temp_x = Mathf.Sin((360f*i/count)*Mathf.Deg2Rad) * r.x;
                float temp_y = Mathf.Cos((360f*i/count)*Mathf.Deg2Rad) * r.y;
                tempvec.Add(new Vector3(temp_x,temp_y));
            }

            return tempvec;
        }

    }
}