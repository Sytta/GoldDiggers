// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Per pixel bumped refraction.
// Uses a normal map to distort the image behind, and
// two additional textures to tint and highlight.
// ------
// Assembled by chance using a lot of freely available reference
// by Mauricio Perin aka @maperns for Aduge Studio/Zueira Digital
// game Qasir al-Wasat in a long series of try and error attempts
// during the years of 2011, 2012 ad 2015.

Shader "FX/Glass/Sublte" {
	Properties{
		_MainTex("Tint Color (RGB)", 2D) = "white" {}
	_ScndTex("Highlight Color (RGB)", 2D) = "white" {}
	_BumpAmt("Distortion", range(0,128)) = 10
		_BumpMap("Normalmap", 2D) = "bump" {}
	}

		Category{

		// This is transparent so objects behind the shader are drawn beforehand.
		Tags{ "Queue" = "Transparent" "RenderType" = "Opaque" }


		SubShader{

		// A pass to grab the scene behind the object and denerate a texture.
		// The result will be available as _GrabTexture
		GrabPass{
		Name "BASE"
		Tags{ "LightMode" = "Always" }
	}

		// The main pass takes the generated texture and uses the bumpmap
		// to distort it accordingly
		Pass{
		Name "BASE"
		Tags{ "LightMode" = "Always" }

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"

		struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord: TEXCOORD0;
	};

	struct v2f {
		float4 vertex : POSITION;
		float4 uvgrab : TEXCOORD0;
		float2 uvbump : TEXCOORD1;
		float2 uvmain : TEXCOORD2;
		float2 uvscnd : TEXCOORD3;
	};

	float _BumpAmt; //A slider to fine tune the distortion
	float4 _BumpMap_ST; //Distortion map is a grayscale texture, midgray is neutral.
	float4 _MainTex_ST; //Darkening texture for color effects and such, white is neutral.
	float4 _ScndTex_ST; //Texture used for highlights, black is neutral.

	v2f vert(appdata_t v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
#if UNITY_UV_STARTS_AT_TOP
		float scale = -1.0;
#else
		float scale = 1.0;
#endif
		o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
		o.uvgrab.zw = o.vertex.zw;
		o.uvbump = TRANSFORM_TEX(v.texcoord, _BumpMap);
		o.uvmain = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.uvscnd = TRANSFORM_TEX(v.texcoord, _ScndTex);
		return o;
	}

	sampler2D _GrabTexture;
	float4 _GrabTexture_TexelSize;
	sampler2D _BumpMap;
	sampler2D _MainTex;
	sampler2D _ScndTex;

	half4 frag(v2f i) : COLOR
	{
		// math of the distortion
		half2 bump = UnpackNormal(tex2D(_BumpMap, i.uvbump)).rg; // we could optimize this by just reading the x & y without reconstructing the Z
		float2 offset = bump * _BumpAmt * _GrabTexture_TexelSize.xy;
		i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;

		half4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
		half4 tint = tex2D(_MainTex, i.uvmain);
		half4 high = tex2D(_ScndTex, i.uvscnd);
		// Adding the higlight at 20% strentgh then doing the darkening color effect
		return (col + (high * 0.2)) / tint;
	}
		ENDCG
	}
	}

		// ------------------------------------------------------------------
		// Fallback for Old graphics cards and Unity vanilla fallback

		SubShader{
		Blend DstColor Zero
		Pass{
		Name "BASE"
		SetTexture[_MainTex]{ combine texture }
	}
	}
	}

}