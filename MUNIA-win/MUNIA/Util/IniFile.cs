using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using OpenTK;

namespace MUNIA.Util {
	public class IniFile {

		public List<IniSection> Sections { get; set; } = new List<IniSection>();
		public IniSection CurrentSection { get; set; }

		public IniFile(Stream s) {
			using (StreamReader sr = new StreamReader(s)) {
				while (!sr.EndOfStream) 
					ProcessLine(sr.ReadLine());
			}
		}

		public IniSection GetSection(string sectionName) {
			return Sections.Find(x => x.Name == sectionName);
		}

		public IniSection GetOrCreateSection(string sectionName, string insertAfter = null) {
			var ret = Sections.Find(x => x.Name == sectionName);
			if (ret == null) {
				int insertIdx = (insertAfter != null) ? Sections.FindIndex(section => section.Name == insertAfter) : -1;

				ret = new IniSection(sectionName);
				if (insertIdx != -1) {
					Sections.Insert(insertIdx, ret);
					ret.Index = insertIdx;
					// move up all section indices
					for (int i = insertIdx + 1; i < Sections.Count; i++)
						Sections[i].Index++;
				}
				else {
					Sections.Add(ret);
					ret.Index = Sections.Count;
				}
			}
			return ret;
		}

		int ProcessLine(string line) {
			IniSection.FixLine(ref line);
			if (line.Length == 0) return 0;

			// Test if this line contains start of new section i.e. matches [*]
			if ((line[0] == '[') && (line[line.Length - 1] == ']')) {
				string sectionName = line.Substring(1, line.Length - 2);
				var iniSection = new IniSection(sectionName, Sections.Count);
				Sections.Add(iniSection);
				CurrentSection = iniSection;
			}
			else if (CurrentSection != null) {
				return CurrentSection.ParseLine(line);
			}
			return 0;
		}

		void SetCurrentSection(string sectionName) {
			CurrentSection = Sections.Find(x => x.Name == sectionName);
		}

		public void SetCurrentSection(IniSection section) {
			if (Sections.Contains(section))
				CurrentSection = section;
			else
				throw new InvalidOperationException("Invalid section");
		}

		public string ReadString(string section, string key, string @default = "") {
			if (CurrentSection == null || CurrentSection.Name != section)
				SetCurrentSection(section);
			return CurrentSection.ReadString(key, @default);
		}

		public bool ReadBool(string key) {
			return CurrentSection.ReadBool(key);
		}

		public bool ReadBool(string section, string key) {
			if (CurrentSection.Name != section)
				SetCurrentSection(section);
			return ReadBool(key);
		}

		public class IniSection {
			public int Index { get; set; }
			public string Name { get; set; }

			public class IniValue {
				private string value;
				public IniValue(string value) {
					this.value = value;
				}

				public override string ToString() {
					return value;
				}
				public static implicit operator IniValue(string value) {
					return new IniValue(value);
				}
				public static implicit operator string(IniValue val) {
					return val.value;
				}
				public void Set(string value) {
					this.value = value;
				}
				public override bool Equals(object obj) {
					return value.Equals(obj.ToString());
				}
				protected bool Equals(IniValue other) {
					return string.Equals(value, other.value);
				}

				public override int GetHashCode() {
					return value?.GetHashCode() ?? 0;
				}

			}

			public Dictionary<string, IniValue> SortedEntries { get; set; }
			public List<KeyValuePair<string, IniValue>> OrderedEntries { get; set; }

			static readonly NumberFormatInfo culture = CultureInfo.InvariantCulture.NumberFormat;

			public IniSection(string name = "", int index = -1) {
				SortedEntries = new Dictionary<string, IniValue>();
				OrderedEntries = new List<KeyValuePair<string, IniValue>>();
				Name = name;
				Index = index;
			}

			public override string ToString() {
				var sb = new StringBuilder();
				sb.Append('[');
				sb.Append(Name);
				sb.AppendLine("]");
				foreach (var v in OrderedEntries) {
					sb.Append(v.Key);
					sb.Append('=');
					sb.AppendLine(v.Value);
				}
				return sb.ToString();
			}

			public void Clear() {
				SortedEntries.Clear();
				OrderedEntries.Clear();
			}

			public int ParseLines(IEnumerable<string> lines) {
				return lines.Sum(ParseLine);
			}

			public int ParseLine(string line) {
				// ignore comments
				if (line[0] == ';') return 0;
				string key;
				int pos = line.IndexOf("=", StringComparison.Ordinal);
				if (pos != -1) {
					key = line.Substring(0, pos);
					string value = line.Substring(pos + 1);
					FixLine(ref key);
					FixLine(ref value);
					SetValue(key, value, false);
					return 1;
				}
				return 0;
			}

			public void SetValue(string key, string value, bool @override = true) {
				if (!SortedEntries.ContainsKey(key)) {
					IniValue val = value;
					OrderedEntries.Add(new KeyValuePair<string, IniValue>(key, val));
					SortedEntries[key] = val;
				}
				else if (@override) {
					SortedEntries[key].Set(value);
					OrderedEntries.RemoveAll(e => e.Key == key);
					OrderedEntries.Add(new KeyValuePair<string, IniValue>(key, value));
				}
			}

			public static void FixLine(ref string line) {
				int start = 0;

				while (start < line.Length && (line[start] == ' ' || line[start] == '\t'))
					start++;

				int end = line.IndexOf(';', start);
				if (end == -1) end = line.Length;

				while (end > 1 && (line[end - 1] == ' ' || line[end - 1] == '\t'))
					end--;

				line = line.Substring(start, Math.Max(end - start, 0));
			}

			public static string FixLine(string line) {
				string copy = line;
				FixLine(ref copy);
				return copy;
			}

			public bool HasKey(string keyName, bool caseSensitive = true) {
				return caseSensitive
					? SortedEntries.ContainsKey(keyName)
					: SortedEntries.Any(e => e.Key.Equals(keyName, StringComparison.OrdinalIgnoreCase));
			}

			static readonly string[] TrueValues = { "yes", "1", "true", "on" };
			static readonly string[] FalseValues = { "no", "0", "false", "off" };

			public bool ReadBool(string key, bool defaultValue = false) {
				string entry = ReadString(key);
				if (TrueValues.Contains(entry, StringComparer.InvariantCultureIgnoreCase))
					return true;
				else if (FalseValues.Contains(entry, StringComparer.InvariantCultureIgnoreCase))
					return false;
				else return defaultValue;
			}

			public string ReadString(string key, string defaultValue = "", bool caseSensitive = true) {
				IniValue ret;
				if (caseSensitive && SortedEntries.TryGetValue(key, out ret))
					return ret;
				else if (!caseSensitive) {
					var v = OrderedEntries.Where(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
					return v.Any() ? v.First().Value.ToString() : defaultValue;
				}
				else
					return defaultValue;
			}

			public int ReadInt(string key, int defaultValue = 0) {
				int ret;
				if (int.TryParse(ReadString(key), out ret))
					return ret;
				else
					return defaultValue;
			}

			public Point ReadXY(string key) {
				string[] val = ReadString(key).Split(',');
				return new Point(int.Parse(val[0]), int.Parse(val[1]));
			}

			public short ReadShort(string key, short defaultValue = 0) {
				short ret;
				if (short.TryParse(ReadString(key), out ret))
					return ret;
				else
					return defaultValue;
			}

			public float ReadFloat(string key, float defaultValue = 0.0f) {
				float ret;
				if (float.TryParse(ReadString(key).Replace(',', '.'), NumberStyles.Any, culture, out ret))
					return ret;
				else
					return defaultValue;
			}

			public double ReadDouble(string key, double defaultValue = 0.0) {
				double ret;
				if (double.TryParse(ReadString(key).Replace(',', '.'), NumberStyles.Any, culture, out ret))
					return ret;
				else
					return defaultValue;
			}

			public T ReadEnum<T>(string key, T @default) {
				if (HasKey(key))
					return (T)Enum.Parse(typeof(T), ReadString(key));
				return @default;
			}

			public List<string> ReadList(string key) {
				return ReadString(key).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
			}

			public string ConcatenatedValues() {
				var sb = new StringBuilder();
				foreach (var v in OrderedEntries)
					sb.Append(v.Value);
				return sb.ToString();
			}

			/// <summary>
			///  returns index of key:value
			/// </summary>
			/// <param name="p"></param>
			/// <returns></returns>
			public int FindValueIndex(string p) {
				for (int i = 0; i < OrderedEntries.Count; i++)
					if (OrderedEntries[i].Value == p)
						return i;
				return -1;
			}

			public void WriteTo(StreamWriter sw) {
				sw.Write('[');
				sw.Write(Name);
				sw.WriteLine(']');
				foreach (var kvp in OrderedEntries) {
					sw.Write(kvp.Key);
					sw.Write('=');
					sw.WriteLine(kvp.Value);
				}
			}

			public Vector3 ReadXYZ(string key) {
				return ReadXYZ(key, new Vector3(0, 0, 0));
			}
			public Vector3 ReadXYZ(string key, Vector3 @default) {
				string size = ReadString(key);
				string[] parts = size.Split(',');
				int x, y, z;
				if (int.TryParse(parts[0], out x) && int.TryParse(parts[1], out y) && int.TryParse(parts[2], out z))
					return new Vector3(x, y, z);
				return @default;
			}

			public Size ReadSize(string key) {
				return ReadSize(key, new Size(0, 0));
			}
			public Size ReadSize(string key, Size @default) {
				string size = ReadString(key);
				string[] parts = size.Split(',');
				int x, y;
				if (int.TryParse(parts[0], out x) && int.TryParse(parts[1], out y))
					return new Size(x, y);
				return @default;
			}

			public Point ReadPoint(string key) {
				return ReadPoint(key, Point.Empty);
			}
			public Point ReadPoint(string key, Point @default) {
				string point = ReadString(key);
				string[] parts = point.Split(',');
				int x, y;
				if (int.TryParse(parts[0], out x) && int.TryParse(parts[1], out y))
					return new Point(x, y);
				return @default;
			}
		}

		public void Save(string filename) {
			var sw = new StreamWriter(filename, false, Encoding.Default, 64 * 1024);
			foreach (var section in Sections) {
				if (section.Name == "#include" && section.OrderedEntries.Count == 0)
					continue;
				section.WriteTo(sw);
				if (section != Sections.Last())
					sw.WriteLine();
			}
			sw.Flush();
			sw.Dispose();
		}

	}
}