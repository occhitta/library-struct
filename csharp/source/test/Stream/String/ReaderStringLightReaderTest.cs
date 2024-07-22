namespace Occhitta.Libraries.Stream.String;

/// <summary>
/// <see cref="ReaderStringLightReader" />検証クラスです。
/// </summary>
public class ReaderStringLightReaderTest : StringLightReaderTest {
	#region 継承メソッド定義:InvokeTest/UpdateName
	/// <summary>
	/// 引数情報を実行します。
	/// </summary>
	/// <param name="source">要素内容</param>
	/// <param name="action">検証処理</param>
	protected override void InvokeTest(string source, Action<StringLightReader> action) {
		using var system = new StringReader(source);
		using var reader = new ReaderStringLightReader(system);
		action(reader);
	}
	/// <summary>
	/// 要素名称を置換します。
	/// </summary>
	/// <param name="source">要素名称</param>
	/// <returns>置換内容</returns>
	protected override string UpdateName(string source) => source switch {
		"ObjectRoot" => "Occhitta.Libraries.Struct",
		"ObjectName" => "Occhitta.Libraries.Stream.String.ReaderStringLightReader",
		"ObjectCode" => "System.IO.TextReader get_Source()",
		"SourceName" => "System.Private.CoreLib",
		"BufferName" => "buffer",
		"BufferCode" => "(System.Reflection.RuntimeMethodInfo)Void Throw(System.String)",
		"OffsetName" => "index",
		"OffsetCode" => "(System.Reflection.RuntimeMethodInfo)Void ThrowNegative[T](T, System.String)",
		"LengthName" => "count",
		"LengthCode" => "(System.Reflection.RuntimeMethodInfo)Void ThrowNegative[T](T, System.String)",
		"BorderName" => "Null",
		"BorderCode" => "(System.Reflection.RuntimeMethodInfo)Int32 Read(Char[], Int32, Int32)",
		_            => source
	};
	#endregion 継承メソッド定義:InvokeTest/UpdateName

	#region 検証メソッド定義:CreateTest
	/// <summary>
	/// 生成例外を検証します。
	/// </summary>
	/// <param name="source">読込情報</param>
	/// <returns>例外内容</returns>
	[TestCase(null, ExpectedResult = """
System.ArgumentNullException {
	Message : "Value cannot be null. (Parameter 'source')"
	ParamName : "source"
	TargetSite : (System.Reflection.RuntimeConstructorInfo)Void .ctor(System.IO.TextReader)
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "Occhitta.Libraries.Struct"
	HResult : -2147467261
	StackTrace : ...
}
""")]
	public string CreateTest(TextReader source) {
		var result = Assert.Throws<ArgumentNullException>(() => new ReaderStringLightReader(source));
		return ExceptionUtilities.ToString(result, "\t");
	}
	#endregion 検証メソッド定義:CreateTest
}
