using System.Collections.Generic;

namespace ExtraManager
{
    public static class UtilityConstants
    {
		public static readonly List<(string Name, string Version)> Dependencies = new List<(string, string)>
		{
			("RAGENativeUI.dll", "1.0.2.0"),
			("Venoxity.Common.dll", "1.0.3.0"),
		};
	}
}
