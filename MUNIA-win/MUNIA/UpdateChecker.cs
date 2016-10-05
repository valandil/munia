﻿using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Xml;

namespace MUNIA {
	class UpdateChecker : EventArgs {
		public event EventHandler Connected;
		public event EventHandler AlreadyLatest;
		public event EventHandler<UpdateAvailableArgs> UpdateAvailable;
		public event EventHandler UpdateCheckFailed;
		public event DownloadProgressChangedEventHandler DownloadProgressChanged;

		public const string UpdateCheckHost = "https://munia.io/";
		public const string UpdateCheckPage = "tool/version_check";

		public void CheckVersion() {
			WebClient wc = new WebClient();
			wc.Proxy = null;
			wc.OpenReadCompleted += (sender, args) => Connected(this, Empty);
			wc.DownloadProgressChanged += (sender, args) => DownloadProgressChanged(this, args);
			wc.DownloadStringCompleted += (sender, args) => {
				if (args.Cancelled || args.Error != null) {
					UpdateCheckFailed?.Invoke(this, Empty);
				}
				else {
					try {
						XmlDocument xd = new XmlDocument();
						xd.LoadXml(args.Result);
						var versionNode = xd["version"];
						var version = Version.Parse(versionNode["version_string"].InnerText);
						var releaseDate = DateTime.ParseExact(versionNode["release_date"].InnerText.Trim(), "yyyy'-'M'-'d", null);
						string releaseNotes = versionNode["release_notes"].InnerText;
						string url = versionNode["url"].InnerText;

						var myVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
						if (version > Version.Parse(myVersion.FileVersion))
							UpdateAvailable?.Invoke(this, new UpdateAvailableArgs {
								DownloadUrl = url,
								ReleaseDate = releaseDate,
								ReleaseNotes = releaseNotes,
								Version = version,
							});
						else
							AlreadyLatest?.Invoke(this, Empty);
					}
					catch {
						UpdateCheckFailed?.Invoke(this, Empty);
					}
				}
			};
			// trigger the download..
			wc.DownloadStringAsync(new Uri(UpdateCheckHost + UpdateCheckPage));
		}

	}

	class UpdateAvailableArgs : EventArgs {
		public Version Version { get; set; }
		public string ReleaseNotes { get; set; }
		public DateTime ReleaseDate { get; set; }
		public string DownloadUrl { get; set; }
	}
}
