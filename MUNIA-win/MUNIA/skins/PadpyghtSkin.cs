﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MUNIA.Controllers;
using MUNIA.Util;
using OpenTK.Graphics.OpenGL;

namespace MUNIA.Skins {
	public class PadpyghtSkin : Skin {
		public List<Button> Buttons = new List<Button>();
		public List<Stick> Sticks = new List<Stick>();
		public List<Trigger> Triggers = new List<Trigger>();
		private SizeF _baseDimension = SizeF.Empty;
		private SizeF _dimensions = SizeF.Empty;

		private int _baseTexture;
		private Bitmap _baseImg;

		public void Load(string iniPath) {
			try {
				var pi = new FileInfo(iniPath);
				this.Path = iniPath;
				IniFile ini;
				using (var iniFile = File.OpenRead(iniPath))
					ini = new IniFile(iniFile);

				// cleanup old textures
				Buttons.ForEach(b => {
					if (b.PressedTexture != -1) GL.DeleteTexture(b.PressedTexture);
					if (b.Texture != -1) GL.DeleteTexture(b.Texture);
				});
				Sticks.ForEach(s => { if (s.Texture != -1) GL.DeleteTexture(s.Texture); });
				Triggers.ForEach(t => { if (t.Texture != -1) GL.DeleteTexture(t.Texture); });
				Buttons.Clear();
				Sticks.Clear();
				Triggers.Clear();

				Name = pi.Name;
				foreach (ControllerType t in Enum.GetValues(typeof(ControllerType)))
					Controllers.Add(t);


				var general = ini.GetSection("General");
				// _dimensions = new Size(general.ReadInt("Width"), general.ReadInt("Height"));
				_baseImg = (Bitmap)Image.FromFile(System.IO.Path.Combine(pi.DirectoryName, general.ReadString("File_Background")));
				_baseDimension = _baseImg.Size;
				_baseTexture = TextureHelper.CreateTexture(_baseImg);

				// first process the buttons
				foreach (var sec in ini.Sections) {
					if (sec.Name.StartsWith("Button")) {
						int buttonNum = int.Parse(sec.Name.Substring(6));
						Buttons.EnsureSize(buttonNum);
						Buttons[buttonNum - 1] = ReadIniButton(sec, pi);
					}
				}

				// now we can determine the index for the up/down/left/right hat
				int numButtons = Buttons.Count;
				Buttons.EnsureSize(numButtons + 4);
				Buttons[numButtons] = ReadIniButton(ini.GetSection("Up"), pi);
				Buttons[numButtons+1] = ReadIniButton(ini.GetSection("Down"), pi);
				Buttons[numButtons+2] = ReadIniButton(ini.GetSection("Left"), pi);
				Buttons[numButtons+3] = ReadIniButton(ini.GetSection("Right"), pi);

				// todo: axes

				// CalcBounds();

				LoadResult = SkinLoadResult.Ok;
			}
			catch { LoadResult = SkinLoadResult.Fail; }
		}

		private static Button ReadIniButton(IniFile.IniSection sec, FileInfo pi) {
			if (sec == null) return null;
			var ret = new Button {
				Offset = sec.ReadPoint("Position"),
				Size = sec.ReadSize("Size"),
				Pressed = (Bitmap)Image.FromFile(System.IO.Path.Combine(pi.DirectoryName, sec.ReadString("File_Push")))
			};
			if (ret.Pressed != null)
				ret.PressedTexture = TextureHelper.CreateTexture(ret.Pressed);
			return ret;
		}

		public override void Render(int w, int h) {
			if (_dimensions.Width != w || _dimensions.Height != h) {
				_dimensions = new Size(w, h);
				CalcBounds();
			}
			Render();
		}

		private void CalcBounds() {
			foreach (var button in Buttons) {
				var bounds = new RectangleF(button.Offset, button.Size);
				// center, divide by 2 non-floating
				bounds.Offset(-button.Size.Width / 2, -button.Size.Height / 2);

				var l = Project(bounds.Location, _baseDimension, _dimensions);
				var s = Project(new PointF(bounds.Right, bounds.Bottom), _baseDimension,  _dimensions);
				var boundsScaled = RectangleF.FromLTRB(l.X, l.Y, s.X, s.Y);
				button.Bounds = button.PressedBounds = boundsScaled;
			}
			foreach (var stick in Sticks) {
			}
			foreach (var trigger in Triggers) {
			}
		}

		void Render() {
			List<Tuple<ControllerItem, int>> all = new List<Tuple<ControllerItem, int>>();
			all.AddRange(Buttons.Select((b, idx) => Tuple.Create((ControllerItem)b, idx)));
			all.AddRange(Sticks.Select((b, idx) => Tuple.Create((ControllerItem)b, idx)));
			all.AddRange(Triggers.Select((b, idx) => Tuple.Create((ControllerItem)b, idx)));
			all.Sort((tuple, tuple1) => tuple.Item1.Z.CompareTo(tuple1.Item1.Z));

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Texture2D);

			foreach (var ci in all.Where(x => x.Item1.Z < 0))
				RenderItem(ci.Item1, ci.Item2);

			GL.BindTexture(TextureTarget.Texture2D, _baseTexture);
			TextureHelper.RenderTexture(0, _dimensions.Width, 0, _dimensions.Height);

			foreach (var ci in all.Where(x => x.Item1.Z >= 0))
				RenderItem(ci.Item1, ci.Item2);

			GL.Disable(EnableCap.Blend);
		}
		private void RenderItem(ControllerItem i, int itemidx) {
			if (i is Button) RenderButton(itemidx);
			if (i is Stick) RenderStick(itemidx);
			if (i is Trigger) RenderTrigger(itemidx);
		}

		private void RenderButton(int i) {
			var btn = Buttons[i];
			bool pressed = State != null && State.Buttons[i];
			if (pressed && btn.Pressed != null) {
				GL.BindTexture(TextureTarget.Texture2D, btn.PressedTexture);
				TextureHelper.RenderTexture(btn.PressedBounds);
			}
			else if (!pressed && btn.Bitmap != null) {
				GL.BindTexture(TextureTarget.Texture2D, btn.Texture);
				TextureHelper.RenderTexture(btn.Bounds);
			}
		}
		private void RenderStick(int i) {
			var stick = Sticks[i];
			var r = stick.Bounds;
			float x, y;
			if (State != null) {
				x = State.Axes[stick.HorizontalAxis];
				y = State.Axes[stick.VerticalAxis];
			}
			else {
				x = y = 0f;
			}

			SizeF img = GetCorrectedDimensions(_baseImg.Size);
			x *= img.Width / _dimensions.Width * stick.OffsetScale;
			y *= img.Height / _dimensions.Height * stick.OffsetScale;
			r.Offset(new PointF(x, y));

			GL.BindTexture(TextureTarget.Texture2D, stick.Texture);
			TextureHelper.RenderTexture(r);
		}

		private void RenderTrigger(int i) {
			var trigger = Triggers[i];
			var r = trigger.Bounds;
			float o = State?.Axes[trigger.Axis] ?? 0f;

			SizeF img = GetCorrectedDimensions(_baseImg.Size);
			o *= img.Height / _dimensions.Height * trigger.OffsetScale;

			r.Offset(new PointF(0, o));
			GL.BindTexture(TextureTarget.Texture2D, trigger.Texture);
			TextureHelper.RenderTexture(r);
		}

		private SizeF GetCorrectedDimensions(SizeF dim) {
			// find real width/height, compensating for black box
			float svgAR = _dimensions.Width / _dimensions.Height;
			float imgAR = dim.Width / dim.Height;
			if (svgAR > imgAR)
				dim.Height = dim.Width / svgAR;
			else
				dim.Width = dim.Height * svgAR;
			return dim;
		}
		
		private PointF Project(PointF p, SizeF original, SizeF target) {
			/*float originalAR = original.Width / original.Height;
			float targetAR = target.Width / target.Height;
			if (originalAR > targetAR) {
				// compensate for black box
				p.Y -= ((original.Height - original.Width / originalAR) / 2f);
				// adjust ratio
				original.Height = original.Width / originalAR;
			}
			else {
				// compensate for black box
				p.X -= ((original.Width - original.Height * originalAR) / 2f);
				// adjust ratio
				original.Width = original.Height * originalAR;
			}*/

			var x = p.X / original.Width * target.Width;
			var y = p.Y / original.Height * target.Height;
			return new PointF(x, y);
		}
		
		internal PointF Unproject(PointF p) {
			float targetAR = _dimensions.Width / _dimensions.Height;
			float imgAR = _baseDimension.Width / _baseDimension.Height;
			float width = _baseDimension.Width;
			float height = _baseDimension.Height;
			if (targetAR > imgAR)
				height = width / targetAR;
			else
				width = height * targetAR;

			var x = p.X / _dimensions.Width * width;
			var y = p.Y / _dimensions.Height * height;

			if (targetAR > imgAR)
				y += (_baseDimension.Height - _baseDimension.Width / targetAR) / 2f;
			else
				x += (_baseDimension.Width - _baseDimension.Height * targetAR) / 2f;

			return new PointF(x, y);
		}



		public class ControllerItem {
			public Bitmap Bitmap;
			public RectangleF Bounds;
			public int Z;
			public int Texture = -1;
			public Point Offset;
			public Size Size;
		}
		public class Button : ControllerItem {
			public Bitmap Pressed;
			public int PressedTexture = -1;
			public RectangleF PressedBounds;
		}
		public class Stick : ControllerItem {
			public float OffsetScale;
			public int HorizontalAxis;
			public int VerticalAxis;
		}
		public class Trigger : ControllerItem {
			public float OffsetScale;
			public int Axis;
		}
	}
}
