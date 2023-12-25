Shader "Studio1/SimulateLit"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
		_MaskTex ("Mask (R,G,B)", 2D) = "white" {}
        _AmbientColor ("Ambient", Color) = (0,0,0,0)

        _RampMap ("Ramp Map 明暗采样纹理", 2D) = "gray" {}
		_RimTex("Rim (RGB) 轮廓光", 2D) = "gray" {}
		_ColorAlpha ("Alpha", Range(0,1)) = 1
        
        // 流光属性
        [Header(Noise Flash Properties(Mask.G))] 
        _NoiseTex ("Noise(RGB)", 2D) = "black" {}
		_NoiseSpeedX ("Noise speed X", Float) = 1
		_NoiseSpeedY ("Noise speed Y", Float) = 0
		_NoiseColor ("Noise Color", Color) = (1,1,1,1)
		_NoiseMultiplier ("Noise Multiplier", Float) = 1

        [Space][KeywordEnum(Norm, Spec, Refl)] _MODE("Mode", Float) = 0
                
        [Header(Specular Properties(Mask.R))] 
        _SpecColor ("Spec Color", Color) = (0,0,0,0)
		_SpecPower ("Spec Power", Range(1,128)) = 15
		_SpecMultiplier ("Spec Multiplier", Float) = 1
        
        [Header(Reflection Properties(Mask.R))]
        _Reflection ("Refl (RGB)", 2D) = "gray" {}
        _ReflPower ("Reflection Power", Range(1,3)) = 1
		_ReflMultiplier ("Reflection Multiplier", Float) = 2

		[Toggle]_ShowGray("ShowGray?", float) = 0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 100
		/*Pass
		{
			Name "ZTEST"
			ZWrite On
			ColorMask 0

		}*/
		
		//ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "BASE"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal
            #pragma multi_compile _MODE_NORM _MODE_SPEC _MODE_REFL
            #pragma multi_compile __ _SHOWGRAY_ON
            #include "UnityCG.cginc"

            struct appdata
            {
                half4 vertex : POSITION;
				half3 normal : NORMAL;
                half4 tangent : TANGENT;
                half2 uv : TEXCOORD0;
            };

            struct v2f
            {
                half4 vertex : SV_POSITION;
                half2 uv : TEXCOORD0;                
                half2 uvNoise : TEXCOORD1;
				half3 normal : TEXCOORD2;
				half3 viewDir : TEXCOORD3;
				half3 lightDir : TEXCOORD4;
            };

            sampler2D _MainTex, _MaskTex, _NoiseTex;
            half4 _MainTex_ST, _NoiseTex_ST;

            sampler2D _RampMap, _RimTex;
			fixed _ColorAlpha;

            half _NoiseMultiplier, _NoiseSpeedX, _NoiseSpeedY;
            fixed4 _AmbientColor, _NoiseColor;
            
            #ifdef _MODE_SPEC
                half _SpecPower, _SpecMultiplier;
                fixed4 _SpecColor;
            #endif

            #if _MODE_REFL
                sampler2D _Reflection;
                half _ReflPower, _ReflMultiplier;
            #endif

            v2f vert (appdata v)
            {
                fixed3 _LightDir = fixed3(-1.0, -1.0, 1.0);

				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = mul(UNITY_MATRIX_MV, float4(v.normal, 0.0f)).xyz;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uvNoise = TRANSFORM_TEX(v.uv, _NoiseTex) + frac(half2(_NoiseSpeedX, _NoiseSpeedY) * _Time.x);

				TANGENT_SPACE_ROTATION;
				o.viewDir = mul(rotation, ObjSpaceViewDir(v.vertex));
				o.lightDir = mul(rotation, mul((half3x3)unity_WorldToObject, -_LightDir).xyz);

				return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 normal = normalize(i.normal);
				fixed3 viewDir = normalize(i.viewDir);
				fixed3 lightDir = normalize(i.lightDir);

                fixed4 diffuseCol = tex2D(_MainTex, i.uv);				
                fixed4 maskCol = tex2D(_MaskTex, i.uv);

                fixed3 noiseCol = tex2D(_NoiseTex, i.uvNoise).rgb;
				noiseCol = noiseCol * diffuseCol.rgb * _NoiseColor.rgb * _NoiseMultiplier * maskCol.g;

                half2 uvNormal = normal.xy * 0.5 + 0.5;
                fixed3 col = diffuseCol.rgb * tex2D(_RimTex, uvNormal).rgb * 2 + diffuseCol.rgb + noiseCol.rgb;

                half2 uvRamp = half2(lightDir.z * 0.5 + 0.5, 0.5);
				fixed3 rampCol = tex2D(_RampMap, uvRamp).rgb;

                col *= rampCol + _AmbientColor.rgb;
                #if _MODE_REFL
                    fixed4 reflCol = tex2D(_Reflection, uvNormal);
                    col = lerp(col, (col * pow(reflCol.rgb, _ReflPower)) * _ReflMultiplier, maskCol.r);
                    //col.rgb += reflCol.rgb;
                #else
                    #if _MODE_SPEC
                        half spec = max(0.0, normalize(viewDir + lightDir).z);
                        col += _SpecColor.rgb * ((pow(spec, _SpecPower) * maskCol.r) * _SpecMultiplier) * 2.0;                            
                    #endif
                #endif
				#if _SHOWGRAY_ON
					float grey = dot(col, float3(0.299, 0.587, 0.114));
					col.rgb = float3(grey, grey, grey);
				#endif
					diffuseCol.a = diffuseCol.a * _ColorAlpha;

                return fixed4(col, diffuseCol.a);
            }
            ENDCG
        }

        UsePass "Studio1/ShadowCaster/ShadowCaster"
    }
}
