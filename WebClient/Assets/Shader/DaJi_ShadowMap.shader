// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/DaJi_ShadowMap" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MaskTex ("Mask (R,G,B)", 2D) = "white" {}
		_SpecColor ("Spec Color", Color) = (0,0,0,0)
		_SpecPower ("Spec Power", Range(1,128)) = 15
		_SpecMultiplier ("Spec Multiplier", Float) = 1
		_RampMap ("Ramp Map", 2D) = "white" {}
		_AmbientColor ("Ambient", Color) = (0.2,0.2,0.2,0)
		_ShadowColor ("Shadow Color", Color) = (0,0,0,0)
		_LightTex ("轮廓光 (RGB)", 2D) = "white" {}
		_NormalTex ("Normal", 2D) = "bump" {}
		_NoiseTex ("Noise(RGB)", 2D) = "white" {}
		_Scroll2X ("Noise speed X", Float) = 1
		_Scroll2Y ("Noise speed Y", Float) = 0
		_NoiseColor ("Noise Color", Color) = (1,1,1,1)
		_MMultiplier ("Layer Multiplier", Float) = 2
		_TransparentColor("TransparentColor", Color) = (0.32,0.35,0.31,0.41)
	}

	SubShader {
		Tags { "RenderType"="Opaque" "Queue" = "Geometry+100" /*"LightMode" = "ForwardBase"*/ }
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
				//#pragma multi_compile_fwdbase
				
				#include "UnityCG.cginc"
				//#include "AutoLight.cginc"

				struct appdata_t {
					half4 vertex : POSITION;
					half3 normal : NORMAL;
					half4 tangent : TANGENT;
					half2 texcoord : TEXCOORD0;
				};

				struct v2f {
					half4 pos : SV_POSITION;
					half2 texcoord : TEXCOORD0;
					half2 noiseTexcoord : TEXCOORD1;
					half3 normal : TEXCOORD2;
					half3 viewDirT : TEXCOORD3;
					half4 lightDirT : TEXCOORD4;

					//LIGHTING_COORDS(5, 6)
				};

				sampler2D _MainTex;
				sampler2D _NoiseTex;
				sampler2D _LightTex;
				sampler2D _MaskTex;
				sampler2D _Reflection;
				sampler2D _RampMap;
				half4 _MainTex_ST;
				half4 _NoiseTex_ST;
				fixed4 _NoiseColor;
				fixed4 _SpecColor;
				fixed4 _AmbientColor;
				half _MMultiplier;
				half _SpecMultiplier;
				half _SpecPower;
				half _ReflectionLV;
				half _Scroll2X;
				half _Scroll2Y;
				
				v2f vert (appdata_t v) {
					fixed3 _LightDir = fixed3(-1.0, -1.0, 1.0);

					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.normal = mul(UNITY_MATRIX_MV, half4(v.normal, 0.0f)).xyz;
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.noiseTexcoord = TRANSFORM_TEX(v.texcoord, _NoiseTex) + frac(half2(_Scroll2X, _Scroll2Y) * _Time.x);

					TANGENT_SPACE_ROTATION;
					o.viewDirT = mul(rotation, ObjSpaceViewDir(v.vertex));
					o.lightDirT.xyz = mul(rotation, mul((half3x3)unity_WorldToObject, -_LightDir).xyz);

					//float3 posW = mul(_Object2World, v.vertex).xyz;
					//float3 normalW = normalize(mul(_Object2World, float4(v.normal, 0.0)).xyz);
					//float3 posToDirW = WorldSpaceLightDir(v.vertex);//normalize(_WorldSpaceLightPos0.xyz - posW);
					//o.lightDirT.w = saturate(dot(normalW, posToDirW));

					//TRANSFER_VERTEX_TO_FRAGMENT(o);

					return o;
				}
				
				fixed4 frag (v2f i) : SV_Target {
					half3 normal = normalize(i.normal);
					half3 viewDirT = normalize(i.viewDirT);
					half3 lightDirT = normalize(i.lightDirT.xyz);

					fixed4 diffuseCol = tex2D(_MainTex, i.texcoord);
					fixed4 maskCol = tex2D(_MaskTex, i.texcoord);

					fixed3 noiseCol = tex2D(_NoiseTex, i.noiseTexcoord).xyz;
					noiseCol = noiseCol * diffuseCol.xyz * _NoiseColor.xyz;
					noiseCol = noiseCol * (maskCol.y * _MMultiplier);

					half2 normalTexcoord = normal.xy * 0.5 + 0.5;
					fixed4 reflectionCol = tex2D(_Reflection, normalTexcoord);
					fixed3 lightCol = (diffuseCol.xyz * (tex2D(_LightTex, normalTexcoord) * 2.0).xyz) + diffuseCol.xyz + noiseCol;

					half2 rampTexcoord = half2(lightDirT.z * 0.5 + 0.5, 0.5);
					fixed3 rampCol = tex2D(_RampMap, rampTexcoord).xyz;

					half spec = max(0.0, normalize(viewDirT + lightDirT).z);
					fixed3 col = (_SpecColor * ((pow(spec, _SpecPower) * maskCol.x) * _SpecMultiplier) * 2.0) + ((rampCol + _AmbientColor) * lightCol);
					//fixed3 col = lerp(lightCol, (lightCol * pow(reflectionCol.xyz, float3(_RimPower, _RimPower, _RimPower))) * _ReflectionLV, maskCol.xxx);
					//col = col + noiseCol;

					//float attenuation = LIGHT_ATTENUATION(i) * i.lightDirT.w;

					return fixed4(col, diffuseCol.a);// * attenuation;
				}
			ENDCG
		}
	}

	Fallback "VertexLit"
}
