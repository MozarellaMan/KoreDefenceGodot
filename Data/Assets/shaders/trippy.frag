
uniform float iTime;
uniform sampler2D currentTexture;
uniform vec3 iResolution;

void main(){
  vec2 fragCoord = gl_TexCoord[0].xy;
  vec2 uv = fragCoord.xy;

  	float amount = 0.0;

  	amount = (1.0 + sin(iTime*1.0)) * 0.2;
  	amount *= 1.0 + sin(iTime*2.0) * 0.2;
  	amount *= 1.0 + sin(iTime*3.0) * 0.2;
  	amount *= 1.0 + sin(iTime*6.0) * 0.2;
  	amount = pow(amount, 3.0);

  	amount *= 0.05;

      vec3 col;
      col.r = texture2D( currentTexture, vec2(uv.x+amount,uv.y) ).r;
      col.g = texture2D( currentTexture, uv ).g;
      col.b = texture2D( currentTexture, vec2(uv.x-amount,uv.y) ).b;

  	col *= (1.0 - amount * 0.5);

      gl_FragColor = vec4(col,1.0);
}