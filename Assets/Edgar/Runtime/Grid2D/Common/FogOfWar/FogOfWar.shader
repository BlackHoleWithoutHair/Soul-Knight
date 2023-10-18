Shader "Edgar/FogOfWar" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_VisionTex ("Vision texture (RGB)", 2D) = "black" {}	
		_VisionTex2 ("Vision texture 2 (RGB)", 2D) = "black" {}	
		_VisionTexOffset("Vision texture offset", Vector) = (.0, .0, .0)
		_VisionTexSize("Vision texture size", Vector) = (.0, .0, .0)
		_FogColor ("Fog Color", Color) = (1,1,1,1)
		_TileGranularity("Number of sub-tiles", Float) = 2
		_FogSmoothness ("Fog value precision", Float) = 20
		_InitialFogTransparency ("Initial fog transparence", Float) = 0
	}
	SubShader {
	    Tags
        {
            "RenderType"="Transparent"
            "Queue" = "Transparent"
        }

		Pass {
			ZTest Always
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma multi_compile __ FOG_CUSTOM_MODE
			#include "UnityCG.cginc"

			uniform sampler2D _CameraDepthTexture;
			uniform float4x4 _ViewProjInv;

			uniform sampler2D _MainTex;
			uniform sampler2D _VisionTex;
			uniform sampler2D _VisionTex2;
			uniform float2 _VisionTexOffset;
			uniform float2 _VisionTexSize;
			uniform float2 _CellSize;
			uniform float4 _FogColor;
			uniform float _TileGranularity;
			uniform float _FogSmoothness;
			uniform float _InitialFogTransparency;

			float4 GetWorldPositionFromDepth( float2 uv_depth )
			{    
				float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv_depth);
				float4 H = float4(uv_depth.x*2.0-1.0, (uv_depth.y)*2.0-1.0, depth, 1.0);
				float4 D = mul(_ViewProjInv,H);
				return D/D.w;
			}

			float4 frag(v2f_img i) : COLOR {
				float4 c = tex2D(_MainTex, i.uv);			
				float4 result = c;
				float4 worldpos = GetWorldPositionFromDepth(i.uv);
				
				// Compute world position rounded to whole tiles
				float floorX = (- _VisionTexOffset.x + worldpos.x) / _CellSize.x - 1;
				float floorY = (- _VisionTexOffset.y + worldpos.y) / _CellSize.y - 1;

				#if defined(FOG_CUSTOM_MODE)
					float p = 0.5 / _TileGranularity;
					floorX = floor((floorX) * _TileGranularity) / _TileGranularity + p;
					floorY = floor((floorY) * _TileGranularity) / _TileGranularity + p;
				#endif

				float2 floorWorldpos = float2(floorX, floorY);

				// Check if the tile is covered by the Fog of War texture
				// If it is, compute the color of the tile using the texture
				if (floorX > 0 && floorY > 0 && floorWorldpos.x < _VisionTexSize.x && floorWorldpos.y < _VisionTexSize.y) {

					float4 color = tex2D(_VisionTex, float2(floorWorldpos.x / float(_VisionTexSize.x), floorWorldpos.y / float(_VisionTexSize.y)));
					float4 colorInterpolated = tex2D(_VisionTex2, float2(floorWorldpos.x / float(_VisionTexSize.x), floorWorldpos.y / float(_VisionTexSize.y)));

					float isInterpolated = color.g;

					if (isInterpolated > 0.5) {
						#if defined(FOG_CUSTOM_MODE)
							float g = ceil(colorInterpolated.r * _FogSmoothness) / _FogSmoothness;
							result.rgba = lerp(_FogColor, c.rgba, g);
						#else
							result.rgba = lerp(_FogColor, c.rgba, colorInterpolated.r);
						#endif
					} else {
						result.rgba = lerp(_FogColor, c.rgba, color.r);
					}

					// Uncomment for debugging purposes
					// result.rgba = lerp(colorInterpolated.rgba, c.rgba, 0.2);
				// Otherwise, show the tile as completely hidden in the fog
				} else {
					result.rgba = lerp(_FogColor, c.rgba, _InitialFogTransparency);
				}

				return result;
			}
			ENDCG
		}
	}
}