using Occhitta.Libraries.Common.UnitTest;

namespace Occhitta.Libraries.Struct;

/// <summary>
/// <see cref="StructException" />検証クラスです。
/// </summary>
[TestFixture]
public sealed class StructExceptionTest {
	/// <summary>
	/// <see cref="StructException(string, string, int, int)" />を検証します。
	/// </summary>
	/// <param name="reason">基本内容</param>
	/// <param name="region">部分情報</param>
	/// <param name="offset">起点番号</param>
	/// <param name="length">要素個数</param>
	/// <param name="expect">想定情報</param>
	[TestCase("構造が正しくありません。", "A,B", 0, 1,
		  @"Occhitta.Libraries.Struct.StructException {
  Range : ""A,B""
  Index : 0
  Count : 1
  TargetSite : Null
  Message : ""構造が正しくありません。\r\nIndex : 0\r\nCount : 1\r\nRange : A,B\r\n      : ^""
  Data : {  }
  InnerException : Null
  HelpLink : Null
  Source : Null
  HResult : -2146233087
  StackTrace : ...
}")]
	[TestCase("構造が正しくありません。", "AA=BB", 2, 3,
		  @"Occhitta.Libraries.Struct.StructException {
  Range : ""AA=BB""
  Index : 2
  Count : 3
  TargetSite : Null
  Message : ""構造が正しくありません。\r\nIndex : 2\r\nCount : 3\r\nRange : AA=BB\r\n      :   ^^^""
  Data : {  }
  InnerException : Null
  HelpLink : Null
  Source : Null
  HResult : -2146233087
  StackTrace : ...
}")]
	public void Test(string reason, string region, int offset, int length, params string[] expect) {
		Assert.Multiple(() => {
			Assert.That(ExceptionTest.ToString(new StructException(reason, region,          offset, length)), Is.EqualTo(String.Join(Environment.NewLine, expect)));
			Assert.That(ExceptionTest.ToString(new StructException(reason, region.AsSpan(), offset, length)), Is.EqualTo(String.Join(Environment.NewLine, expect)));
		});
	}
}
