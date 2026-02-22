Shaders Improved / Added:

Map:
 	The map gameobject is a new addition to the game and uses a hologram shader to be seen in the actual game. 
  Since the map is very dark and one of the very few light sources you have while playing the game is the flashlight, 
  we had to make other ways for objects to be visible. Using the hologram shader the player can spot it based on how we changed its transparency. 
  Giving it a light blue color made it glow in the dark despite the dark environment. 
  In the last submission we used the hologram shader for the cryo pods in the crew quarters room but we discovered that there wasn’t much 
  of a purpose to this as the cryo pods don’t have any function other than environmental design. Using the hologram shader for the map we can effectively 
  communicate to the player that they can see the object and collect it.

  <img width="509" height="254" alt="image" src="https://github.com/user-attachments/assets/ee06eb0e-72af-4c7b-a72f-0e948fb93439" />

  
Taser Rod:
	The Taser rod uses the Rim lighting, simple diffuse,diffuse, ambient and specular shaders from before. However some changes were made in 
  the inspector to make it look slightly better than the last time. This shader lets us switch (“toggle”) between different types of lighting—Diffuse 
  (Lambert), Ambient, Specular, Normal/Bump mapping, and Rim lighting—using checkboxes in Unity. 

  <img width="778" height="157" alt="image" src="https://github.com/user-attachments/assets/41117fb8-ea2a-4482-837c-f3fad7c28c35" />

_BaseColor is a solid color we can tint our object with. 
_MainTex is the main texture (albedo) that defines how the surface looks (like metal, plastic, etc.). We multiply them together later, so changing the base color 
slightly brightens, darkens, or tints the texture.

<img width="755" height="160" alt="image" src="https://github.com/user-attachments/assets/deee453e-ba02-4874-a262-9a4ddd71db6a" />

_SpecColor controls what color our highlight will be (white = clean light, yellow = warm,
etc.). 
_Shininess controls how tight or soft the highlight is. Higher value → smaller and sharper highlight (like polished metal). Lower value → bigger, blurry highlight (like plastic).

<img width="724" height="111" alt="image" src="https://github.com/user-attachments/assets/a930fc59-7885-46b7-bca8-b3c0d96fb295" />

These control our normal map (bump texture).
● _myBump stores the texture where the RGB colors encode fake depth information.
● _mySlider lets us scale the strength of that bump (how deep the dents look).
It doesn’t add real geometry—it just bends how light interacts with the surface, making it
look 3D.

<img width="692" height="120" alt="image" src="https://github.com/user-attachments/assets/5d3b8a74-9db1-4449-b7c0-6285eb145b68" />

These control how strong the glowing outline is around our object’s edges.
● _RimPower decides how soft or sharp that edge glow is.
● _RimIntensity multiplies the brightness of that glow

<img width="692" height="177" alt="image" src="https://github.com/user-attachments/assets/6ab358cb-043e-4fdb-8883-78ba48c5a727" />

Each toggle acts like a checkbox in the Inspector:
● We can switch each lighting effect on or off without touching the code.
That’s how we demonstrate “only ambient,” “only specular,” etc., which the Part 2 rubric
asks for.

<img width="703" height="175" alt="image" src="https://github.com/user-attachments/assets/af8c6c87-6f3b-41d4-9e3a-9f1d21aaed41" />

These are per-vertex data from our 3D model in Object space—before it’s transformed into the
world.
● positionOS: the vertex position.
● normalOS: the vertex normal direction.
● uv: texture coordinates.
● tangentOS: used for bump mapping—tells how the texture is oriented on the surface

<img width="726" height="196" alt="image" src="https://github.com/user-attachments/assets/3026b62e-6ece-4496-8827-5d34126661eb" />

These are what our vertex shader sends to the fragment shader.
● positionHCS: final screen position (so GPU knows where to draw the pixel).
● normalWS, tangentWS, bitangentWS: all converted to World Space.
● uv: passed through for texture lookup.
● viewDirWS: direction from the pixel to the camera, used for specular and rim.

<img width="724" height="433" alt="image" src="https://github.com/user-attachments/assets/5ea2bf45-894c-4e04-a5c2-f514fdf22c94" />

● viewDirWS: direction from the pixel to the camera, used for specular and rim.
Vertex Shader — vert
● Converts our model’s vertex position from Object Space → Clip Space, so it appears
on screen.
● Converts our normals and tangents into World Space, since all lighting happens there.
● Uses the cross product to build the bitangent, forming the complete TBN matrix.
That TBN is later used to transform our normal map data correctly.
● Converts the vertex position into world coordinates.
● Finds the vector from this point to the camera, which is needed for calculating
reflections and rim lighting.

So at this stage, every vertex now knows:
● Which way it faces (normalWS)
● Where the camera is
● How to sample the texture later
Then Unity interpolates all that between vertices, so every pixel in between also gets its
own values.

Fragment Shader — frag
This runs for every pixel drawn on the screen.

Step 1: Base color
<img width="761" height="74" alt="image" src="https://github.com/user-attachments/assets/75d81925-017e-4abe-b5ff-cc4927be626a" />



Reads the color from our texture and multiplies it with our tint color.
So if we want the same texture in red or blue, we just change _BaseColor. This is our starting color before lighting is added.

Step 2: Choose which normal to use
<img width="630" height="346" alt="image" src="https://github.com/user-attachments/assets/20be5665-e7f4-4750-9976-170353c00e48" />


When the checkbox for “Enable bhong” (_UseBhong) is ON:

It takes our bump texture, applies Unity’s tiling and offset, and then samples it.

UnpackNormal() converts that color into a normal vector (since normal maps store direction data in RGB).

It multiplies the X and Y values by our _mySlider to control bump intensity.

Then it builds a small 3×3 TBN matrix (Tangent, Bitangent, Normal) and uses it to rotate our bump normal from tangent space to world space.


When it’s OFF, the shader just uses the mesh’s actual normal (flat surface). This step makes our materials feel detailed without extra geometry.


Step 3: Get the scene’s main light
<img width="772" height="65" alt="image" src="https://github.com/user-attachments/assets/bb57d9cf-5eb6-4617-aa40-ea7d99695ce8" />



Unity provides the main light in our scene (the directional light, like the sun or a headlamp).
.direction gives the vector pointing from the light toward the surface.
normalize() makes sure that vector has a length of 1.
This is important because lighting math depends on direction, not distance — longer vectors would make the dot product too bright or dark.
we use this direction to figure out how much light actually hits our surface.



Step 4: Compute N·L (Lambert’s Diffuse)
<img width="759" height="72" alt="image" src="https://github.com/user-attachments/assets/756d9056-618b-4d75-8378-37fde577175c" />



The dot product between our surface normal and the light direction measures how directly the surface faces the light.

If it faces straight at the light → dot ≈ 1 → fully lit.

If it’s sideways → dot ≈ 0 → dim edge.

If it faces away → dot < 0 → no light.

saturate() clamps that value to between 0 and 1 so it doesn’t go negative — we don’t want backfaces glowing.


This is what creates realistic light falloff across a curved surface — one side bright, one side dark.
That’s the Lambert Diffuse term.


Step 5: Calculate each lighting component
<img width="788" height="73" alt="image" src="https://github.com/user-attachments/assets/b3d49ed8-312e-4cd7-a0f3-b6276d1307a0" />




Takes our base surface color and multiplies it by how much light hits the surface.

This gives the classic smooth gradient lighting — bright where the light hits directly, dark where it’s angled.


Only Diffuse On
<img width="738" height="440" alt="image" src="https://github.com/user-attachments/assets/4a38c163-76a7-4ddc-b6f9-a5c9d07bd2b6" />




Ambient
<img width="776" height="63" alt="image" src="https://github.com/user-attachments/assets/191b921a-be0f-4c4c-8e3e-9bc9162e2cdd" />



SampleSH(normalWS) uses Spherical Harmonics (Unity’s built-in ambient light probe). It estimates the soft light that bounces around the scene.

Multiplying by base tints that ambient light with our texture color.
This ensures even areas facing away from the light aren’t pitch black — they still get soft global illumination.
Only Ambient On
<img width="571" height="351" alt="image" src="https://github.com/user-attachments/assets/b52cb587-5ca0-4e55-ab53-1025697cb220" />



Specular
<img width="776" height="119" alt="image" src="https://github.com/user-attachments/assets/f596fd8b-b175-4eea-b8ae-d2e123e71738" />


reflect() calculates the mirror reflection direction from the light.
viewDir is the direction toward the camera.
The dot product between those tells how close our camera is to that reflection.
When aligned perfectly → bright spot.
When off-angle → fades out.
Raising that to _Shininess makes the highlight sharper (smaller bright area).
Multiplying by _SpecColor tints that highlight.
This creates the shiny spot we see on metals, glass, or polished plastic.

Only specular on
<img width="674" height="412" alt="image" src="https://github.com/user-attachments/assets/80baf7cf-beb0-4fad-b269-d7bccdda2748" />



Rim Lighting
<img width="752" height="102" alt="image" src="https://github.com/user-attachments/assets/117b1d12-07f5-42f3-aa53-e96b3cba19e6" />



Rim lighting makes edges glow where the surface faces away from the camera.
The dot product here checks how perpendicular the view is to the surface.
When facing straight → dot ≈ 1 → little or no rim.
When at a steep angle → dot ≈ 0 → strong rim.

Subtracting from 1 inverts it so edges glow.
pow(rimFactor, _RimPower) controls softness; _RimIntensity controls brightness. we use _SpecColor to color that rim—matching the material’s highlight color.
This helps objects stand out from dark surroundings (for example, items on the floor). Rim effect On vs Off

<img width="434" height="267" alt="image" src="https://github.com/user-attachments/assets/7ec6d005-8e81-4def-a0bd-ba6ed3cd2f15" />

<img width="499" height="286" alt="image" src="https://github.com/user-attachments/assets/6596aed0-cb11-4c90-bbf5-02ceb6510e3a" />


Step 6: Add them up depending on toggles
<img width="791" height="143" alt="image" src="https://github.com/user-attachments/assets/0c851ae0-6c17-402e-b0de-d522310878bc" />



Each toggle simply checks if it’s turned on (>0.5) and adds that part to the final color.

This means we can see each lighting component separately or combined.
The alpha (transparency) is set to 1.0 since this shader is opaque.


Everything On
<img width="296" height="171" alt="image" src="https://github.com/user-attachments/assets/fd10207a-dcb9-4305-828e-c9fc3788bc8a" />

Doors:
	The doors for the game still use flat shaders but they have been almost completely changed compared to the last time. There is now a metal texture on 
  the door combined with the flat shader to give it a much cleaner look. The door is also brighter a lot more than before as it was harder to spot when playing the game. 
  Now if players want to leave a room, the door is much more visible to interact with. Another modification to this was enhancing the edges of the flat shader slightly to give it a sharper look than before. 
<img width="727" height="253" alt="image" src="https://github.com/user-attachments/assets/b22cf9fe-0c17-4a5b-9bae-f350cdee073d" />

<img width="677" height="374" alt="image" src="https://github.com/user-attachments/assets/7d285ce7-ed14-42a9-b08a-9b781d2a74ef" />

<img width="590" height="304" alt="image" src="https://github.com/user-attachments/assets/ff2b5427-9176-4c94-9919-71cd85fa41d5" />

<img width="291" height="277" alt="image" src="https://github.com/user-attachments/assets/93270ce5-ed58-463e-81a2-be89fd027886" />









Windows:
	The windows in our game use multiple shaders combined to give it a frosted look effect. Firstly, we used the regular glass shader taught within the lecture but
  modified it to look more transparent. Being able to change the transparency, we were able to achieve a better glass look. However the next step was to make the player 
  be able to see through the glass so we decided to use stencil shaders to do this. We placed stencil shaders on some of the walls in the game so that it would have a see 
  through effect in the game. Next we placed another stencil front shader behind the glass to make the glass texture show while being able to look through the walls mimicking a frosted window.
<img width="787" height="251" alt="image" src="https://github.com/user-attachments/assets/bd72684c-89ec-461f-97bf-f0cd28fe451f" />

<img width="887" height="506" alt="image" src="https://github.com/user-attachments/assets/f9d2200a-575b-4a01-a712-6de0447a5157" />

<img width="852" height="397" alt="image" src="https://github.com/user-attachments/assets/183d24d7-3bd6-4f86-800a-5e6c869f4d0c" />

<img width="604" height="306" alt="image" src="https://github.com/user-attachments/assets/223ecda3-5b8c-41b2-87f5-b59961b79045" />








