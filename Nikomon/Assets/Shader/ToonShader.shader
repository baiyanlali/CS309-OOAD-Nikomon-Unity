Shader "Custom/ToonShader" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        // 暂时用上该变量
        _MainBump ("Bump", 2D) = "bump" {}
        // 该变量主要使用来降低颜色种类的
        _Tooniness ("Tooniness", Range(0.1,20)) = 4
        
        _ColorMerge("Color Merge",Range(0.1,20))=4
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Toon
        sampler2D _MainTex;
        // 添加_Tooniness的引用
        float _Tooniness;
        float _ColorMerge;
        struct Input {
            float2 uv_MainTex;
            float uv_MainBump;
        };
        void surf (Input IN, inout SurfaceOutput o) {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Normal=UnpackNormal(tex2D(_MainTex,IN.uv_MainBump));
            o.Albedo=floor(c.rgb*_ColorMerge)/_ColorMerge;
            o.Alpha = c.a;
        }
        half4 LightingToon(SurfaceOutput s,half3 lightDir,half atten)
        {
            half4 c;
            half NdotL=dot(s.Normal,lightDir);
            NdotL=floor(NdotL*_Tooniness)/_Tooniness;
            c.rgb=s.Albedo*_LightColor0.rgb*NdotL*atten*2;
            c.a=s.Alpha;
            return c;
        }
        ENDCG
    } 
    FallBack "Diffuse"
}