Shader "Hole Shader" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGBA)", 2D) = "white" {}
	}

		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "False" "RenderType" = "Transparent" }
		LOD 200

		Pass{
		// The alpha is supplied by the projector.
		Blend OneMinusDstAlpha DstAlpha

		//This just defines the texture
		SetTexture[_MainTex]
	}
	}

		Fallback "Transparent/VertexLit"
}