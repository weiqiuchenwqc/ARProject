// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ZhouYu_ShadowMap" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MaskTex ("Mask (R,G,B)", 2D) = "white" {}
		_RimPower ("Rim Power", Range(1,3)) = 1
		_ReflectionLV ("Reflection Multiplier", Float) = 2
		_LightTex ("轮廓光 (RGB)", 2D) = "white" {}
		_Reflection ("反射 (RGB)", 2D) = "white" {}
		_NormalTex ("Normal", 2D) = "bump" {}
		_NoiseTex ("Noise(RGB)", 2D) = "white" {}
		_Scroll2X ("Noise speed X", Float) = 1
		_Scroll2Y ("Noise speed Y", Float) = 0
		_Color ("Color", Color) = (1,1,1,1)
		_MMultiplier ("Layer Multiplier", Float) = 2
		_TransparentColor("TransparentColor", Color) = (0.32,0.35,0.31,0.41)
	}

	SubShader {
		Tags { "RenderType"="Opaque" "Queue" = "Geometry+100" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			ZTest Greater

			CGPROGRAM
			#include "Lighting.cginc"
			#pragma vertex vert
			#pragma fragment frag
			uniform fixed4 _TransparentColor;
			//uniform sampler2D _MainTex;
			//uniform fixed4 _MainTex_ST;

			struct v2f
			{
				fixed4 pos : SV_POSITION;
				//fixed2 uv : TEXCOORD0;
				//fixed3 normal : normal;
				//fixed3 viewDir : TEXCOORD1;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				/*
				o.uv = v.texcoord;
				*/
				o.pos = UnityObjectToClipPos(v.vertex);
				//o.viewDir = ObjSpaceViewDir(v.vertex);
				//o.normal = v.normal;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//fixed4 diffuse = tex2D(_MainTex, TRANSFORM_TEX(i.uv, _MainTex));
				//fixed3 normal = normalize(i.normal);
				//fixed3 viewDir = normalize(i.viewDir);
				//fixed rim = dot(normal, viewDir);
				return _TransparentColor;	//_TransparentColor * rim;
			}
			ENDCG
		}

		Pass {  
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
					half2 noiseTexcoord : TEXCOORD1;
					float3 normal : TEXCOORD2;
				};

				sampler2D _MainTex;
				sampler2D _NoiseTex;
				sampler2D _LightTex;
				sampler2D _MaskTex;
				sampler2D _Reflection;
				float4 _MainTex_ST;
				float4 _NoiseTex_ST;
				float4 _Color;
				float _MMultiplier;
				float _RimPower;
				float _ReflectionLV;
				float _Scroll2X;
				float _Scroll2Y;
				
				v2f vert (appdata_t v) {
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.normal = mul(UNITY_MATRIX_MV, float4(v.normal, 0.0f)).xyz;
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.noiseTexcoord = TRANSFORM_TEX(v.texcoord, _NoiseTex) + frac(float2(_Scroll2X, _Scroll2Y) * _Time.x);
					return o;
				}
				
				fixed4 frag (v2f i) : SV_Target {
					float3 normal = normalize(i.normal);

					fixed4 diffuseCol = tex2D(_MainTex, i.texcoord);
					fixed4 maskCol = tex2D(_MaskTex, i.texcoord);

					half2 normalTexcoord = normal.xy * 0.5 + 0.5;
					fixed4 reflectionCol = tex2D(_Reflection, normalTexcoord);
					fixed3 lightCol = diffuseCol.xyz * (tex2D(_LightTex, normalTexcoord) * 2.0).xyz;

					fixed3 noiseCol = tex2D(_NoiseTex, i.noiseTexcoord).xyz;
					noiseCol = noiseCol * diffuseCol.xyz * _Color.xyz;
					noiseCol = noiseCol * (maskCol.y * _MMultiplier);

					fixed3 col = lerp(lightCol, (lightCol * pow(reflectionCol.xyz, float3(_RimPower, _RimPower, _RimPower))) * _ReflectionLV, maskCol.xxx);
					col = col + noiseCol;

					return fixed4(col, diffuseCol.a);
				}
			ENDCG
		}
	}

	Fallback "VertexLit"
}
