﻿using MFPSEditor;
using UnityEditor;

public class ScopeProDocumentation : TutorialWizard
{
    //required//////////////////////////////////////////////////////
    private const string ImagesFolder = "scope-pro/editor/";
    private NetworkImages[] m_ServerImages = new NetworkImages[]
    {
        new NetworkImages{Name = "img-0.png", Image = null},
        new NetworkImages{Name = "img-1.jpg", Image = null},
        new NetworkImages{Name = "img-2.png", Image = null},
        new NetworkImages{Name = "img-3.png", Image = null},
        new NetworkImages{Name = "img-4.png", Image = null},
        new NetworkImages{Name = "img-5.png", Image = null},
        new NetworkImages{Name = "https://img.youtube.com/vi/thJkYJ64x0Y/0.jpg", Image = null, Type = NetworkImages.ImageType.Custom},
    };
    private readonly GifData[] AnimatedImages = new GifData[]
    {
        new GifData{ Path = "createwindowobj.gif" },
        new GifData{ Path = "addnewwindowfield.gif" },
        new GifData{ Path = "createwindowbutton.gif" },
        new GifData{ Path = "addonintegrateprevw.gif"},
    };
    private Steps[] AllSteps = new Steps[] {
     new Steps { Name = "Resume", StepsLenght = 0, DrawFunctionName = nameof(ResumeDoc) },
    new Steps { Name = "Scope Pro", StepsLenght = 0, DrawFunctionName = nameof(ScopeProDoc) },
    new Steps { Name = "Dot Sight", StepsLenght = 0, DrawFunctionName = nameof(RedDotDoc) },
    new Steps { Name = "URP / HDRP", StepsLenght = 0, DrawFunctionName = nameof(DrawURPSupport) },
     new Steps { Name = "Notes", StepsLenght = 0, DrawFunctionName = nameof(NotesDoc) },
    };

    public override void WindowArea(int window)
    {
        AutoDrawWindows();
    }
    //final required////////////////////////////////////////////////

    public override void OnEnable()
    {
        base.OnEnable();
        base.Initizalized(m_ServerImages, AllSteps, ImagesFolder, AnimatedImages);
    }

    void ResumeDoc()
    {
        DrawText("Scope Pro has 2 scope/sights techniques to use depend on your scope/sight type, these are the <b>Scope Render</b> and the <b>Dot Sight</b>.\n\n<b><size=16>Scope Render:</size></b>\n\nMostly use for scope's of large range as Sniper scopes, Acog scopes or any other middle/large range scopes, this system use an independent camera to display in a Render Texture in the scope glass mesh, this allows to zoom-in only in the scope camera to be more realistic, also the custom shader comes with a scope depth inner effect.");

        DrawText("<b><size=16>Dot Sight:</size></b>\n\nThis is a just a material with a shader that simulate realistically the effect of a Red dot sight glass, you can change the middle dot icon, glass tint color and the opacity of the glass; this system is used for close range sights like: Cobra, Red Dot, Holo Sight, etc...\n\nThe usage of this is really simple since you just have to replace the material of your sight glass mesh.");

        DrawYoutubeCover("Scope Pro Video", GetServerImage(6), "https://www.youtube.com/watch?v=thJkYJ64x0Y");
    }

    void ScopeProDoc()
    {
        DrawText("In order to setup a Scope Pro you need to do a step manually, since almost every scope/sight model has a different UV mapped, instead of use the default scope glass mesh you will use the glass mesh that comes with the package, so first remove or disable the Glass mesh of your scope model <i>(From the FPWeapon)</i>");
        DrawServerImage(0);
        DownArrow();
        DrawText("Then drag and place the Glass prefab from the addon folder located at: <i>Assets->Addons->ScopePro->Art->Models-><b>scope-glass</b></i> in the same position where the default glass mesh was.");
        DrawServerImage(1);
        DownArrow();
        DrawText("Next, you have to add the script <b>bl_ScopePro.cs</b> in the scope object <i>(not the glass object)</i> and follow the instruction in the inspector of the script for the finish setup.");
        DrawServerImage(2);
        DrawText("That's, your weapon scope is ready to use.");
    }

    void RedDotDoc()
    {
        DrawText("Use the <b>Red dot</b> is much more simple since you only have to replace the material of your scope/sight mesh.\n\nThe only requisite is that your<b> glass mesh have to be separated from the scope/sight body mesh</b>, also there maybe the case where the glass mesh have a custom UV which will prevent the red dot material to show up correctly, if this is your case you can use one of the meshes that comes in the asset, place in the scope/sight and set the material that you want.");

        DrawServerImage(4);
    }

    void DrawURPSupport()
    {
        DrawText("ScopePro by default is set up for the Unity built-in render pipeline, if you are using URP or HDRP you may encounter the issue of the scope image not appearing or it showing as pink, that is because the default shaders don't work for the other render pipelines.\n  \nSince version 1.2.8 the addon comes with the shaders for each render pipeline but you have to manually change the scope shaders to the corresponding render pipeline; this is what you have to do:\n  \n1. Select all your Scope Pro materials, unless you have moved, all the Scope Pro materials are located in the folder: <i>Assets ➔ Addons ➔ ScopePro ➔ Art ➔ Material ➔ Scopes➔*</i>, select all of them.\n  \n2. With all the materials selected, go to the inspector window, in the material shader selection field > select the shader corresponding to the render pipeline in use at: <b>MFPS > Sights > Scope Pro URP</b> or <b>Scope Pro HDRP</b>.");
        DrawServerImage("img-15.png");

        DrawNote("If the scope doesn't show correctly in runtime, play with the scope pro material until you get the desired results, you can do this by selecting the fp weapon in the hierarchy window <i>(while playing in the editor)</i> > select the scope > select the Scope Pro object > in the inspector window you will see all the material values.");

        DrawNote("If the Scope Pro URP or Scope Pro HDRP doesn't appear, install the Shader Graph package from the Unity Package Manager, in the top editor menu > Window > Package Manager > <i>(Unity Registre)</i> Search and Install the <b>Shader Graph</b> package.");
    }

    void NotesDoc()
    {
        DrawText("With the <b>Scope Pro</b> you may get the issue that or the <b>scope glass looks completely black</b> or <b>the render borders are stretched</b> this is happen because the shader calculate the view based on the camera world position, so depend of how close or far the scope is from the player camera the render will be react different.\n\nThe fix for that is simple tho, you just have to modify some values in the Scope Material, for it select the scope where you attach the bl_ScopePro script, in the inspector of that script you should see the material properties.\n\nIn case your problem is that the scope looks completely black simple modify the Vignette Values until you get the desire results.\n\nIf your problem is the stretched borders, modify the <b>Render Texture Tiling and Offset</b> values until you get the desire results.");
        DrawServerImage(3);
        DrawText("You can change these in runtime, simple aim with the weapon -> open the pause menu <i>(escape) </i>-> Go to the Scope -> modify the values.");
    }

    [MenuItem("MFPS/Tutorials/Scope Pro")]
    private static void Open()
    {
        EditorWindow.GetWindow(typeof(ScopeProDocumentation));
    }

    [MenuItem("MFPS/Addons/Scope Pro/Documentation")]
    private static void Open2()
    {
        EditorWindow.GetWindow(typeof(ScopeProDocumentation));
    }
}