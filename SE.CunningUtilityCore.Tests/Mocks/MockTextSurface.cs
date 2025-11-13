using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace IngameScript.Tests.Mocks
{
    public class MockTextSurface : IMyTextSurface
    {
        public List<string> Lines { get; } = new List<string>();

        public bool WriteText(string value, bool append = false)
        {
            if (!append)
            {
                Lines.Clear();
            }
            Lines.Add(value);
            return true;
        }

        public bool WriteText(StringBuilder value, bool append = false)
        {
            if (!append)
            {
                Lines.Clear();
            }
            Lines.Add(value.ToString());
            return true;
        }

        public string GetText() => string.Join("\n", Lines);

        public MySpriteDrawFrame DrawFrame()
        {
            throw new NotImplementedException();
        }

        // --- Stub implementations for the rest of the interface ---

        public string CurrentlyShownImage { get => throw new NotImplementedException(); }
        public float FontSize { get; set; }
        public Color FontColor { get; set; }
        public Color BackgroundColor { get; set; }
        public byte BackgroundAlpha { get; set; }
        public float ChangeInterval { get; set; }
        public string Font { get; set; }
        public TextAlignment Alignment { get; set; }
        public string Script { get; set; }
        public ContentType ContentType { get; set; }
        public Vector2 SurfaceSize { get => throw new NotImplementedException(); }
        public Vector2 TextureSize { get => throw new NotImplementedException(); }
        public bool PreserveAspectRatio { get; set; }
        public float TextPadding { get; set; }
        public Color ScriptBackgroundColor { get; set; }
        public Color ScriptForegroundColor { get; set; }
        public string Name { get => throw new NotImplementedException(); }
        public string DisplayName { get => throw new NotImplementedException(); }

        public void AddImagesToSelection(List<string> ids, bool checkExistence = false) => throw new NotImplementedException();
        public void AddImageToSelection(string id, bool checkExistence = false) => throw new NotImplementedException();
        public void ClearImagesFromSelection() => throw new NotImplementedException();
        public void GetFonts(List<string> fonts) => throw new NotImplementedException();
        public void GetScripts(List<string> scripts) => throw new NotImplementedException();
        public void GetSelectedImages(List<string> output) => throw new NotImplementedException();
        public void GetSprites(List<string> sprites) => throw new NotImplementedException();
        public Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale) => throw new NotImplementedException();
        public void ReadText(StringBuilder buffer, bool append = false) => throw new NotImplementedException();
        public void RemoveImageFromSelection(string id, bool removeDuplicates = false) => throw new NotImplementedException();
        public void RemoveImagesFromSelection(List<string> ids, bool removeDuplicates = false) => throw new NotImplementedException();
        public void Dispose() { }
    }
}
