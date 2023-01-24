// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/CheapMatCapLikeWithOutline"
{
	Properties
	{
		_Color("Tint Color",Color) = (1,1,1,1)
		_MatCap("MatCap (RGB)", 2D) = "white" {}
		_OutlineColor("Outline Color",Color) = (1,1,1,1)
		_Outline("Outline Width",Float) = 0.0
		[MaterialToggle] TextureTransforms("Transform Texture coords", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			name "CHEAP_OUTLINE"

			Cull Front
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			fixed4 _OutlineColor;
			float _Outline;
			
			v2f vert (appdata_base v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);

				float3 norm = mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal.xyz);

				float2 offset = TransformViewToProjection(norm.xy);

				o.vertex.xy += (offset * _Outline);

				o.color = _OutlineColor;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return i.color;
			}
			ENDCG
		}

		UsePass "Unlit/CheapMatCapLike/CHEAP_BASE"

	}
	FallBack "Diffuse"
}
