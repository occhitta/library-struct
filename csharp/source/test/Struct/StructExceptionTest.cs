namespace Occhitta.Libraries.Struct;

/// <summary>
/// <see cref="StructException" />検証クラスです。
/// </summary>
[TestFixture]
public sealed class StructExceptionTest {
	#region 内部メソッド定義:Pattern1/Pattern2
	/// <summary>
	/// 検証情報を生成します。
	/// <para>当該返却値は以下の検証メソッドで利用します。
	///   <para><see cref="Pattern1A(string, string, int, int)"/></para>
	///   <para><see cref="Pattern1B(string, string, int, int)"/></para>
	/// </para>
	/// </summary>
	/// <returns>検証情報</returns>
	private static IEnumerable<TestCaseData> Pattern1() {
		yield return new TestCaseData("DSV項目の形式が正しくありません。", " \"\" ", 1, 2) {
			ExpectedResult = """
Occhitta.Libraries.Struct.StructException {
	Reason : "DSV項目の形式が正しくありません。"
	Region : " \"\" "
	Offset : 1
	Length : 2
	TargetSite : Null
	Message : "DSV項目の形式が正しくありません。\r\nOffset : 1\r\nLength : 2\r\nRegion :  \"\" \r\n       :  ^^"
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : Null
	HResult : -2146233087
	StackTrace : Null
}
"""};
	}
	/// <summary>
	/// 検証情報を生成します。
	/// <para>当該返却値は以下の検証メソッドで利用します。
	///   <para><see cref="Pattern2A(string, string, int, int, Exception)"/></para>
	///   <para><see cref="Pattern2B(string, string, int, int, Exception)"/></para>
	/// </para>
	/// </summary>
	/// <returns>検証情報</returns>
	private static IEnumerable<TestCaseData> Pattern2() {
		yield return new TestCaseData("DSV項目の形式が正しくありません。", "\"A\",\"B\"", 4, 3, new StructException("整数変換に失敗しました。", "\"B\"", 1, 1)) {
			ExpectedResult = """
Occhitta.Libraries.Struct.StructException {
	Reason : "DSV項目の形式が正しくありません。"
	Region : "\"A\",\"B\""
	Offset : 4
	Length : 3
	TargetSite : Null
	Message : "DSV項目の形式が正しくありません。\r\nOffset : 4\r\nLength : 3\r\nRegion : \"A\",\"B\"\r\n       :     ^^^"
	Data : {
	}
	InnerException : Occhitta.Libraries.Struct.StructException {
		Reason : "整数変換に失敗しました。"
		Region : "\"B\""
		Offset : 1
		Length : 1
		TargetSite : Null
		Message : "整数変換に失敗しました。\r\nOffset : 1\r\nLength : 1\r\nRegion : \"B\"\r\n       :  ^"
		Data : {
		}
		InnerException : Null
		HelpLink : Null
		Source : Null
		HResult : -2146233087
		StackTrace : Null
	}
	HelpLink : Null
	Source : Null
	HResult : -2146233087
	StackTrace : Null
}
"""};
	}
	#endregion 内部メソッド定義:Pattern1/Pattern2

	#region 検証メソッド定義:Pattern1A/Pattern1B
	/// <summary>
	/// <see cref="StructException(string, string, int, int)" />を検証します。
	/// </summary>
	/// <param name="reason">基本内容</param>
	/// <param name="region">部分情報</param>
	/// <param name="offset">起点番号</param>
	/// <param name="length">要素個数</param>
	/// <returns>例外内容</returns>
	[TestCaseSource(nameof(Pattern1))]
	public string Pattern1A(string reason, string region, int offset, int length) {
		var actual = new StructException(reason, region, offset, length);
		return ExceptionUtilities.ToString(actual, "\t");
	}
	/// <summary>
	/// <see cref="StructException(string, ReadOnlySpan{char}, int, int)" />を検証します。
	/// </summary>
	/// <param name="reason">基本内容</param>
	/// <param name="region">部分情報</param>
	/// <param name="offset">起点番号</param>
	/// <param name="length">要素個数</param>
	/// <returns>例外内容</returns>
	[TestCaseSource(nameof(Pattern1))]
	public string Pattern1B(string reason, string region, int offset, int length) {
		var actual = new StructException(reason, region.AsSpan(), offset, length);
		return ExceptionUtilities.ToString(actual, "\t");
	}
	#endregion 検証メソッド定義:Pattern1A/Pattern1B

	#region 検証メソッド定義:Pattern2A/Pattern2B
	/// <summary>
	/// <see cref="StructException(string, string, int, int, Exception)" />を検証します。
	/// </summary>
	/// <param name="reason">基本内容</param>
	/// <param name="region">部分情報</param>
	/// <param name="offset">起点番号</param>
	/// <param name="length">要素個数</param>
	/// <param name="source">関連例外</param>
	/// <returns>例外内容</returns>
	[TestCaseSource(nameof(Pattern2))]
	public string Pattern2A(string reason, string region, int offset, int length, Exception source) {
		var actual = new StructException(reason, region, offset, length, source);
		return ExceptionUtilities.ToString(actual, "\t");
	}
	/// <summary>
	/// <see cref="StructException(string, ReadOnlySpan{char}, int, int, Exception)" />を検証します。
	/// </summary>
	/// <param name="reason">基本内容</param>
	/// <param name="region">部分情報</param>
	/// <param name="offset">起点番号</param>
	/// <param name="length">要素個数</param>
	/// <param name="source">関連例外</param>
	/// <returns>例外内容</returns>
	[TestCaseSource(nameof(Pattern2))]
	public string Pattern2B(string reason, string region, int offset, int length, Exception source) {
		var actual = new StructException(reason, region.AsSpan(), offset, length, source);
		return ExceptionUtilities.ToString(actual, "\t");
	}
	#endregion 検証メソッド定義:Pattern2A/Pattern2B
}
