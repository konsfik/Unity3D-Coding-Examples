# Unity3D-Coding-Examples
This repository includes sample projects made with Unity, with the purpose of creating a collection of reusable, manageable pieces of code that can be used as toolsets on larger-scale applications.  
## Table of Contents:
1. [Procedural Water Surface](#1-procedural-water-surface)  
1.1. [Description](#11-description)   
1.2. [Screenshots](#12-screenshots)   
1.3. [Further Links](#13-further-links)  
2. [Procedural Landscape](#2-procedural-landscape)  
2.1. [Description](#21-description)  
2.2. [Screenshots](#22-screenshots)  
2.3. [Future Plans](#23-future-plans)  
2.4. [Further Links](#24-further-links)  
3. [Procedural Audio](#3-procedural-audio)  
3.1. [Description](#31-description)  
3.2. [Screenshots](#32-screenshots)  
3.3. [Further Links](#33-further-links)  

## 1. Procedural Water Surface
project - specific - folder: https://github.com/konsfik/Unity3D-Coding-Examples/tree/master/1-Procedural-Water-Surface  
### 1.1. Description:
This project is a case study on **"how you can procedurally generate Waves on a Custom Mesh-Surface by using C# code in Unity3D"**. Keep in mind that this is **not** a fluid-simulation, but rather a mathematical approximation of how a fluid-surface might "look like". Meaning that there is not an underlying physical model that drives the simulation, but only some mathematical functions (sinusoidal) that shape the surface's form.  
### 1.2. Screenshots:  
<table style="width:100%">
  <tr>
    <th>Images</th>
    <th>Descriptions</th>
  </tr>
  <tr>
    <td><img src="/screenshots/1-Procedural-Water-Surface-Screenshots/konsfik-procedural-water-surface-unity3d-project-editor-screenshot.jpg" width="400" title="Procedural Water Surface - editor screenshot" /></td>
    <td>Procedural Water Surface - editor screenshot</td> 
  </tr>
  <tr>
    <td><img src="/screenshots/1-Procedural-Water-Surface-Screenshots/konsfik-procedural-water-surface-unity3d-screenshot-1.jpg" width="400" title="Procedural Water Surface - application screenshot" />  </td>
    <td>Procedural Water Surface - application screenshot</td> 
  </tr>
</table>  

### 1.3. Further links:  
demo-video: https://www.youtube.com/watch?v=xy8MhL6WoSw  
more info about this example: http://www.konsfik.com/procedural-water-surface-made-in-unity3d/  
  
## 2. Procedural Landscape  
project - specific - folder: https://github.com/konsfik/Unity3D-Coding-Examples/tree/master/2-Procedural-Landscape  
### 2.1. Description:  
This project is a case-study on **"how you can procedurally generate a landscape - surface in Unity, using c# code"**. It allows you to make changes and see a preview of the generated mesh in Unity editor's edit-mode.  
The aim is to also be able to save the generated mesh as a prefab, but I have not implemented this functionality yet. More details coming soon.  

### 2.2. Screenshots:  
<table style="width:100%;">
  <tr>
    <th>Images</th>
    <th>Descriptions</th>
  </tr>
  <tr>
    <td><img src="/screenshots/2-Procedural-Landscape-Screenshots/konsfik-procedural-landscape-editor-screenshot-1.jpg" width="400" title="editor screenshot" /> </td>
    <td>Procedural Landscape - editor screenshot</td> 
  </tr>
  <tr>
    <td><img src="/screenshots/2-Procedural-Landscape-Screenshots/konsfik-procedural-landscape-editor-screenshot-controls.jpg" width="400" title="editor controls" />  </td>
    <td>Procedural Landscape - editor controls</td> 
  </tr>
  <tr>
    <td><img src="/screenshots/2-Procedural-Landscape-Screenshots/konsfik-procedural-landscape-animated-play-mode-screenshot-1.jpg" width="400" title="animated play mode screenshot" /></td>
    <td>Procedural Landscape - animated mode</td> 
  </tr>
</table>

### 2.3. Future plans:
This project is currently functional, however it will need some expansion (in its functionality) inorder to become an actual level-design tool.  

**Here is a list of the functionality that has already been implemented:**  
- [x] Apply Perlin Noise on a custom grid - mesh, creating an artificial - landscape - geometry.  
- [x] Use the data (height - variation) of the created landscape to make a custom gradient material.  
- [x] Expose variables to inspector and make sure that it the geometry and material update correctly while in edit mode.  
- [x] Create a demo-mode, in which the Landscape updates by itself, in an ever-changing manner (just for the fun of it).  

**And here is the current 'TO - DO - LIST':**  
- [ ] The algorithm adds a collider on the procedural mesh, so that you can use it as a playeble level.  
- [ ] The algorithm allows you to save the mesh as a prefab, so that you cn create a collection of pre-made Landscapes.  
- [ ] The algorithm allows you to export the mesh in a 3D-file format (such as .obj or .dxf) so that you can edit it with another application.  
- [ ] The Procedural Mesh's geometry expands in real time - as you explore it, creating a never-ending world.  
- [ ] The Landscape is populated with extra elements (such as rocks and vegetation) thaus creating a richer environment.  

I am not sure if / when I will fulfill the list's tasks. For the time being I am more interested in populating this repository with a variety of examples. However, I am writing these down for future reference.
 
### 2.4. Further links:  
demo-video: https://www.youtube.com/watch?v=mXWjAjq2yYQ  
more info about this example: http://www.konsfik.com/procedural-landscape-made-in-unity3d/  

## 3. Procedural Audio  
Project - specific - folder: https://github.com/konsfik/Unity3D-Coding-Examples/tree/master/3-Procedural-Audio  
### 3.1 Description:
This project is a case-study on **"how you can procedurally generate some simple sounds from within the Unity engine, and connect those sounds with parameters of other objects in your scene"**. The advantage of that is that you have access to parameters that **drive** the audio generation, and not only to the resulting audio. The "disadvantage" is that you must have a good understanding of digital audio inorder to create your own tools / alterations upon this example.  
### 3.2. Screenshots:  
<table style="width:100%;">
  <tr>
    <th>Images</th>
    <th>Descriptions</th>
  </tr>
  <tr>
    <td><img src="/screenshots/3-Procedural-Audio-Screenshots/konsfik-procedural-audio-made-in-unity3d-editor-screenshot-1.jpg" width="400" title="editor screenshot" /> </td>
    <td>Procedural Audio - editor screenshot</td> 
  </tr>
  <tr>
    <td><img src="/screenshots/3-Procedural-Audio-Screenshots/konsfik-procedural-audio-made-in-unity3d-editor-controls-screenshot-1.jpg" width="400" title="editor controls" />  </td>
    <td>Procedural Audio - editor controls</td> 
  </tr>
  <tr>
    <td><img src="/screenshots/3-Procedural-Audio-Screenshots/konsfik-procedural-audio-made-in-unity3d-autoplay-mode-screenshot-1.jpg" width="400" title="autoPlay mode screenshot" /></td>
    <td>Procedural Audio - autoPlay mode</td> 
  </tr>
</table>

# 3.3. Further links:  
demo-video: https://youtu.be/fg0zjFfQJDU  
more info about this example: http://www.konsfik.com/procedural-audio-made-in-unity3d/  
