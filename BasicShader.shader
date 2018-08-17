Shader "Diamond Square shader" {
	Subshader {
		Pass {
			CGPROGRAM

			// defines vertex shader as function vert
			#pragma vertex vert
			// defines frag/pixel shader as frag
			#pragma fragment frag

			uniform int size;

			struct vertexInput {
				float4 vertices : POSITION;
				float4 colour 	: COLOR;
			};

			struct vertexOutput {
				float4 vertices : POSITION;
				float4 colour 	: COLOR;
			};

			vertexOutput vert(vertexInput v) {
				vertexOutput output;

				output.vertices = UnityObjectToClipPos(v.vertices);
				output.colour = v.vertices/size;

				return output;
			}

			float4 frag (vertexOutput finput) : COLOR {
				//return finput.colour;
				return finput.colour;
			}

				
			ENDCG
		}
	}
}