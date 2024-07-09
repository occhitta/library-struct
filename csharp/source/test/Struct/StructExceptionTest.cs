using Occhitta.Libraries.Common;
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
	[TestCase("構造が正しくありません。", "A,B", 0, 1, """
Occhitta.Libraries.Struct.StructException {
	Reason : "構造が正しくありません。"
	Region : "A,B"
	Offset : 0
	Length : 1
	TargetSite : Null
	Message : "構造が正しくありません。\r\nOffset : 0\r\nLength : 1\r\nRegion : A,B\r\n       : ^"
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : Null
	HResult : -2146233087
	StackTrace : Null
}
""")]
	[TestCase("構造が正しくありません。", "AA=BB", 2, 3, """
Occhitta.Libraries.Struct.StructException {
	Reason : "構造が正しくありません。"
	Region : "AA=BB"
	Offset : 2
	Length : 3
	TargetSite : Null
	Message : "構造が正しくありません。\r\nOffset : 2\r\nLength : 3\r\nRegion : AA=BB\r\n       :   ^^^"
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : Null
	HResult : -2146233087
	StackTrace : Null
}
""")]
	public void Test(string reason, string region, int offset, int length, string expect) {
		Assert.Multiple(() => {
			Assert.That(ExceptionUtilities.ToString(new StructException(reason, region,          offset, length), "\t"), Is.EqualTo(expect));
			Assert.That(ExceptionUtilities.ToString(new StructException(reason, region.AsSpan(), offset, length), "\t"), Is.EqualTo(expect));
		});
	}
}
