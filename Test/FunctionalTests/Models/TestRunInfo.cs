using System.Collections.Generic;

namespace R5.RunInfoBuilder.FunctionalTests.Models
{
	public class TestRunInfo
	{
		public List<string> StringList1 { get; set; }
		public List<int> IntList1 { get; set; }
		public List<int> IntList2 { get; set; }
		public List<string> UnwritableStringList { get; }
		public string String1 { get; set; }
		public bool Bool1 { get; set; }
		public bool Bool2 { get; set; }
		public bool Bool3 { get; set; }
		public int Int1 { get; set; }
		public int Int2 { get; set; }
		public int Int3 { get; set; }
		public bool UnwritableBool { get; }
		public TestCustomType CustomType { get; set; }
	}

	public class TestCustomType
	{

	}
}
