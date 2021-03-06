// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Game_CameraEffect_Bloom"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BloomTex("Bloom (RGB)",2D) = "black"{}
	}

	CGINCLUDE
	#include "UnityCG.cginc"
	uniform sampler2D _MainTex;//主贴图属性
	uniform half4 _MainTex_TexelSize;
	uniform half4 _MainTex_ST;

	uniform half2 _ThresholdParams;//辉光参数
	uniform half _Spread;

	uniform sampler2D _BloomTex;//辉光图属性
	uniform half _BloomIntensity;

	struct v2fCombineBloom
	{
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;

		#if UNITY_UV_STARTS_AT_TOP
		half2 uv2 : TEXCOORD1;
		#endif
	};

	struct v2fBlurDown
	{
		float4 pos : SV_POSITION;
		half2 uv0 : TEXCOORD0;
		half4 uv12 : TEXCOORD1;
		half4 uv34 : TEXCOORD2;
	};

	struct v2fBlurUp
	{
		float4 pos : SV_POSITION;
		half4 uv12 : TEXCOORD0;
		half4 uv34 : TEXCOORD1;
		half4 uv56 : TEXCOORD2;
		half4 uv78 : TEXCOORD3;
	};

	v2fBlurDown vertBlurDown(appdata_img v)
	{
		v2fBlurDown o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv0 = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy,_MainTex_ST);
		o.uv12.xy = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + half2(1.0h,1.0h) * _MainTex_TexelSize.xy * _Spread, _MainTex_ST);
		o.uv12.zw = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + half2(-1.0h,1.0h) * _MainTex_TexelSize.xy * _Spread, _MainTex_ST);
		o.uv34.xy = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + half2(-1.0h,-1.0h) * _MainTex_TexelSize.xy * _Spread, _MainTex_ST);
		o.uv34.zw = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + half2(1.0h,-1.0h) * _MainTex_TexelSize.xy * _Spread, _MainTex_ST);
		return o;
	}

	v2fBlurUp vertBlurUp(appdata_img v)
	{
		v2fBlurUp o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv12.xy = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + half2(1.0h,1.0h) * _MainTex_TexelSize.xy * _Spread,_MainTex_ST);
		o.uv12.zw = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + half2(-1.0h,1.0h) * _MainTex_TexelSize.xy * _Spread,_MainTex_ST);
		o.uv34.xy = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + half2(-1.0h,-1.0h) * _MainTex_TexelSize.xy * _Spread,_MainTex_ST);
		o.uv34.zw = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + half2(1.0h,-1.0h) * _MainTex_TexelSize.xy * _Spread,_MainTex_ST);
		o.uv56.xy = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + half2(0.0h,2.0h) * _MainTex_TexelSize.xy * _Spread,_MainTex_ST);
		o.uv56.zw = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + half2(0.0h,-2.0h) * _MainTex_TexelSize.xy * _Spread,_MainTex_ST);
		o.uv78.xy = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + half2(2.0h,0.0h) * _MainTex_TexelSize.xy * _Spread,_MainTex_ST);
		o.uv78.zw = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + half2(-2.0h,0.0h) * _MainTex_TexelSize.xy * _Spread,_MainTex_ST);
		return o;
	}

	v2fCombineBloom vertCombineBloom(appdata_img v)
	{
		v2fCombineBloom o;

		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = UnityStereoScreenSpaceUVAdjust(v.texcoord,_MainTex_ST);
		#if UNITY_UV_STARTS_AT_TOP
		o.uv2 = o.uv;
		if(_MainTex_TexelSize.y < 0.0)
		{
			o.uv.y = 1.0 - o.uv.y;
		}
		#endif
		return o;
	}

	fixed4 fragBlurDownFirstPass(v2fBlurDown i) : SV_Target
	{
		fixed4 col0 = tex2D(_MainTex,i.uv0);
		fixed4 col1 = tex2D(_MainTex,i.uv12.xy);
		fixed4 col2 = tex2D(_MainTex,i.uv12.zw);
		fixed4 col3 = tex2D(_MainTex,i.uv34.xy);
		fixed4 col4 = tex2D(_MainTex,i.uv34.zw);

		fixed4 col = col0 + col1 * 0.25 + col2 * 0.25 + col3 * 0.25 + col4 * 0.25;

		col = col * 0.5 + _ThresholdParams.y;
		col = max(col,0.0) * _BloomIntensity;

		return col;
	}

	fixed4 fragBlurDown(v2fBlurDown i) : SV_Target
	{
		fixed4 col0 = tex2D(_MainTex,i.uv0);
		fixed4 col1 = tex2D(_MainTex,i.uv12.xy);
		fixed4 col2 = tex2D(_MainTex,i.uv12.zw);
		fixed4 col3 = tex2D(_MainTex,i.uv34.xy);
		fixed4 col4 = tex2D(_MainTex,i.uv34.zw);

		fixed4 col = col0 + col1 * 0.25 + col2 * 0.25 + col3 * 0.25 + col4 * 0.25;

		col = col * 0.5;

		return col;
	}

	#define oneTwelve 0.0833333h
	#define oneSix 0.1666666h

	fixed4 fragBlurUp(v2fBlurUp i) : SV_Target
	{
		fixed4 col1 = tex2D(_MainTex,i.uv12.xy);
		fixed4 col2 = tex2D(_MainTex,i.uv12.zw);
		fixed4 col3 = tex2D(_MainTex,i.uv34.xy);
		fixed4 col4 = tex2D(_MainTex,i.uv34.zw);
		fixed4 col5 = tex2D(_MainTex,i.uv56.xy);
		fixed4 col6 = tex2D(_MainTex,i.uv56.zw);
		fixed4 col7 = tex2D(_MainTex,i.uv78.xy);
		fixed4 col8 = tex2D(_MainTex,i.uv78.zw);

		fixed4 col = col1 * oneSix + col2 * oneSix + col3 * oneSix + col4 * oneSix +
		col5 * oneTwelve + col6 * oneTwelve + col7 * oneTwelve + col8 * oneTwelve;

		return col;
	}

	fixed4 fragCombineBloom(v2fCombineBloom i) : SV_Target
	{
		#if UNITY_UV_STARTS_AT_TOP
			fixed4 col = tex2D(_MainTex,i.uv2);
			return col + tex2D(_BloomTex,i.uv);
		#else
			fixed4 col = tex2D(_MainTex,i.uv);
			return col + tex2D(_BloomTex,i.uv);
		#endif
	}

	ENDCG

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		//处理缩放和阀值
		Pass
		{
			CGPROGRAM

			#pragma vertex vertBlurDown
			#pragma fragment fragBlurDownFirstPass

			ENDCG
		}

		//down pass
		Pass
		{
			CGPROGRAM

			#pragma vertex vertBlurDown
			#pragma fragment fragBlurDown

			ENDCG
		}

		//up pass
		Pass
		{
			CGPROGRAM

			#pragma vertex vertBlurUp
			#pragma fragment fragBlurUp

			ENDCG
		}

		//final pass
		Pass
		{
			CGPROGRAM

			#pragma vertex vertCombineBloom
			#pragma fragment fragCombineBloom

			ENDCG
		}
	}
}
