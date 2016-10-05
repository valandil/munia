﻿using System.Collections.Generic;
using MUNIA.Controllers;

namespace MUNIA.Skins {

	public abstract class Skin {
		public string Name { get; set; }
		public string Path { get; set; }
		public SkinLoadResult LoadResult { get; protected set; }
		public List<ControllerType> Controllers { get; } = new List<ControllerType>();

		public abstract void Render(int width, int height);

		public bool UpdateState(IController controller) {
			var oldState = State;
			State = controller?.GetState();
			return !Equals(oldState, State);
		}
		protected ControllerState State;
	}


	public enum SkinLoadResult {
		Fail,
		Ok,
	}
}
